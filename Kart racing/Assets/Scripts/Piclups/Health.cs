using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class Health : MonoBehaviour
{
    public AudioClip clip;
    public int _health;

    BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
        StartCoroutine(ColliderEnable());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character pk))
        {
            if (!pk.isBot)
            {
                pk.health += _health;
                pk.UpdateHealthbar();
                if (PlayerPrefs.GetInt("Viberation") == 1 && !pk.isEnemy)
                    MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
                if(!pk.isEnemy) AudioManager.inst.PlayPopup(clip);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ColliderEnable()
    {
        yield return new WaitForSeconds(0.1f);
        _boxCollider.enabled = true;
    }
}
