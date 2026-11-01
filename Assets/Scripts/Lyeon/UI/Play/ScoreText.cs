using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class ScoreText : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("오브젝트 이동을 위한 Transform입니다.")]
        [SerializeField] private Transform objectTransform;
        [Tooltip("점수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text text;
        [Tooltip("오브젝트가 이동할 위치입니다.")]
        [SerializeField] private Vector3 targetPosition = new(0, 1.72f, 1.0f);
        /// Mole: (0, 1.72, 1.0)
        /// UI: ???

        private bool active = false;
        
        private Vector3 initPosition = Vector3.zero;
        private Vector3 movePosition = Vector3.zero;

        private float targetAlpha = 1;

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
                objectTransform.position = Vector3.Lerp(objectTransform.position, movePosition, 2.5f * Time.deltaTime);
                text.alpha = Mathf.Lerp(text.alpha, targetAlpha, 5.0f * Time.deltaTime);

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