English | [繁體中文](https://github.com/RockyHong/UnityGetComponentCache/blob/main/README.zh-tw.md)

# Unity Get Component Cache Overview

**Unity Get Component Cache** is designed to solve and optimize the efficient access to Unity Components.

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

# Using Unity Get Component Cache:

## Installation

In your Unity project, open 'Window -> Package Manager', and add a Package from the Git URL:

```csharp
https://github.com/RockyHong/UnityGetComponentCache.git
```

## **Usage 1. One-Time Caching**:

Use the [GetComponentCache] attribute to mark fields for caching. Initialize these caches in a one-time method like 'Awake'. This ensures no initialization is missed even when new fields are added.

```csharp
using UnityGetComponentCache;

public class ExampleBehaviour : MonoBehaviour
{
    [GetComponentCache]
    Animator _animator;

    [GetComponentCache]
    Rigidbody _rigidbody;

    void Awake()
    {
        // Initialize cached components at once.
        GetComponentCacheInitializer.InitializeCaches(this);
    }
}
```

## **Usage 2. Editor Pre-Configuration**:

- Mark public fields or serializable private fields with [GetComponentCache].
- Use the 'Get Component Caches' button in the GameObject Inspector to pre-fill values, simplifying pre-run setup.

```csharp
using UnityGetComponentCache;

// Use [GetComponentCache] for public or [SerializeField] private fields.
public class ExampleBehaviour : MonoBehaviour
{
    [GetComponentCache]
    public Animator animator; // Public field.

    [GetComponentCache, SerializeField]
    private Rigidbody _rigidbody; // Private field.
}
```

![image](https://github.com/RockyHong/UnityGetComponentCache/assets/19500834/1333920a-124e-4c2e-b977-5cfbe36743af) ➜ ![image](https://github.com/RockyHong/UnityGetComponentCache/assets/19500834/35b8bf85-765b-4366-b823-d7eca00f09d0)

- **Initialize Get Component Caches' button status**:
  - **Green**: All [GetComponentCache] fields (public or with [SerializeField]) are cached and non-null.
  - **Yellow**: Some [GetComponentCache] fields are null.
  - **Red**: All [GetComponentCache] fields are null.
  - **\***: Indicates unsaved changes.

## **Usage 3. Runtime Lazy Caching**:

Utilize GetComponentCache<T> for efficient component retrieval. This extension method streamlines component access by caching them on first use. Ensure to prefix the method with this as it's an extension method.

```csharp
using UnityGetComponentCache;

public class ExampleBehaviour : MonoBehaviour
{
    Animator _animator => this.GetComponentCache<Animator>();
    Rigidbody _rigidbody => this.GetComponentCache<Rigidbody>();
}
```

or

```csharp
using UnityGetComponentCache;

public class ExampleBehaviour : Monobehaviour
{
    void Foo()
    {
        var animator = this.GetComponentCache<Animator>();
        var rigidbody = this.GetComponentCache<Rigidbody>();
        // Perform operations with cached values...
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
    [GetComponentCache]
    Animator _animator;

    [GetComponentCache]
    Rigidbody _rigidbody;
}
```
