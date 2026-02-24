
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    public Transform[] spwanPoints;
    public Transform[] enemyWaypoints;
    public EnemyAI[] enemies;
    int maxEnemies;
    public List<EnemyAI> enemiesAlive;
    List<EnemyAI> enemiesDied;
    List<BotAI> botsAlive;
    public List<BotAI> botsInGame;
    [SerializeField]public EnemyAI enemyWithBall;
    public string[] dummyNames;
    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = new List<EnemyAI>();
        enemiesDied = new List<EnemyAI>();
        botsAlive = new List<BotAI>();
    }
    public void GameStarted()
    {
        maxEnemies = Mathf.Clamp(PlayerPrefs.GetInt("PlayerRank") + 3, 4, 10);
        UIManager.Instance.playerCount.text = maxEnemies.ToString() + "/" + maxEnemies.ToString();
        SpwanEnemies();
        InvokeRepeating(nameof(SpwanBots), 5, 10);
    }
    void SpwanEnemies()
    {
        List<string> pickedNames = PickUniqueNames(dummyNames, maxEnemies);
        for (int i = 0; i < maxEnemies; i++)
        {
            var val = Mathf.Clamp(i, 0, spwanPoints.Length - 1);
            var enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spwanPoints[val].position, spwanPoints[val].rotation);
            enemy._name = pickedNames[i];
            enemiesAlive.Add(enemy);
            GameManager.Instance.scores.Add(enemy,0);
        }
    }
    void SpwanBots()
    {
        if (botsAlive==null || botsAlive.Count < 1)
            return;
        int index = Random.Range(0, botsAlive.Count);
        botsAlive[index].ResetBot();
        botsAlive[index].gameObject.SetActive(true);
        botsAlive.RemoveAt(index);
    }

    public void chganeEnemiesState(Transform target)
    {
        foreach (var enemy in enemiesAlive)
        {
            enemy.ChangeStateToChase(target);
        }
        foreach (var enemy in enemiesDied)
        {
            enemy.ChangeStateToChase(target);
        }
    }
    public void chganeEnemiesStateToFindBall()
    {
        foreach (var enemy in enemiesAlive)
        {
            enemy.findBall(Vector3.forward);
        }
    }
    public void chganeEnemiesStateToRunExcept(EnemyAI en)
    {
        foreach (var enemy in enemiesAlive)
        {
            if(en!=enemy)
                enemy.move.ChangeStateToWander();
        }
    }
    public void chganeEnemiesStateToRun()
    {
        foreach (var enemy in enemiesAlive)
        {
            enemy.move.ChangeStateToWander();
        }
    }

    public void BotDied(BotAI bot)
    {
        foreach (var enemy in enemiesAlive)
        {
            if (enemy.move.botChasing == bot)
            {
                enemy.move.botHere = false;
                enemy.move.botChasing = null;
            }
        }
        Debug.LogWarning(bot._name);
        bot.transform.position = spwanPoints[Random.Range(0, spwanPoints.Length)].position;
        botsAlive.Add(bot);
    }
    public void PickupDestroyed(Pickable pick)
    {
        foreach (var enemy in enemiesAlive)
        {
            if (enemy.move.pickTarget == pick)
            {
                enemy.move.pickUpHere = false;
                enemy.move.pickTarget = null;
            }
        }
    }
    public void DeActivateEnemy(EnemyAI enemy)
    {
        enemiesAlive.Remove(enemy);
        enemy.transform.position = spwanPoints[Random.Range(0, spwanPoints.Length)].position;
        enemiesDied.Add(enemy);
        foreach(BotAI bot in botsInGame)
        {
            if (bot.isAlive)
                bot.CheckForEnemyTargetDeath(enemy.transform);
        }
        Invoke(nameof(RespwanEnemy),5);
    }
    void RespwanEnemy()
    {
        if(enemiesDied.Count<1)
            return;
        enemiesDied[0].gameObject.SetActive(true);
        enemiesDied[0].ResetHealth();
        enemiesAlive.Add(enemiesDied[0]);
        enemiesDied.RemoveAt(0);
    }
    public void GameStopped()
    {
        CancelInvoke();
        foreach(var enemy in enemiesAlive)
        {
            enemy.move.fallen = true;
            enemy.gameObject.SetActive(false);
        }
    }
    /*public EnemyAI WhichEnemyHasBall()
    {
        foreach (var enemy in enemiesAlive)
        {
            if(enemy.move.ballPicked)
                return enemy;
        }
        return null;
    }*/
    List<string> PickUniqueNames(string[] names, int count)
    {
        HashSet<string> uniqueNames = new HashSet<string>();
        System.Random random = new System.Random();

        // Loop until we have the desired number of unique names or run out of options
        while (uniqueNames.Count < count && uniqueNames.Count < names.Length)
        {
            int index = random.Next(names.Length);
            uniqueNames.Add(names[index]); // Adds only if not already in the set
        }

        return new List<string>(uniqueNames); // Convert HashSet to List
    }

    public void ResetEnemyPositionRotation(Transform _enemy)
    {
        Transform trans = spwanPoints[Random.Range(0, spwanPoints.Length)];

        _enemy.SetPositionAndRotation(trans.position, trans.rotation);
    }
}
