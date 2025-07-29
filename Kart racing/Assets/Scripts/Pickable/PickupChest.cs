
using UnityEngine;

public class PickupChest : Pickable
{
    GameObject reward;
    public GameObject[] rewards;
    private void Start()
    {
        InitializePickable();
        reward = rewards[Random.Range(0, rewards.Length)];
    }

    public override void GiveReward()
    {

        Instantiate(reward,transform.position,transform.rotation);
        Instantiate(chestDestroyVfx, transform.position, transform.rotation);
    }
    
}
