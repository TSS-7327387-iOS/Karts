using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.AI;

public class Shield : MonoBehaviour
{
    public Minions[] Mininos;
    public MiniPlayers[] MininosForPlayers;
    public float duration;
    public AudioClip clip;
    bool used;


    BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
        StartCoroutine(ColliderEnable());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(used)
            return;
        if (other.TryGetComponent<Pickups>(out Pickups pk))
        {
            used = true;
            if (pk.character.isEnemy)
            {
                foreach (var mini in Mininos)
                {
                    var minion = Instantiate(mini, RandomPoint(pk.transform.position, 2, pk.transform), pk.transform.rotation);
                    minion.InitializedMinion(pk.character);
                    pk.MinionSpwaned(minion, duration);
                }
            }
            else
            {
                foreach (var mini in MininosForPlayers)
                {
                    var minion = Instantiate(mini, RandomPoint(pk.transform.position, 2, pk.transform), pk.transform.rotation);
                    minion.Initialize(pk.character);
                    pk.PlayersMinionSpwaned(minion, duration);
                }
            }
            if (PlayerPrefs.GetInt("Viberation") == 1 && !pk.character.isEnemy)
                MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
            if(!pk.character.isEnemy) AudioManager.inst.PlayPopup(clip);
            Destroy(gameObject);
        }
    }
    Vector3 RandomPoint(Vector3 center, float range,Transform obj)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return obj.position + (transform.forward * 2);
    }

    IEnumerator ColliderEnable()
    {
        yield return new WaitForSeconds(0.1f);
        _boxCollider.enabled = true;
    }
}
