using System;
using System.Collections;
using System.Linq;
using PowerslideKartPhysics;
using UnityEngine;

namespace Tarodev
{
    public class Missile : MonoBehaviour
    { [Header("REFERENCES")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Target _target;
        [SerializeField] private GameObject _explosionPrefab;

        [Header("MOVEMENT")]
        [SerializeField] private float _speed = 15;
        [SerializeField] private float _rotateSpeed = 95;

        [Header("PREDICTION")]
        [SerializeField] private float _maxDistancePredict = 100;
        [SerializeField] private float _minDistancePredict = 5;
        [SerializeField] private float _maxTimePrediction = 5;
        private Vector3 _standardPrediction, _deviatedPrediction;

        [Header("DEVIATION")]
        [SerializeField] private float _deviationAmount = 50;
        [SerializeField] private float _deviationSpeed = 2;
        private bool _hasLockedTarget = false;
        
        [Header("Shader Change On Hit")]
        [SerializeField] private Shader hitShader;
        

        private void Awake()
        {
            StartCoroutine(DestroyMissileAfterTime(7f)); // Start coroutine in Awake
        }
        private void Start()
        {

           //AssignNearestTarget();
           if (!_hasLockedTarget)
           {
               AssignNearestTarget();
           }
        }


        private void AssignNearestTarget()
        { GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Target bestTarget = null;

            float shortestDistance = Mathf.Infinity;
            float maxLockDistance = 50f;     // realistic lock distance
            float maxLockAngle = 90f;        // realistic front angle (wider than before)

            foreach (GameObject enemy in enemies)
            {
                Kart kart = enemy.GetComponent<Kart>();
                if (kart != null && !kart.isExploded)
                {
                    Target target = kart.GetComponentInParent<Target>();
                    if (target != null)
                    {
                        Vector3 toTarget = target.transform.position - transform.position;
                        float distance = toTarget.magnitude;

                        if (distance > maxLockDistance) continue; // skip too far targets

                        float angle = Vector3.Angle(transform.forward, toTarget.normalized);

                        if (angle <= maxLockAngle)
                        {
                            if (distance < shortestDistance)
                            {
                                shortestDistance = distance;
                                bestTarget = target;
                            }
                        }
                    }
                }
            }

            if (bestTarget != null)
            {
                _target = bestTarget;
                _hasLockedTarget = true;
            }

        }



        private void FixedUpdate()
        {
            if (_target == null && !_hasLockedTarget)
            {
                AssignNearestTarget();
            }

            Vector3 targetDirection;

            if (_target != null)
            {
                float leadTimePercentage = Mathf.InverseLerp(
                    _minDistancePredict,
                    _maxDistancePredict,
                    Vector3.Distance(transform.position, _target.transform.position));

                PredictMovement(leadTimePercentage);
                AddDeviation(leadTimePercentage);
                targetDirection = (_deviatedPrediction - transform.position).normalized;
            }
            else
            {
                // No target: go straight
                targetDirection = transform.forward;
            }

            RotateRocket(targetDirection);
            ApplyForwardThrust();
        }

        private void PredictMovement(float leadTimePercentage)
        {
            float predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
            _standardPrediction = _target.Rb.position + _target.Rb.velocity * predictionTime;
        }

        private void AddDeviation(float leadTimePercentage)
        {
            Vector3 deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
            Vector3 predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
            _deviatedPrediction = _standardPrediction + predictionOffset;
        }

      
        private void RotateRocket(Vector3 targetDirection)
        {
            // Calculate the rotation needed to face the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate toward the target
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(newRotation);
        }

        private void ApplyForwardThrust()
        {
            // Use force for a more natural missile push
            Vector3 force = transform.forward * _speed;
            _rb.velocity = force; // You can use AddForce(force, ForceMode.Acceleration) for more physics-based feel
        }
        [Header("Spin")]
        public Kart.SpinAxis kartSpin = Kart.SpinAxis.Yaw;
        public int kartSpinCount = 2;

        private Kart _firingKart; // Store reference to the kart that fired this missile

        public void Initialize(Kart firingKart)
        {
            _firingKart = firingKart; // Assign the firing kart when the missile is instantiated
        }

        private void OnCollisionEnter(Collision collision)
        {
           
            Kart hitKart = collision.gameObject.transform.GetTopmostParentComponent<Kart>();

            if (hitKart != null && hitKart != _firingKart)
            {
                if (!hitKart.isShieldActivated)
                {
                    Debug.Log("Missile collided with " + collision.gameObject.name);
                    hitKart.SpinOut(kartSpin, kartSpinCount);

                    if (hitKart.damageParticlesAll != null)
                    {
                        hitKart.takeDamage_HealthSlider(0.2f);
                        hitKart.ApplyTemporaryHitShader(hitShader, 3f);
                    }

                    // if (hitShader != null)
                    // {
                    //     // StartCoroutine(ChangeKartShaderTemporarily(hitKart, 3f));
                    //     hitKart.ApplyTemporaryHitShader(hitShader, 3f);
                    // }
                }

                if (AudioManagerNew.instance != null)
                {
                    AudioManagerNew.instance.PlaySound("Missile");
                }

                if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                // Hit wall or other object
                if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        private IEnumerator ChangeKartShaderTemporarily(Kart kart, float duration)
        {
            
            Renderer[] renderers = kart.GetComponentsInChildren<Renderer>();

            // Store original materials
            Material[] originalMaterials = new Material[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                // Duplicate original material to store
                originalMaterials[i] = new Material(renderers[i].material);

                // Create a temporary material with the hit shader
                Material tempMat = new Material(renderers[i].material);
                tempMat.shader = hitShader;
                renderers[i].material = tempMat;
            }

            yield return new WaitForSeconds(duration);

            // Restore original materials
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = originalMaterials[i];
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _standardPrediction);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
        }

        private void OnDestroy()
        {
            Debug.Log("missile Destroyed");
        }
        private IEnumerator DestroyMissileAfterTime(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
    
    
}




