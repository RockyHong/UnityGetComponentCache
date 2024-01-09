using UnityEngine;
using System.Collections.Generic;

namespace UnityGetComponentCache
{
    public static class ExtensionMethods
    {
        private static Dictionary<Component, Dictionary<System.Type, Component>> componentCache =
            new Dictionary<Component, Dictionary<System.Type, Component>>();

        /// <summary>
        /// Caches and retrieves a single instance of a component of type T attached to the MonoBehaviour. 
        /// Assumes only one component of type T exists on the GameObject.
        /// </summary>
        /// <typeparam name="T">Component type to retrieve and cache.</typeparam>
        /// <param name="component">MonoBehaviour to get the component from.</param>
        /// <returns>Cached component of type T.</returns>
        public static T GetComponentCache<T>(this Component component) where T : Component
        {
            if (!componentCache.ContainsKey(component))
            {
                componentCache[component] = new Dictionary<System.Type, Component>();
            }

            var type = typeof(T);
            if (!componentCache[component].ContainsKey(type))
            {
                T retrievedComponent = component.GetComponent<T>();
                if (retrievedComponent == null)
                {
                    throw new System.InvalidOperationException(
                        $"Component of type {type} not found on {component.gameObject.name}");
                }

                componentCache[component][type] = retrievedComponent;
            }

            return componentCache[component][type] as T;
        }
    }
}