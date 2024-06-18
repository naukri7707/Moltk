using Naukri.Moltk.MeshDeformation;
using UnityEditor;

namespace Naukri.Moltk.Editor.MeshDeformation
{
    [CustomEditor(typeof(DeformableObject), true)]
    public class DeformableObjectEditor : UnityEditor.Editor
    {
        private DeformableObject deformableObject;

        public override void OnInspectorGUI()
        {
            var monoScriptProperty = serializedObject.FindProperty("m_Script");
            var parametersProperty = serializedObject.FindProperty(nameof(DeformableObject.parameters));
            var vertexModifiersProperty = serializedObject.FindProperty(nameof(DeformableObject.vertexModifiers));
            var changeShaderPassDynamiclyProperty = serializedObject.FindProperty(nameof(DeformableObject.changeShaderPassDynamicly));
            var materialsProperty = serializedObject.FindProperty(nameof(DeformableObject.materials));
            var shaderPassLayersProperty = serializedObject.FindProperty(nameof(DeformableObject.shaderPassLayers));

            using (var scope = new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(monoScriptProperty);
            }
            EditorGUILayout.PropertyField(parametersProperty);
            EditorGUILayout.PropertyField(vertexModifiersProperty);
            EditorGUILayout.PropertyField(changeShaderPassDynamiclyProperty);
            if (changeShaderPassDynamiclyProperty.boolValue)
            {
                EditorGUILayout.PropertyField(materialsProperty);
                EditorGUILayout.PropertyField(shaderPassLayersProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            deformableObject = serializedObject.targetObject as DeformableObject;
        }
    }
}
