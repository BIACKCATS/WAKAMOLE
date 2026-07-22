using System.Collections;
using UnityEngine;

namespace Wakamole.Core.Camera
{
    public class CameraShake
    {
        private UnityEngine.Camera camera;
        private float duration = 1.0f;
        private float strength = 1.0f;

        public CameraShake(UnityEngine.Camera camera) { this.camera = camera; }
        public void SetDuration(float duration) { this.duration = duration; }
        public void SetStrength(float strength) { this.strength = strength; }

        public IEnumerator Shake()
        {
            float currentTime = 0;
            Vector3 initPosition = camera.transform.position;
            Vector3 randomPosition = Vector3.zero;
            while (currentTime < duration)
            {
                randomPosition.x = Random.Range(-strength, strength);
                randomPosition.y = Random.Range(-strength, strength);
                camera.transform.position = initPosition + randomPosition;

                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            camera.transform.position = initPosition;
        }
    }
}