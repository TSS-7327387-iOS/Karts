using System;
using System.Collections;
using System.Linq;
using PowerslideKartPhysics;
using UnityEngine;

namespace Tarodev
{
    public class Missile : MonoBehaviour
    {
        [Header("REFERENCES")]
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

        private void Awake()
        {
            StartCoroutine(DestroyMissileAfterTime(7f)); // Start coroutine in Awake
        }
        private void Start()
        {

            AssignNearestTarget();
           
        }


        private void AssignNearestTarget()
        {


            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            Target nearestTarget = null;
            float shortestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject enemy in enemies)
            {
                Kart kart = enemy.GetComponent<Kart>();
                if (kart != null && kart.isExploded == false) // Check if the kart is not exploded
                {
                    Target target = kart.GetComponentInParent<Target>();
                    if (target != null)
                    {
                        float distance = Vector3.Distance(currentPosition, target.transform.position);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            nearestTarget = target;
                        }
                    }
                }
            }
           /* foreach (GameObject enemy in enemies)
            {

                Target target = enemy.GetComponentInChildren<Target>();
                if (target != null)
                {
                    float distance = Vector3.Distance(currentPosition, target.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestTarget = target;
                    }
                }
            }*/

            if (nearestTarget != null)
            {
                _target = nearestTarget;
            }
        }
        /*     private void AssignNearestTarget()
             {
                 GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                 Target nearestTarget = null;
                 float shortestDistance = Mathf.Infinity;
                 Vector3 currentPosition = transform.position;

                 int playerCurrentID = MyGameManager.instance.playerLapCounter.curntID; // Get the player's waypoint ID

                 foreach (GameObject enemy in enemies)
                 {
                     Kart kart = enemy.GetComponent<Kart>();
                     if (kart != null && kart.isExploded==false) // Check if the kart is not exploded
                     {
                         int enemyCurrentID = MyGameManager.instance.enemiesLapCounter
                             .FirstOrDefault(e => e.gameObject == enemy)?.curntID ?? -1;

                         if (enemyCurrentID > playerCurrentID) // Only target enemies ahead of the player
                         {
                             Target target = enemy.GetComponentInChildren<Target>();
                             if (target != null)
                             {
                                 float distance = Vector3.Distance(currentPosition, target.transform.position);
                                 if (distance < shortestDistance)
                                 {
                                     shortestDistance = distance;
                                     nearestTarget = target;
                                 }
                             }
                         }
                     }
                 }

                 if (nearestTarget != null)
                 {
                     _target = nearestTarget;
                 }
             }
     */



        private void FixedUpdate()
        {
            //if (_target == null) return;
            if (_target == null)
            {
                AssignNearestTarget(); // Immediately re-assign if the target is lost
                return;
            }

            _rb.linearVelocity = transform.forward * _speed;

            float leadTimePercentage = Mathf.InverseLerp(
                _minDistancePredict,
                _maxDistancePredict,
                Vector3.Distance(transform.position, _target.transform.position));

            PredictMovement(leadTimePercentage);
            AddDeviation(leadTimePercentage);
            RotateRocket();
        }

        private void PredictMovement(float leadTimePercentage)
        {
            float predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
            _standardPrediction = _target.Rb.position + _target.Rb.linearVelocity * predictionTime;
        }

        private void AddDeviation(float leadTimePercentage)
        {
            Vector3 deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
            Vector3 predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
            _deviatedPrediction = _standardPrediction + predictionOffset;
        }

        private void RotateRocket()
        {
            Vector3 heading = _deviatedPrediction - transform.position;
            Quaternion rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
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
           

           // if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            Kart hitKart = collision.gameObject.transform.GetTopmostParentComponent<Kart>();

            if (hitKart != null && hitKart != _firingKart) // Ensure it's not the launching kart
            {
                if (!hitKart.isShieldActivated)
                { Debug.Log("Missile collided with " + collision.gameObject.name);
                    hitKart.SpinOut(kartSpin, kartSpinCount);
                    if(hitKart.damageParticlesAll != null)//Decrease health bar in player UI
                    {
                        hitKart.takeDamage_HealthSlider(0.2f);
                    }
                }
                if (AudioManagerNew.instance != null)
                {
                    AudioManagerNew.instance.PlaySound("Missile");
                }
                if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        /* private void OnCollisionEnter(Collision collision)
         {
             Debug.Log("missile collision enter" + collision.gameObject.name);
             if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
             if (!collision.gameObject.transform.GetTopmostParentComponent<Kart>().isShieldActivated)
             {
                 collision.gameObject.transform.GetTopmostParentComponent<Kart>().SpinOut(kartSpin, kartSpinCount);
                 Destroy(gameObject);
             }
             //  if (collision.transform.TryGetComponent<IExplode>(out var ex)) ex.Explode();
             // Destroy(gameObject);
         }*/

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




