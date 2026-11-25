using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class ScoreTextUI : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("오브젝트 이동을 위한 Transform입니다.")]
        [SerializeField] private Transform objectTransform;
        [Tooltip("점수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text text;
        private Vector3 targetPosition = new(0, 120.0f, 0);

        private bool active = false;
        
        private Vector3 initPosition = Vector3.zero;
        private Vector3 movePosition = Vector3.zero;

        private float targetAlpha = 1;
        private float speed = 10.0f;

        public float Speed { get => speed; set => speed = value; }

        public void SetInitialPosition(Vector3 position)
        {
            initPosition = position;
            objectTransform.position = position;
        }

        public void ShowScore(int score)
        {
            if (active) Inactive();
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            movePosition = initPosition + targetPosition;
            text.SetText("+{0}점", score);
            targetAlpha = 1;
            active = true;
        }

        private void Awake()
        {
            initPosition = objectTransform.position;
            Inactive();
        }

        private void Update()
        {
            if (active)
            {
                objectTransform.position = Vector3.Lerp(objectTransform.position, movePosition, Speed * Time.deltaTime);
                text.alpha = Mathf.Lerp(text.alpha, targetAlpha, Speed * 2 * Time.deltaTime);

                if (Mathf.Abs(text.alpha - targetAlpha) < 0.01f) targetAlpha = 0;
                if (Vector3.Distance(objectTransform.position, movePosition) < 0.01f)
                {
                    objectTransform.position = movePosition;
                    Inactive();
                }
            }
        }

        private void Inactive()
        {
            active = false;
            objectTransform.position = initPosition;
            text.alpha = 0;
            gameObject.SetActive(false);
        }
    }
}