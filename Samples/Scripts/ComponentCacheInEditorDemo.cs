using UnityComponentCache;
using UnityEngine;
using UnityEngine.Serialization;

public class ComponentCacheInEditorDemo : MonoBehaviour
{
    // Don't forget to publicize the fields you want to cache, or serialize them
    // and mark them with the [UnityComponentCache] attribute
    
    // Demo: Cache a transform and a text mesh
    [ComponentCache] public Transform publicCachedTransform;
    [SerializeField, ComponentCache] private TextMesh privateCachedTextMesh;

    private void Update()
    {
        float someValue = Mathf.Sin(Time.time);
        
        // Demo: Do something with the cached transform
        var position = publicCachedTransform.position;
        Vector3 somePosition = new Vector3(someValue, position.y, position.z);
        Quaternion someRotation = Quaternion.Euler(someValue * 45f, 0, 0);
        publicCachedTransform.SetPositionAndRotation(somePosition, someRotation);

        // Demo: Do something with the cached text mesh
        privateCachedTextMesh.text = $"{name} : {someValue:F2}";
        privateCachedTextMesh.color=Color.Lerp(Color.red, Color.blue, someValue);
    }
}
