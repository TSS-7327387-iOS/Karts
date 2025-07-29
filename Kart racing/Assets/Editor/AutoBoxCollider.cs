using UnityEditor;
using UnityEngine;

public class AutoBoxColliderFitter : EditorWindow
{
    [MenuItem("Tools/Fit BoxCollider To Renderer")]
    public static void ShowWindow()
    {
        GetWindow<AutoBoxColliderFitter>("Fit BoxCollider");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Fit BoxCollider on Selected Objects"))
        {
            FitBoxCollidersOnSelection();
        }
    }

    void FitBoxCollidersOnSelection()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogWarning($"No Renderer found on {obj.name}");
                continue;
            }

            BoxCollider box = obj.GetComponent<BoxCollider>();
            if (box == null)
                box = obj.AddComponent<BoxCollider>();

            Bounds bounds = renderer.bounds;

            Vector3 localCenter = obj.transform.InverseTransformPoint(bounds.center);
            Vector3 localSize = new Vector3(
                bounds.size.x / obj.transform.lossyScale.x,
                bounds.size.y / obj.transform.lossyScale.y,
                bounds.size.z / obj.transform.lossyScale.z
            );

            box.center = localCenter;
            box.size = localSize;
        }

        Debug.Log("Finished fitting BoxColliders on selected objects.");
    }
}
