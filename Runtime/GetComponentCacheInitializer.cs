using System;
using System.Reflection;
using UnityEngine;

namespace UnityGetComponentCache
{
    public static class GetComponentCacheInitializer
    {
        /// <summary>
        /// Initialize all component caches in the given MonoBehaviour.
        /// Call this method in 'Awake()', 'Start()' or any other method that is called before the first usage of the cached component.
        /// </summary>
        public static void InitializeCaches(MonoBehaviour monoBehaviour)
        {
            var fields = monoBehaviour.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (var field in fields)
            {
                var hasComponentCache = Attribute.GetCustomAttribute(field, typeof(GetComponentCacheAttribute)) != null;

                if (hasComponentCache)
                {
                    var component = monoBehaviour.GetComponent(field.FieldType);
                    if (component != null)
                    {
                        field.SetValue(monoBehaviour, component);
                        Debug.Log($"[UnityComponentCache] Set {field.FieldType} to {field.Name} in {monoBehaviour.name}");
                    }
                    else
                    {
                        Debug.LogError($"[UnityComponentCache] Component of type {field.FieldType} not found for {field.Name} in {monoBehaviour.name}");
                    }
                }
            }
        }
    }
}