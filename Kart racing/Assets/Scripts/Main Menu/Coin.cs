using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool canUsed=false;
    public AudioClip sound;
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce((Random.insideUnitSphere + Vector3.up)*5,ForceMode.Impulse);
        Invoke(nameof(CanUsed),1);
    }
    void CanUsed()
    {
        canUsed = true;
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if(!canUsed)
            return;
        if (other.TryGetComponent<Character>(out Character ch))
        {
            if (!ch.isBot)
            {
                ch.AddCoin();
                AudioManager.inst.PlayPopup(sound);
                Destroy(gameObject);
            }
        }
    }
}
