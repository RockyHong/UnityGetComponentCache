using UnityEngine;

namespace UnityGetComponentCache
{
    /// <summary>
    /// Use this attribute to mark a component field that should be cached.
    /// **Cached in Runtime**: 'GetComponentCacheInitializer.InitializeCaches()' must be called before start using the cached component. 
    /// **Cached in Editor**: The field must be public or marked as [SerializeField] in order to reveal the 'Get Component Caches' button in the inspector. 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class GetComponentCacheAttribute : PropertyAttribute
    {
    }
}