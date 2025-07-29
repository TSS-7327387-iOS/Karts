using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRefillVfx : MonoBehaviour
{
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.player.transform.position + new Vector3(0f, 0.5f, 0f), Time.deltaTime * 2f);

        if(transform.position == GameManager.Instance.player.transform.position + new Vector3(0f, 0.5f, 0f))
        {
            Destroy(gameObject, 0.5f);
        }
    }

}
