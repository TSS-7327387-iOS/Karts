using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RafiActivator : MonoBehaviour
{
    [Tooltip("Yeh woh GameObject hai jisko sirf Main Menu se aane par ON karna hai.")]
    public GameObject rafiRoot;

    [Tooltip("Activate/Deactivate kis Unity event pe karein?")]
    public bool runInAwake = true;   // agar Awake me check karna hai
    public bool runInStart = false;  // ya Start me

    private void Awake()
    {
        if (runInAwake)
            Apply();
    }

    private void Start()
    {
        if (runInStart)
            Apply();
    }

    private void Apply()
    {
        if (rafiRoot == null)
        {
            Debug.LogError("[RafiActivator] rafiRoot not assigned.");
            return;
        }

        bool cameFromMenu = FlowOrigin.ReadAndConsumeCameFromMainMenu();
        rafiRoot.SetActive(cameFromMenu);

        Debug.Log($"[RafiActivator] Rafi active = {cameFromMenu}");
    }
}
