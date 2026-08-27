using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Core.Camera;

namespace Wakamole.Lyeon.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Current { get; private set; }

        [Header("Information")]
        [SerializeField] private Camera cam;
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

        private bool expandMove = true;

        private Ray mouseRay;
        private Vector2 currentMouse = Vector2.zero; // 화면 상의 마우스 위치
        private Vector3 mousePosition = Vector3.zero, initPosition = Vector3.zero, targetPosition = Vector3.zero, randomPosition = Vector3.zero;
        private CameraShake shake;

        public float MoveSpeed => moveSpeed;
        public bool ExpandMove { get => expandMove; set => expandMove = value; }
        public Vector3 InitPosition => initPosition;
        public Vector3 TargetPosition
        {
            get => targetPosition;
            set
            {
                if (expandMove) targetPosition = value;
            }
        }

        private void Awake()
        {
            targetPosition = initPosition = transform.position;

            if (Current != null) Current = null;
            Current = this;

            shake = new(cam)
            {
                Duartion = 0.2f
            };

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
            if (expandMove)
            {
                if (mousePosition.x > mouseMaxDistance.x) targetPosition.x = moveMaxLimit.x;
                else if (mousePosition.x < mouseMinDistance.x) targetPosition.x = moveMinLimit.x;
                else targetPosition.x = initPosition.x;

                if (mousePosition.z > mouseMaxDistance.y) targetPosition.z = moveMaxLimit.y;
                else if (mousePosition.z < mouseMinDistance.y) targetPosition.z = moveMinLimit.y;
                else targetPosition.z = initPosition.z;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition + randomPosition, moveSpeed * Time.deltaTime);
        }

        private void OnDisable()
        {
            Current = null;
        }

        public void Shake(float strength = 0.5f)
        {
            shake.Strength = strength;
            StartCoroutine(shake.Shake());
        }
    }
}