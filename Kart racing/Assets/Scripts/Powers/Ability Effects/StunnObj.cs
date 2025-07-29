using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnObj : MonoBehaviour
{
    public Character character;
    public float duration;
    public void InitializeData(Character ch, float val)
    {
        character = ch;
        duration = val;
        Destroy(gameObject,val);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Character>(out Character chhh))
        {
            if(chhh==character || chhh.isBot)
                return;
            chhh.power.StunnEffect(duration);
        }
    }
}
