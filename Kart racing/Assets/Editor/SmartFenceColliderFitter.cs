using UnityEditor;
using UnityEngine;

public class SmartFenceColliderFitter : EditorWindow
{
    enum ColliderType { BoxCollider, MeshCollider }
    ColliderType colliderType = ColliderType.BoxCollider;

    [MenuItem("Tools/Fit Fence Colliders")]
    static void Init()
    {
        SmartFenceColliderFitter window = (SmartFenceColliderFitter)GetWindow(typeof(SmartFenceColliderFitter));
        window.titleContent = new GUIContent("Fence Collider Fitter");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Fence Collider Fitter", EditorStyles.boldLabel);

        colliderType = (ColliderType)EditorGUILayout.EnumPopup("Collider Type:", colliderType);

        if (GUILayout.Button("Fit Collider to Selected Objects"))
        {
            FitColliders();
        }
    }

    void FitColliders()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            Renderer renderer = obj.GetComponent<Renderer>();

            if (meshFilter == null || renderer == null)
            {
                Debug.LogWarning($"{obj.name} skipped — no mesh or renderer.");
                continue;
            }

            // Remove existing colliders
            foreach (var col in obj.GetComponents<Collider>())
                DestroyImmediate(col);

            if (colliderType == ColliderType.MeshCollider)
            {
                MeshCollider meshCol = obj.AddComponent<MeshCollider>();
                meshCol.sharedMesh = meshFilter.sharedMesh;
                meshCol.convex = true; // Only if needed (e.g., for Rigidbody)
            }
            else // BoxCollider
            {
                BoxCollider box = obj.AddComponent<BoxCollider>();

                Bounds bounds = renderer.bounds;
                Vector3 localCenter = obj.transform.InverseTransformPoint(bounds.center);
                Vector3 localSize = bounds.size;

                // Adjust for lossy scale
                localSize = new Vector3(
                    bounds.size.x / obj.transform.lossyScale.x,
                    bounds.size.y / obj.transform.lossyScale.y,
                    bounds.size.z / obj.transform.lossyScale.z
                );

                box.center = localCenter;
                box.size = localSize;
            }
        }

        Debug.Log("✅ Collider fitting completed.");
    }
}
