
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pickable : MonoBehaviour
{
    public float _health;
    public Image healthBar;
    public GameObject chestDestroyVfx;
    float startHealth;
    public void InitializePickable()
    {
        startHealth = 1/_health;
        healthBar.fillAmount = 1f;
    }

    private void UpdateHealth(float demage)
    {
        healthBar.fillAmount -= (startHealth * demage);

        if (_health <= 0)
        {
            Instantiate(chestDestroyVfx, transform.position, transform.rotation);

            GiveReward();
            GameManager.Instance.enemyManager.PickupDestroyed(this);
            if (GameManager.Instance.player.pickup == this)
            {
                GameManager.Instance.player.pickupHere = false;
                GameManager.Instance.player.pickup = null;
            }

            
            gameObject.SetActive(false);
        }
    }


    public virtual void GiveReward()
    {
    }

    public void TakeDemage(float demage)
    {
        if (_health > 0)
        {
            _health -= demage;
            UpdateHealth(demage);
        }
    }

}
