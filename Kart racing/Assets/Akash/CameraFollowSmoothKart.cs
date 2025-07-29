using PowerslideKartPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollowSmoothKart : MonoBehaviour
{
        public Camera myCam;
        public float defaultSmoothSpeed= 3.77f;
        public List<Vector3> DefaultOffsetVals = new List<Vector3>(new Vector3[5]);

        public Transform target;
        public float smoothSpeed = 0.125f;//the faster is to follow faster target ,slower is follow slower target b/w 0 to 1
        public Vector3 offset;
    public ParticleSystem speedParticles;
  
    
    private void Awake()
    {
      
    }
    private void Start()
   
    {
        myCam = GetComponent<Camera>();


    }
    /*  void LateUpdate()
          {
          if (target == null) return;

              Vector3 desiredPosition = target.position + offset;
              Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
              transform.position = smoothedPosition;

              transform.LookAt(target);

          }*/
    void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position behind the car
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 10f);

        // Smoothly rotate the camera to always look in the car's forward direction
        Quaternion desiredRotation = Quaternion.LookRotation(target.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime * 10f);
    }

    public void changeOffsetVal(Vector3 offsetVal)
        {
            offset = offsetVal;
        }
    public void changeSmoothSpeed(float smoothSpeedVal)
    {
        smoothSpeed = smoothSpeedVal;
    }
    public void changeCameraTarget(Transform targetVal)
    {
        target = targetVal;
    }
    public void DoFov(float endValue,float duration)
    {
        myCam.DOFieldOfView(endValue, duration);
    }
  
    public void playSpeedParticles()
    {
        speedParticles.Play();
    }
    public void stopSpeedParticles()
    {
        speedParticles.Stop();
    }
}
