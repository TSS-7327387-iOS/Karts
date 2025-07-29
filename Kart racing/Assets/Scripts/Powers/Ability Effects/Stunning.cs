
using UnityEngine;

public class Stunning : Powers
{
    public StunnObj stunnObj;
    private void Start()
    {
        base.Initialize();
    }
    public override void InitiateStunn(float delay)  //StunEffect
    {
        Instantiate(stunnObj,transform.position,transform.rotation).InitializeData(character,delay);
    }
  
}
