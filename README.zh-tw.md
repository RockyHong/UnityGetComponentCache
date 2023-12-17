# 簡介

**Unity Component Cache** 旨在解決與優化 Unity 有效存取 Component 的需求。

## 嘗試解決的問題：

- 在 Update 中高頻率使用 'GetComponent' 可能會顯著降低效能。

```csharp
void Update()
{
    var rigidbody = GetComponent<Rigidbody>();
    // 使用 rigidbody 進行一些操作...
}
```

- 在 'Awake' 或 'Start' 方法中使用 'GetComponent' 來一次性取得所有 Component 欄位的值，可能會讓程式碼變得冗長與混亂。新增欄位的時候，也容易會忘記要在補上初始化。

```csharp
Animator animator;
Rigidbody rigidbody;
Collider collider;
AudioSource audioSource;
// 其他更多欄位...

void Awake()
{
   animator = GetComponent<Animator>();
   rigidbody = GetComponent<Rigidbody>();
   collider = GetComponent<Collider>();
   audioSource = GetComponent<AudioSource>();
   // 更多 GetComponent...
}
```

- 使用屬性實作 Lazy Cache 可能會有冗長與重複的程式碼，增加不必要的複雜性。

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

# 使用 Unity Component Cache：

## **安裝和設置**：

在 Unity 專案中，打開 'Window -> Package Manager'，從 Git URL 新增 Package：

```csharp
https://github.com/RockyHong/UnityComponentCache.git
```

## **1. 一次性快取**：

使用 [ComponentCache] 的 Attribute 標記需要快取的欄位，然後在一次性的方法中初始化這些快取，例如說 'Awake' 。這樣即使未來新增新的欄位，只要有標記 Attribute，就不怕遺漏初始化。

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
        // 一次性初始化快取元件。
        ComponentCacheInitializer.InitializeCaches(this);
    }
}
```

## **2. 編輯器中儲存**：

- 在 GameObject 的檢視介面 (Inspector) 中點擊 'Initialize Unity Component Caches'按鈕，可以將所有標記[ComponentCache]且可取用的欄位進行預先 GetComponent 並填入值，簡化運行前的設置。

```csharp
using UnityComponentCache;

// 使用[ComponentCache]標記 public 公有或 [SerializeField] 可序列化的私有欄位。
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    public Animator animator; // 公有欄位.

    [ComponentCache, SerializeField]
    private Rigidbody _rigidbody; // 可序列化的私有欄位.
}
```

- **按鈕狀態：**:
  - **綠色**: 所有被標記 [ComponentCache] 欄位（公共或帶有[SerializeField]）都已快取且非 null。
  - **黃色**: 部分被標記 [ComponentCache] 欄位為 null。
  - **紅色**: 所有被標記 [ComponentCache] 欄位都為 null。
  - **\***: 表示有未儲存的更動。

## **3. Runtime 延遲快取**：

繼承 LazyComponentCacheBehaviour，便可以使用 'GetCachedComponent<T>' 來取得 Component。它將會自動在必要的時候進行 GetComponent 與快取。

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour
{
    Animator _animator => GetCachedComponent<Animator>();
    Rigidbody _rigidbody => GetCachedComponent<Rigidbody>();
}
```

或

```csharp
using UnityComponentCache;

public class ExampleBehaviour : LazyComponentCacheBehaviour
{
    void Foo()
    {
        var animator = GetCachedComponent<Animator>();
        var rigidbody = GetCachedComponent<Rigidbody>();
        // 隨意使用 Component 而不用擔心效能問題。
    }
}
```

# **額外想法**

## 使用 [RequireComponent] 屬性：

可以考慮與 [RequireComponent] 結合使用，確保必要的 Component 始終包含於該 GameObject 中，增強快取機制的有效性。

```csharp
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ExampleBehaviour : MonoBehaviour
{
    [ComponentCache]
    Animator _animator;

    [ComponentCache]
    Rigidbody _rigidbody;
    // 實現細節...
}
```