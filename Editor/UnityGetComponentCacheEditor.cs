using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace UnityGetComponentCache
{
    [CustomPropertyDrawer(typeof(GetComponentCacheAttribute))]
    public class GetComponentCacheDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Start checking for changes
            EditorGUI.BeginChangeCheck();

            // Calculate rects for the property field and the button
            Rect propertyRect = new Rect(position.x, position.y, position.width - 60, position.height);
            Rect buttonRect = new Rect(position.x + position.width - 50, position.y, 50, position.height);

            // Draw the property field
            EditorGUI.PropertyField(propertyRect, property, label);

            var component = property.serializedObject.targetObject as Component;
            var fieldType = fieldInfo.FieldType;
            bool isFieldNull = CheckIfFieldIsNull(component, fieldInfo);

            // Determine button color
            GUI.backgroundColor = isFieldNull ? Color.yellow : Color.green;

            // Draw the button and handle its click
            if (GUI.Button(buttonRect, "Get"))
            {
                if (isFieldNull && component != null)
                {
                    var newComponent = component.GetComponent(fieldType);
                    if (newComponent != null)
                    {
                        fieldInfo.SetValue(component, newComponent);
                        property.serializedObject.Update();
                        EditorUtility.SetDirty(component); // Mark object as dirty
                    }
                }
            }

            // Reset the background color
            GUI.backgroundColor = Color.white;

            EditorGUI.EndProperty();
        }
        
        private bool CheckIfFieldIsNull(Component component, FieldInfo field)
        {
            if (component == null) return true;

            var value = field.GetValue(component);
            if (value == null) return true;

            // Special handling for Unity objects
            if (value is UnityEngine.Object unityObject)
            {
                return unityObject == null;
            }

            return false;
        }
    }
}