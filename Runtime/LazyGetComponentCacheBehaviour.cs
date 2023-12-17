using System.Collections.Generic;
using UnityEngine;

namespace UnityGetComponentCache
{
    public class LazyGetComponentCacheBehaviour : MonoBehaviour
    {
        private Dictionary<System.Type, Component> componentCaches = new Dictionary<System.Type, Component>();

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