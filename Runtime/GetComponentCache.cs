namespace UnityGetComponentCache
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    /// <summary>
    /// Use this attribute to mark a component field that should be cached.
    /// **Cached in Runtime**: 'GetComponentCacheInitializer.InitializeCaches()' must be called before start using the cached component. 
    /// **Cached in Editor**: The field must be public or marked as [SerializeField] in order to reveal the 'Get Component Caches' button in the inspector. 
    /// </summary>
    public class GetComponentCacheAttribute : System.Attribute
    {
    }
}