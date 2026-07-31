using UnityEngine;

namespace Wakamole.Lyeon.Entity.Component
{
    public class MoleCharactor : MonoBehaviour
    {
        private bool active = false;
        private Vector3 initPosition = Vector3.zero;
        private Vector3 movePosition = new(0, 0.1f, 0);
        private Vector3 targetPosition = Vector3.zero;

        private bool moving = false;
        private float showTime = 0, moveSpeed = 0;
        private float showedTime = 0;

        /// <summary>
        /// 두더지가 활성화된 상태입니다.
        /// </summary>
        public bool Active
        {
            get => active;
            set
            {
                active = value;
                if (active) targetPosition = movePosition;
                else targetPosition = initPosition;
                showedTime = 0;
                moving = true;
            }
        }

        /// <summary>
        /// 두더지의 애니메이션 재생 상태입니다.
        /// </summary>
        public bool Moving => moving;

        /// <summary>
        /// 두더지를 때릴 수 있는 시간입니다.
        /// </summary>
        public float ShowTime { set => showTime = value; }

        /// <summary>
        /// 두더지가 움직이는 속도입니다.
        /// </summary>
        public float MoveSpeed { set => moveSpeed = value; }

        private void Awake()
        {
            initPosition = transform.localPosition;
        }

        private void Update()
        {
            if (!active) return;

            showedTime += Time.deltaTime;
            if (showedTime >= showTime) Active = false;
        }

        private void FixedUpdate()
        {
            if (!moving) return;
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                transform.localPosition = targetPosition;
                moving = false;
                if (targetPosition == initPosition)
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }      
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.fixedDeltaTime * moveSpeed);
        }
    }
}