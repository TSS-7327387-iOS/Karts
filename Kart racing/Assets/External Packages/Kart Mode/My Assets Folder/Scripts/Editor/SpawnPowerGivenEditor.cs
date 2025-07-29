using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnPowerGiven))]
public class SpawnPowerGivenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty selectedPower = serializedObject.FindProperty("selectedPower");

        EditorGUILayout.PropertyField(selectedPower); // Show Enum Dropdown

        // Show fields based on selected enum
        switch ((SpawnPowerGiven.PowerAttribute)selectedPower.enumValueIndex)
        {
            case SpawnPowerGiven.PowerAttribute.Health:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("healthAmount"));
                break;

            case SpawnPowerGiven.PowerAttribute.Trap:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("trapDamage"));
                break;

            case SpawnPowerGiven.PowerAttribute.Bullet:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletDemage"));
                break;

            case SpawnPowerGiven.PowerAttribute.Missile:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("missileDamage"));
                break;

            //case SpawnPowerGiven.PowerAttribute.Nos:
            //    EditorGUILayout.PropertyField(serializedObject.FindProperty("nosBoostAmount"));
            //    break;
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerReceivedVfx"));

        serializedObject.ApplyModifiedProperties();
    }
}
