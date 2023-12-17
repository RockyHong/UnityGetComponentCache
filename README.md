# Introduction

**Unity Component Cache** is designed to address a specific challenge faced in game development: the need for efficient component access.

## Identifying Common Challenges:

- Regular use of 'GetComponent' in Update loops can significantly degrade performance, a common issue in Unity development.

```csharp
void Update()
{
    var rigidbody = GetComponent<Rigidbody>();
    // Using rigidbody for some operations...
}
```

- The standard practice of multiple 'GetComponent' calls in 'Awake' or 'Start' methods leads to redundant and cluttered code.

```csharp
Animator animator;
Rigidbody rigidbody;
Collider collider;
AudioSource audioSource;
// More GetComponent...

void Awake()
{
   animator = GetComponent<Animator>();
   rigidbody = GetComponent<Rigidbody>();
   collider = GetComponent<Collider>();
   audioSource = GetComponent<AudioSource>();
    // More GetComponent calls...
}
```

- Implementing lazy-loading for components often results in lengthy and repetitive code, adding unnecessary complexity.

```csharp
Animator _animator;
Animator animator
{
    get
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        return _animator;
    }
}

RigidBody _rigidbody;
RigidBody rigidbody
{
    get
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<RigidBody>();
        }
        return _rigidbody;
    }
}
```

# Using Unity Component Cache:

## **1. Cache Manually**:

Use the [ComponentCache] attribute to mark components for caching. Initialize these caches efficiently with 'ComponentCacheInitializer.InitializeCaches()' in essential methods like 'Awake'.

```csharp
using UnityComponentCache;

public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    Animator _animator;

    [ComponentCache]
    Rigidbody _rigidbody;

    void Awake()
    {
        // Initialize cached components at once.
        ComponentCacheInitializer.InitializeCaches(this);
    }
}
```

## **2. Cache In Editor**:

- Click 'Initialize Unity Component Caches' button in the GameObject Inspector to cache components marked with [ComponentCache], simplifying pre-runtime setup.

```csharp
using UnityComponentCache;

// Use [ComponentCache] for public or [SerializeField] private fields.
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    public Animator animator; // Public field.

    [ComponentCache, SerializeField]
    private Rigidbody _rigidbody; // Private field.
}
```

- **'Initialize Unity Component Caches' button status**:
   - **Red**: All [ComponentCache] fields (public or with [SerializeField]) are cached and non-null.
   - **Yellow**: Some [ComponentCache] fields are null.
   - **Green**: All [ComponentCache] fields are null.
   - **\***: Indicates unsaved changes.

## **3. Lazy Cache Runtime**: 
Adopt LazyComponentCacheBehaviour for on-demand component caching. This approach optimizes performance by loading caching components only when needed.

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour
{
    Animator _animator => GetCachedComponent<Animator>();
    Rigidbody _rigidbody => GetCachedComponent<Rigidbody>();
}
```

or

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour
{
    void Foo()
    {
        var animator = GetCachedComponent<Animator>();
        var rigidbody = GetCachedComponent<Rigidbody>();
        // Use components at ease without worring performance.
    }
}
```

# Installation and Setup

In your Unity project, go to 'Window -> Package Manager'.
Add Package from Git URL:

   ```csharp
   https://github.com/RockyHong/UnityComponentCache.git
   ```

# **Additional Reminder**

### Using [RequireComponent] Attribute:

Consider pairing [RequireComponent] with Component Cache for optimal component management. It's a helpful strategy to ensure necessary components are always included, enhancing the effectiveness of the caching mechanism.

```csharp
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    Animator _animator;

    [ComponentCache]
    Rigidbody _rigidbody;
    // Implementation details...
}
```
