using System.Collections;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
        }
        
        public IEnumerator Shake(float duration, float magnitude, float speed)
        {
            Vector3 originalPosition = _camera.transform.localPosition;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Mathf.PerlinNoise(Time.time * speed, 0.0f) * 2.0f - 1.0f;
                float y = Mathf.PerlinNoise(0.0f, Time.time * speed) * 2.0f - 1.0f;

                x *= magnitude;
                y *= magnitude;

                _camera.transform.localPosition = new Vector3(
                    originalPosition.x + x,
                    originalPosition.y + y,
                    originalPosition.z
                );

                elapsed += Time.deltaTime;

                yield return null;
            }

            _camera.transform.localPosition = originalPosition;
        }
    }
}