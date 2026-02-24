using UnityEngine;
using UnityEngine.UIElements;

public class BotDetection : MonoBehaviour
{
    EnemyMovement move;
    public bool isEnemy=true;
    Player player;
    CharacterController cc;
    private void Start()
    {
        if (isEnemy) move = GetComponentInParent<EnemyMovement>();
        else
        {
            player = GetComponentInParent<Player>();
            cc = player.GetComponentInParent<CharacterController>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<BotAI>(out BotAI bb))
        {
            if (isEnemy)
            {
                
                    move.botHere = bb.isAlive;
                    move.botChasing = bb.isAlive ? bb : null;
                
                return;

            }
            else
            {
                player.kickBot = bb.isAlive;
                player.bot = bb.isAlive ? bb : null;

                player.detectPlayer.detectTarget = bb.isAlive ? bb.transform : null;

                if (cc.velocity.magnitude < 0.1f &&  bb._name != "devil")
                    player.controller.transform.LookAt(bb.transform);  //Put lookRotation method here
                return;

            }
        }
        if (other.TryGetComponent<Pickable>(out Pickable pk))
        {
            if (isEnemy)
            {
                move.pickUpHere = true;
                move.pickTarget = pk;
            }
            else
            {
                player.pickupHere = true;
                player.pickup = pk;
                player.detectPlayer.detectTarget = pk.transform;
            }
        }
   /*     if (isEnemy) move.botHere = false;
        else player.kickBot = false;*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            if (isEnemy)
            {
                move.botHere = false;
                move.botChasing = null;
            }
            else
            {
                player.kickBot = false;
                player.bot = null;
                player.detectPlayer.detectTarget = null;
            }
        }
        if (other.TryGetComponent<Pickable>(out Pickable pk))
        {
            if (isEnemy)
            {
                if (move.pickUpHere || move.pickTarget==pk)
                {
                    move.pickUpHere = false;
                    move.pickTarget = pk;
                }
                else
                {
                    player.pickupHere = false;
                    player.pickup = null;
                    player.detectPlayer.detectTarget = null;
                }
            }
        }
    }
}
