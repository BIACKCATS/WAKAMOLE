using UnityEngine;
using UnityEngine.InputSystem;

namespace Wakamole.Lyeon.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Information")]
        [Tooltip("카메라의 이동 속도입니다.")]
        [SerializeField] private float moveSpeed = 4.0f;
        [Tooltip("카메라가 움직이지 않는 영역의 최대값입니다.")]
        [SerializeField] private Vector2 mouseMaxDistance;
        [Tooltip("카메라가 움직이지 않는 영역의 최솟값입니다.")]
        [SerializeField] private Vector2 mouseMinDistance;
        [Tooltip("카메라가 이동 가능한 최대 좌표입니다.")]
        [SerializeField] private Vector2 moveMaxLimit;
        [Tooltip("카메라가 이동 가능한 최소 좌표입니다.")]
        [SerializeField] private Vector2 moveMinLimit;
        [Tooltip("바닥으로 감지할 Layer입니다.")]
        [SerializeField] private LayerMask groundLayer;

        private bool expandMove = false;

        public bool ExpandMove { get => expandMove; set => expandMove = value; }

        private Ray mouseRay;
        private Vector2 currentMouse = Vector2.zero; // 화면 상의 마우스 위치
        private Vector3 mousePosition = Vector3.zero, initPosition = Vector3.zero, targetPosition = Vector3.zero;

        private void Awake()
        {
            targetPosition = initPosition = transform.position;
        }

        private void Update()
        {
            if (Mouse.current == null || !expandMove) return;
            currentMouse = Mouse.current.position.ReadValue();
            mouseRay = Camera.main.ScreenPointToRay(currentMouse);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, groundLayer))
                mousePosition = hit.point;
        }

        private void LateUpdate()
        {
            if (!expandMove) return;
            if (mousePosition.x > mouseMaxDistance.x) targetPosition.x = moveMaxLimit.x;
            else if (mousePosition.x < mouseMinDistance.x) targetPosition.x = moveMinLimit.x;
            else targetPosition.x = initPosition.x;

            if (mousePosition.z > mouseMaxDistance.y) targetPosition.z = moveMaxLimit.y;
            else if (mousePosition.z < mouseMinDistance.y) targetPosition.z = moveMinLimit.y;
            else targetPosition.z = initPosition.z;

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}