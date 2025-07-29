using UnityEditor;
using UnityEngine;

public class ReplaceChildrenWithPrefab : EditorWindow
{
    GameObject prefabToReplaceWith;
    string componentName;

    [MenuItem("Tools/Replace Children With Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceChildrenWithPrefab>("Replace Children");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Children With Prefab", EditorStyles.boldLabel);

        prefabToReplaceWith = (GameObject)EditorGUILayout.ObjectField("Replacement Prefab", prefabToReplaceWith, typeof(GameObject), false);
        componentName = EditorGUILayout.TextField("Component Name", componentName);

        if (GUILayout.Button("Replace Now"))
        {
            ReplaceChildren();
        }
    }

    void ReplaceChildren()
    {
        if (prefabToReplaceWith == null || string.IsNullOrEmpty(componentName))
        {
            Debug.LogWarning("Please assign a prefab and enter a valid component name.");
            return;
        }

        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        Transform[] children = selected.GetComponentsInChildren<Transform>(true);

        int replaceCount = 0;
        foreach (Transform child in children)
        {
            if (child == selected.transform) continue;

            var component = child.GetComponent(componentName);
            if (component == null) continue;

            // Instantiate the prefab
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabToReplaceWith, selected.transform);

            newObj.transform.localPosition = child.localPosition;
            newObj.transform.localRotation = child.localRotation;
            newObj.transform.localScale = child.localScale;
            newObj.name = child.name;

            Undo.RegisterCreatedObjectUndo(newObj, "Replace With Prefab");
            Undo.DestroyObjectImmediate(child.gameObject);
            replaceCount++;
        }

        Debug.Log($"Replaced {replaceCount} children with prefab.");
    }
}
