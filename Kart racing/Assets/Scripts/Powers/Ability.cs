using System;
using UnityEngine;

[CreateAssetMenu(fileName ="Ability",menuName = "ScriptableObjects/Ability")]
public class Ability : ScriptableObject
{
    public float radius,rechargeTime;
    public AbilityType type;
    public Animator animator;
    public GameObject effect;
    public AudioClip powerAudio;
    public float powerAudioLength;
    //public Character character;
    public float duration;

    public virtual void StartAttck(Transform target,Character ch) {}

    public virtual void ShowEffect() { }
    
    

    
}
[Serializable]
public enum AbilityType
{
    Attack,
    Defence,
    Sneak,
    Spell
}
