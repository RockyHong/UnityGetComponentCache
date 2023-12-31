using UnityEngine;

namespace UnityGetComponentCache.Samples
{
    public class RuntimeLazyCachingSample : MonoBehaviour
    {
        private Transform cachedTransform => this.GetComponentCache<Transform>();
        private TextMesh cachedTextMesh => this.GetComponentCache<TextMesh>();

        private void Update()
        {
            float someValue = Mathf.Sin(Time.time);

            // Demo: Do something with the cached transform
            var position = cachedTransform.position;
            Vector3 somePosition = new Vector3(someValue, position.y, position.z);
            Quaternion someRotation = Quaternion.Euler(someValue * 45f, 0, 0);
            cachedTransform.SetPositionAndRotation(somePosition, someRotation);

            // Demo: Do something with the cached text mesh
            cachedTextMesh.text = $"{name} : {someValue:F2}";
            cachedTextMesh.color = Color.Lerp(Color.red, Color.blue, someValue);
        }
    }
}