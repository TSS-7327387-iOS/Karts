using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    CharacterController controller;
    ThirdPersonController thirdPersonController;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fall"))
        {
            controller.enabled = false;
            Transform spwanPoint = GameManager.Instance.playerSpwanPoints[Random.Range(0, GameManager.Instance.playerSpwanPoints.Length - 1)];
            transform.SetPositionAndRotation(spwanPoint.position, spwanPoint.rotation);
            controller.enabled = true;
            thirdPersonController.player.canPlayerPick = true;
            //Debug.Log("Fall");
        }
    }
}
