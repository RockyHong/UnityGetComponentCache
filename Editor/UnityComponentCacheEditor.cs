using UnityEditor;
using UnityEngine;

namespace UnityComponentCache
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class UnityComponentCacheEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MonoBehaviour monoBehaviour = (MonoBehaviour)target;
            bool isDirty = EditorUtility.IsDirty(target);
            bool hasCacheableFields = false;
            bool anyFieldIsNull = false;
            bool allFieldsAreNull = true;

            var fields = monoBehaviour.GetType()
                .GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance |
                           System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.GetCustomAttributes(typeof(ComponentCacheAttribute), true).Length > 0 &&
                    (field.IsPublic || field.GetCustomAttributes(typeof(SerializeField), true).Length > 0))
                {
                    hasCacheableFields = true;

                    var fieldValue = field.GetValue(monoBehaviour);
                    if (fieldValue == null || (fieldValue is Component && fieldValue.Equals(null)))
                    {
                        anyFieldIsNull = true;
                    }
                    else
                    {
                        allFieldsAreNull = false;
                    }
                }
            }

            if (hasCacheableFields)
            {
                Color defaultColor = GUI.backgroundColor;

                if (!anyFieldIsNull)
                {
                    GUI.backgroundColor = Color.green; // All fields are not null and saved

                }
                else if (anyFieldIsNull && !allFieldsAreNull)
                {
                    GUI.backgroundColor = Color.yellow; // All fields are not null but not saved
                }
                else
                {
                    GUI.backgroundColor = Color.red;
                }

                string btnText = "Initialize Unity Component Caches";
                if (isDirty) btnText += "*";

                if (GUILayout.Button(btnText))
                {
                    ComponentCacheInitializer.InitializeCaches(monoBehaviour);
                    EditorUtility.SetDirty(target);
                }

                GUI.backgroundColor = defaultColor;
            }
        }
    }
}