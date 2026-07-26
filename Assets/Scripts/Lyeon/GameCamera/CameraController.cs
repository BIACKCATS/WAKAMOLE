using UnityEngine;
using UnityEngine.InputSystem;

namespace Wakamole.Lyeon.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        // 미작동으로 인해...
        /*
        [Header("Components")]
        [Tooltip("카메라입니다.")]
        [SerializeField] private Camera cam;

        [Header("Information")]
        [Tooltip("카메라의 이동 속도입니다.")]
        [SerializeField] private float moveSpeed = 4.0f;
        [Tooltip("카메라가 위치 보정을 시작할 마우스와의 거리입니다.")]
        [SerializeField] private Vector2 mouseDistanceLimit;
        [Tooltip("카메라가 이동 가능한 현재 위치 기준 범위입니다.")]
        [SerializeField] private Vector2 moveLimit;
        [Tooltip("바닥으로 감지할 Layer입니다.")]
        [SerializeField] private LayerMask groundLayer;

        private Ray mouseRay;
        private Vector2 currentMouse = Vector2.zero;
        private Vector3 mousePosition = Vector3.zero, initPosition = Vector3.zero, targetPosition = Vector3.zero;

        private void Awake()
        {
            targetPosition = initPosition = transform.position;
            
            mouseDistanceLimit.x += initPosition.x;
            mouseDistanceLimit.y += initPosition.z;
        }

        private void Update()
        {
            if (Mouse.current == null) return;
            currentMouse = Mouse.current.position.ReadValue();
            mouseRay = Camera.main.ScreenPointToRay(currentMouse);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                mousePosition = hit.point;
                Debug.Log(mousePosition);
            }
        }

        private void LateUpdate()
        {
            // 위치 보정
            if (mousePosition.x > mouseDistanceLimit.x) targetPosition.x += mousePosition.x - mouseDistanceLimit.x;
            else if (mousePosition.x < -mouseDistanceLimit.x) targetPosition.x -= mousePosition.x + mouseDistanceLimit.x;
            if (mousePosition.z > mouseDistanceLimit.y) targetPosition.z += mousePosition.z - mouseDistanceLimit.y;
            else if (mousePosition.z < -mouseDistanceLimit.y) targetPosition.z -= mousePosition.z + mouseDistanceLimit.y;

            // 이동 범위 보정
            if (targetPosition.x < initPosition.x - moveLimit.x) targetPosition.x = initPosition.x - moveLimit.x;
            else if (targetPosition.x > initPosition.x + moveLimit.x) targetPosition.x = initPosition.x + moveLimit.x;
            if (targetPosition.z < initPosition.z - moveLimit.y) targetPosition.z = initPosition.z - moveLimit.y;
            else if (targetPosition.z > initPosition.z + moveLimit.y) targetPosition.z = initPosition.z + moveLimit.y;

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } */
    }
}