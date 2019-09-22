#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
    [System.Serializable]
    public class RequirementVO : CommodityVO
	{
        public RequirementOperator Operator;
    }
/*
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequirementVO))]
    public class RequirementDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Type"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Value"), true);

        }
    }
#endif
*/
}
