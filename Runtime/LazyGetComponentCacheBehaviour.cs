using System.Collections.Generic;
using UnityEngine;

namespace UnityGetComponentCache
{
    public class LazyGetComponentCacheBehaviour : MonoBehaviour
    {
        private Dictionary<System.Type, Component> componentCaches = new Dictionary<System.Type, Component>();

        /// <summary>
        /// Use this method to get a component from the cache.
        /// If the component is not cached, it will be retrieved from the 'GetComponent' method and cached.
        /// If it is not found on the game object, null will be returned.
        public T GetComponentCache<T>() where T : Component
        {
            System.Type type = typeof(T);
            if (!componentCaches.TryGetValue(type, out Component component))
            {
                component = GetComponent<T>();
                if (component != null)
                {
                    componentCaches[type] = component;
                }
            }
            return component as T;
        }
    }
}