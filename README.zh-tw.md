[English](https://github.com/RockyHong/UnityGetComponentCache/blob/main/README.md) | 繁體中文

# Unity Get Component Cache 簡介

**Unity Get Component Cache** 旨在解決與優化 Unity 有效存取 Component 的需求。

## 嘗試解決的問題：

- 在 Update 中高頻率的使用 'GetComponent' 可能會顯著降低效能。

```csharp
void Update()
{
    var rigidbody = GetComponent<Rigidbody>();
    // 使用 rigidbody 進行一些操作...
}
```

- 在 'Awake' 或 'Start' 方法中使用 'GetComponent' 來一次性取得所有 Component 欄位的值，可能會讓程式碼變得冗長與混亂。新增欄位的時候，也很容易忘記要補上初始化。

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

- 逐一實作延時快取可能會有冗長與重複的程式碼，增加不必要的複雜性。

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

# 使用 Unity Get Component Cache：

## **安裝**：

在 Unity 專案中，打開 'Window -> Package Manager'，從 Git URL 新增 Package：

```csharp
https://github.com/RockyHong/UnityGetComponentCache.git
```

## **使用方法 1. 一次性快取**：

使用 [GetComponentCache] 的 Attribute 標記需要快取的欄位，然後在一次性的方法中初始化這些快取，例如說 'Awake' 。這樣即使未來新增新的欄位，只要有標記 Attribute，就不怕遺漏初始化。

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
        // 一次性初始化快取元件。
        GetComponentCacheInitializer.InitializeCaches(this);
    }
}
```

## **使用方法 2. 編輯器中預先取值**：

- 在 GameObject 的檢視介面 (Inspector) 中點擊 'Get Component Caches' 按鈕，可以將所有標記 [GetComponentCache] 且可取用的欄位進行預先 GetComponent 並填入值，簡化運行前的設置。

```csharp
using UnityGetComponentCache;

// 使用[GetComponentCache]標記 public 公有或 [SerializeField] 可序列化的私有欄位。
public class ExampleBehaviour : MonoBehaviour
{
    [GetComponentCache]
    public Animator animator; // 公有欄位.

    [GetComponentCache, SerializeField]
    private Rigidbody _rigidbody; // 可序列化的私有欄位.
}
```

![image](https://github.com/RockyHong/UnityGetComponentCache/assets/19500834/1333920a-124e-4c2e-b977-5cfbe36743af) ➜ ![image](https://github.com/RockyHong/UnityGetComponentCache/assets/19500834/35b8bf85-765b-4366-b823-d7eca00f09d0)

- **'Get' 按鈕狀態**：
  - **一般色**：欄位非 null。
  - **紅色**：欄位為 null。

## **使用方法 3. Runtime 延時快取**：

使用 GetComponentCache<T> 來有效地取得元件。這個擴充方法透過首次使用時的快取，來簡化元件存取。使用時需在方法前加上 this，因為它是一個擴充方法。

```csharp
using UnityGetComponentCache;

public class ExampleBehaviour : MonoBehaviour
{
    Animator _animator => this.GetComponentCache<Animator>();
    Rigidbody _rigidbody => this.GetComponentCache<Rigidbody>();
}
```

或

```csharp
using UnityGetComponentCache;

public class ExampleBehaviour : MonoBehaviour
{
    void Foo()
    {
        var animator = this.GetComponentCache<Animator>();
        var rigidbody = this.GetComponentCache<Rigidbody>();
        // 對快取的值進行操作...
    }
}
```

## **額外想法**

#### 使用 [RequireComponent] 屬性：

可以考慮與 [RequireComponent] 結合使用，確保必要的 Component 始終包含於該 GameObject 中，增強快取機制的有效性。

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
