using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public string _name;
    public int health;
    public bool isEnemy;
    public bool isBot;
    public DetectPlayer detectPlayer;
    public Ability specialPower;
    // Ability ability;
    public Animator animator;
    public Powers power;
    public GameObject deathBonus;
    public bool isAlive = true;
    [HideInInspector]
    public PlayerProfile playerProfile;
    public bool isAnimatingPower;
    public Image healthBar;
    public TextMeshProUGUI UIname;
    public int _health;

    public GameObject kick;
    public BoxCollider fightCollider;
    float attackDelay=0.9f;


   
   

    public void InitializeAbilty()
    {
        power.character = this;
        playerProfile = new PlayerProfile();
        playerProfile.name = _name;
        UIname.text = _name;
        if (isEnemy)
            playerProfile.rank = UnityEngine.Random.Range(1, 9);
        else if (!isBot)
        {
            playerProfile.rank = PlayerPrefs.GetInt("PlayerRank", 1);
            playerProfile.xp = PlayerPrefs.GetInt("PlayerXP", 1);
            beforeSessionCoins = PlayerPrefs.GetInt("Coin");
        }
        _health = health;
        /*ability = ScriptableObject.CreateInstance<Ability>();
        ability.character = this;
        ability.animator = animator;
        ability.type = specialPower.type;
        ability.duration = specialPower.duration;
        ability.radius = specialPower.radius;
        ability.rechargeTime = specialPower.rechargeTime;
        ability.effect = specialPower.effect;
        specialPower = ability;*/
        attackDelay = 0.5f;
    }

    public void InitializeBotAI()
    {
        _health = health;
        
    }

    public GameObject crownPic;

    public void ActivateCrown(bool val)
    {
        crownPic.SetActive(val);
    }
    public void TakeDamage(int damage)
    {
        if (!isBot && power.inVulnerability) return;
        if (isAlive)
        {
            if (!isBot && !isEnemy) MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
            health -= damage;

            //MyChange
            //UpdateHealthbar();

            if (health <= 0)
            {
                healthBar.fillAmount = 0f;
                isAnimatingPower = false;
                isAlive = false;
                Die();
            }
            else if (health > 0)
            {
                UpdateHealthbar();
            }
        }
    }
    public void UpdateHealthbar()
    {
            
        if (healthBar != null)
            healthBar.fillAmount = 1 - ((float)(_health - health) / 100);

        //float val = 1 - ((float)(_health - health) / 100);
        //Debug.Log("_________________Health value________________ = " + val);
    }
    public void ResetHealth()
    {
        health = _health;

        if (healthBar != null) healthBar.fillAmount = 1 - ((float)(_health - health) / 100);
    }
    public virtual void Die()
    {

    }
    [HideInInspector]
    public int sessionCoins = 0;
    [HideInInspector]
    public int beforeSessionCoins = 0;
    public void AddCoin()
    {
        if (!isBot && !isEnemy)
        {
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 1);
            UIManager.Instance.UpdateCoins(PlayerPrefs.GetInt("Coin"));
            sessionCoins++;
        }
    }
    public void DetectTarget()
    {
        detectPlayer.InvertState();
    }

    public void KickColliderEnable()
    {
        kick.GetComponent<Collider>().enabled = true;

        Debug.Log("KickColliderEnable");
    }

    public void KickColliderDisable()
    {
        kick.GetComponent<Collider>().enabled = false;

        Debug.Log("KickColliderDisable");
    }
    IEnumerator attack;
    public void FightColliderEnable()
    {
        //fightCollider.enabled = true;

        attack = StartAttack();
        StartCoroutine(attack);
        Debug.Log("FightColliderEnable");
    }

    public void FightColliderDisable()
    {
        //fightCollider.enabled = false;
        //StopCoroutine(attack);
        StopAllCoroutines();
        //Debug.Log("FightColliderDisable");
    }
    
    IEnumerator StartAttack()
    {
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            detectPlayer.DetectAndAct();
        }

    }
}
[Serializable]
public class PlayerProfile
{
    public int rank;
    public string name;
    public int xp;
}