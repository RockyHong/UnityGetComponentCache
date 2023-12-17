# Unity Component Cache

## Introduction

Unity Component Cache is designed to address a specific challenge faced in game development: the need for efficient component access.

## Installation and Setup

In your Unity project, go to Window -> Package Manager.
Add Package from Git URL:

```csharp
https://github.com/RockyHong/UnityComponentCache.git
```

### Identifying Common Challenges:

1. **Frequent GetComponent Usage**:
   Regular use of 'GetComponent' in Update loops can significantly degrade performance, a common issue in Unity development.

```csharp
void Update()
{
    var rigidbody = GetComponent<Rigidbody>();
    // Using rigidbody for some operations...
}
```

2. **Redundant Manual Caching**:
   The standard practice of multiple 'GetComponent' calls in 'Awake' or 'Start' methods leads to redundant and cluttered code.

```csharp
Animator animator;
Rigidbody rigidbody;
// More GetComponent...

void Awake()
{
    animator = GetComponent<Animator>();
    rigidbody = GetComponent<Rigidbody>();
    // More GetComponent calls...
}
```

3. **Verbose Lazy Initialization**:
   Implementing lazy-loading for components often results in lengthy and repetitive code, adding unnecessary complexity.

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

### Using Unity Component Cache:

#### **Cache Manually**:

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

    void Update()
    {
        // Use animator and _rigidbody without GetComponent.
    }
}
```

#### **Cache In Editor**:

1. Click 'Initialize Unity Component Caches' in the GameObject Inspector to cache components marked with [ComponentCache], simplifying pre-runtime setup.

```csharp
using UnityComponentCache;

// Use [ComponentCache] for public or [SerializeField] private fields.
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    public Animator animator; // Public field.

    [ComponentCache, SerializeField]
    private Rigidbody _rigidbody; // Private field.

    void Update()
    {
        // Use animator and _rigidbody without GetComponent.
    }
}
```

2. Button Status Guide:

- Red: All [ComponentCache] fields (public or with [SerializeField]) are cached and non-null.
- Yellow: Some [ComponentCache] fields are null.
- Green: All [ComponentCache] fields are null.
- '\*': Indicates unsaved changes.

#### **Lazy Cache Runtime**: Adopt LazyComponentCacheBehaviour for on-demand component caching. This approach optimizes performance by loading caching components only when needed.

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour
{
    private Animator _animator => GetCachedComponent<Animator>();
    private Rigidbody _rigidbody => GetCachedComponent<Rigidbody>();

    void Update()
    {
        // Use animator and _rigidbody without GetComponent.
    }
}
```

or

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour
{
    void Update()
    {
        var animator = GetCachedComponent<Animator>();
        var rigidbody = GetCachedComponent<Rigidbody>();
        // Use animator and _rigidbody without GetComponent.
    }
}
```

### **Additional Reminder**

#### Using [RequireComponent] Attribute:

Consider pairing [RequireComponent] with Component Cache for optimal component management. It's a helpful strategy to ensure necessary components are always included, enhancing the effectiveness of the caching mechanism.

```csharp
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    private Animator _animator;

    [ComponentCache]
    private Rigidbody _rigidbody;
    // Implementation details...
}
```
