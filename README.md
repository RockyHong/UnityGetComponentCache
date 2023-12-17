# Unity Component Cache

## Introduction

Unity Component Cache is designed to address a specific challenge faced in game development: the need for efficient component access.

## Installation and Setup

**Installation**:

1. Unity Package Manager:
   In your Unity project, go to Window -> Package Manager.
   Add Package from Git URL: https://github.com/RockyHong/UnityComponentCache.git

### The Challenge Trying to Solve:

- **Frequent GetComponent Calls**: Frequently accessing components attached to a GameObject using `GetComponent` can lead to performance issues, especially in update loops.

```csharp
void Awake() {
    // Problem: Multiple GetComponent calls in Awake, causing performance issues.
    var animator = GetComponent<Animator>();
    var rigidbody = GetComponent<Rigidbody>();
    // More GetComponent calls...
}
```

- **Manual Caching Overhead**: Manually caching components often results in repetitive and cumbersome code with multiple `GetComponent` calls in the `Awake` or `Start` methods. Our system optimizes this process, reducing overhead and streamlining code.

```csharp
void Update() {
    // Problem: Calling GetComponent in Update, leading to severe performance hits.
    var rigidbody = GetComponent<Rigidbody>();
    // Using rigidbody for some operations...
}
```

### The Solution:

#### **Cache Manually**: Use the [ComponentCache] attribute to mark components for caching. Initialize these caches efficiently with 'ComponentCacheInitializer.InitializeCaches()' in essential methods like Awake.

```csharp
using UnityComponentCache;

public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    private Animator _animator;

    [ComponentCache]
    private Rigidbody _rigidbody;

    void Awake() {
        // Efficiently initialize cached components
        ComponentCacheInitializer.InitializeCaches(this);
    }

    void Update() {
        // Use animator and _rigidbody without GetComponent.
    }
}
```

#### **Cache In Editor**: Click 'Initialize Unity Component Caches' in the GameObject Inspector to cache components marked with [ComponentCache], simplifying pre-runtime setup.

```csharp
using UnityComponentCache;

public class ExampleBehaviour : MonoBehaviour {
    // Use [ComponentCache] for public or [SerializeField] private fields.

    [ComponentCache]
    public Animator animator; // Public field.

    [ComponentCache, SerializeField]
    private Rigidbody _rigidbody; // Private field.

    void Update() {
        // Use animator and _rigidbody without GetComponent.
    }
}
```

Button Status Guide:

- Red: All [ComponentCache] fields (public or with [SerializeField]) are cached and non-null.
- Yellow: Some [ComponentCache] fields are null.
- Green: All [ComponentCache] fields are null.
- '\*': Indicates unsaved changes.

#### **Lazy Cache Runtime**: Adopt LazyComponentCacheBehaviour for on-demand component caching. This approach optimizes performance by loading caching components only when needed.

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour {
    private Animator _animator => GetCachedComponent<Animator>();
    private Rigidbody _rigidbody => GetCachedComponent<Rigidbody>();

    void Update() {
        // Use animator and _rigidbody without GetComponent.
    }
}
```

or

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour {
    void Update() {
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
public class ExampleBehaviour : MonoBehaviour {
    [ComponentCache]
    private Animator _animator;

    [ComponentCache]
    private Rigidbody _rigidbody;
    // Implementation details...
}
```
