English | [繁體中文](https://github.com/RockyHong/UnityComponentCache/blob/main/README.zh-tw.md)

# Unity Component Cache Overview

**Unity Component Cache** is designed to solve and optimize the efficient access to Unity Components.

## Problems Addressed:

- Frequent use of 'GetComponent' can significantly reduce performance.

```csharp
void Update()
{
    var rigidbody = GetComponent<Rigidbody>();
    // Performing some operations with rigidbody...
}
```

- Using 'GetComponent' in 'Awake' or 'Start' methods for one-time initialization of all component fields might lead to lengthy and confusing code. It's also easy to forget to initialize new fields.

```csharp
Animator animator;
Rigidbody rigidbody;
Collider collider;
AudioSource audioSource;
// Many other fields...

void Awake()
{
   animator = GetComponent<Animator>();
   rigidbody = GetComponent<Rigidbody>();
   collider = GetComponent<Collider>();
   audioSource = GetComponent<AudioSource>();
    // More GetComponent calls...
}
```

- Implementing lazy-loading with properties might lead to verbose and redundant code.

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

## Installation

In your Unity project, open 'Window -> Package Manager', and add a Package from the Git URL:

```csharp
https://github.com/RockyHong/UnityComponentCache.git
```

## **Usage Method 1. One-Time Caching:**:

Use the [ComponentCache] attribute to mark fields for caching. Initialize these caches in a one-time method like 'Awake'. This ensures no initialization is missed even when new fields are added.

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

## **Usage Method 2. Editor Pre-Configuration**:

- Mark public fields or serializable private fields with [ComponentCache].
- Use the 'Initialize Unity Component Caches' button in the GameObject Inspector to pre-fill values, simplifying pre-run setup.

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

- **Initialize Unity Component Caches' button status**:
  - **Red**: All [ComponentCache] fields (public or with [SerializeField]) are cached and non-null.
  - **Yellow**: Some [ComponentCache] fields are null.
  - **Green**: All [ComponentCache] fields are null.
  - **\***: Indicates unsaved changes.

## **Usage Method 3. Runtime Lazy Caching**:

Inherit from 'LazyComponentCacheBehaviour' to use 'GetCachedComponent<T>' for component retrieval. It automatically performs 'GetComponent' and caching when necessary.

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

## **Additional Note**

### Using [RequireComponent] Attribute:

Consider combining with [RequireComponent] to ensure essential components are always included in the GameObject, enhancing the caching mechanism's effectiveness.

```csharp
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    Animator _animator;

    [ComponentCache]
    Rigidbody _rigidbody;
}
```
