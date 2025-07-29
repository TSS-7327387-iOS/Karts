using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisiblity : Powers
{
    public Material orgMat, powerMat;
    public SkinnedMeshRenderer body;
    public MeshRenderer cloak;
    public MeshRenderer[] bodyParts;

    private void Start()
    {
        base.Initialize();
    }
    public override void InVisible(float duration)
    {
        //SetLayerRecursively(this.gameObject,8);
        if (!character.isEnemy && !character.isBot)
        {
            body.material=powerMat;
            cloak.material=powerMat;
            foreach (var outline in bodyParts)
                outline.material = powerMat;
        }
        if (character.isEnemy)
        {
            GameManager.Instance.enemyManager.chganeEnemiesStateToRunExcept(move.enemy);
            SetLayerRecursively(this.gameObject, 8);
        }
        else if(GameManager.Instance.playerHasBalls)
            GameManager.Instance.enemyManager.chganeEnemiesStateToRun();
        Invoke(nameof(ResetVisibility), duration);
        PlayPowerSound();
    }

    
    public override void ResetVisibility()
    {
        if (!character.isEnemy && !character.isBot)
        {
            body.material = orgMat;
            cloak.material = orgMat;
            foreach (var outline in bodyParts)
                outline.material = orgMat;
        }

        if (character.isEnemy)
        {
            int Layer = LayerMask.NameToLayer("Enemy");
            SetLayerRecursively(this.gameObject, Layer);
        }

        character.isAnimatingPower = false;
        GameManager.Instance.enemyManager.chganeEnemiesState(GameManager.Instance.ballHolder);
    }
    private void OnDisable()
    {
        if (!character.isEnemy && !character.isBot)
        {
            body.material = orgMat;
            cloak.material = orgMat;
            foreach (var outline in bodyParts)
                outline.material = orgMat;
        }

        if (character.isEnemy)
        {
            int Layer = LayerMask.NameToLayer("Enemy");
            SetLayerRecursively(this.gameObject, Layer);
        }
        character.isAnimatingPower = false;
        ShutAudioOff();
    }
    void SetLayerRecursively( GameObject obj,int newLayer )
    {
        obj.layer = newLayer;

        foreach (Transform t in obj.transform)
        {
            SetLayerRecursively(t.gameObject, newLayer);
        }
    }
}
