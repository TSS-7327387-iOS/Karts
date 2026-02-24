using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BotPlayerDetection : MonoBehaviour
{
    [SerializeField]BotAI bot;
    private void Start()
    {
        if (bot == null)
            bot = GetComponentInParent<BotAI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == this.transform || !bot.isAlive)
            return;

        if (other.TryGetComponent<Character>(out Character charac))
        {
            if (charac.isBot || charac.isAnimatingPower ||!charac.isAlive)
                return;

            if (!bot.nearbyAgents.Contains(other.transform))
                bot.nearbyAgents.Add(other.transform);

            bot.nearbyAgents.Sort((a, b) =>
                Vector3.SqrMagnitude(b.position - transform.position)
                    .CompareTo(
                        Vector3.SqrMagnitude(a.position - transform.position)
                    )
            );
            bot.ChaseTarget();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == this.transform || !bot.isAlive)
            return;
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            if (bot.nearbyAgents.Contains(other.transform))
            {
                bot.nearbyAgents.Remove(other.transform);
                if (bot.nearbyAgents.Count == 0)
                {
                    bot.ChangeStateToWander();
                }
            }

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            
            bot.animator.SetInteger("Attack", 0);
            
        }
    }
}
