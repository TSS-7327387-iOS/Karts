using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float speed,duration;
    public AudioClip clip;

    BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
        StartCoroutine(ColliderEnable());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Pickups>(out Pickups pk))
        {
            pk.StartSpeed(duration,speed);
            if (PlayerPrefs.GetInt("Viberation") == 1 && !pk.character.isEnemy)
                MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
            if(!pk.character.isEnemy) AudioManager.inst.PlayPopup(clip);
            Destroy(gameObject);
        }
    }

    IEnumerator ColliderEnable()
    {
        yield return new WaitForSeconds(0.1f);
        _boxCollider.enabled = true;
    }
}
