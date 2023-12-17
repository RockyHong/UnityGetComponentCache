using System.Collections.Generic;
using UnityEngine;

namespace UnityComponentCache
{
    public class LazyComponentCacheBehaviour : MonoBehaviour
    {
        private Dictionary<System.Type, Component> cachedComponents = new Dictionary<System.Type, Component>();

        public T GetCachedComponent<T>() where T : Component
        {
            System.Type type = typeof(T);
            if (!cachedComponents.TryGetValue(type, out Component component))
            {
                component = GetComponent<T>();
                if (component != null)
                {
                    cachedComponents[type] = component;
                }
            }
            return component as T;
        }
    }
}