using System.Collections;
using UnityEngine;

namespace Wakamole.Core.Camera
{
    /// <summary>
    /// 카메라에 지정된 값만큼 흔들림 효과를 부여하는 클래스입니다.
    /// </summary>
    public class CameraShake
    {
        private UnityEngine.Camera camera;
        private float duration = 1.0f;
        private float strength = 1.0f;

        /// <summary>
        /// 효과가 지속되는 시간입니다.
        /// </summary>
        public float Duartion { set => duration = value; }
        /// <summary>
        /// 효과의 강도입니다. 커질 수록 흔들림이 심해집니다.
        /// </summary>
        public float Strength { set => strength = value; }

        /// <summary>
        /// CameraShake 클래스를 초기화합니다.
        /// </summary>
        /// <param name="camera">해당 효과를 적용할 Camera입니다.</param>
        public CameraShake(UnityEngine.Camera camera) { this.camera = camera; }

        /// <summary>
        /// MonoBehavior.StartCoroutine() 함수를 통해 해당 함수를 실행해 흔들림 효과가 지정된 값만큼 실행됩니다.
        /// </summary>
        /// <returns>MonoBehavior.StartCoroutine()에서 실행할 함수를 반환합니다.</returns>
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