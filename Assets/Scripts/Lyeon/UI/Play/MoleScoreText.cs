using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class MoleScoreText : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("점수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text text;
        [Tooltip("오브젝트가 이동할 위치입니다.")]
        [SerializeField] private Vector3 targetPosition = new(0, 2.0f, 0);

        private bool active = false;
        
        private Vector3 initPosition = Vector3.zero;

        private float targetAlpha = 1;

        public void ShowScore(int score)
        {
            if (active) Inactive();
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            text.SetText("+{0}점", score);
            targetAlpha = 1;
            active = true;
        }

        private void Awake()
        {
            initPosition = transform.localPosition;
            Inactive();
        }

        private void Update()
        {
            if (active)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 2.5f * Time.deltaTime);
                text.alpha = Mathf.Lerp(text.alpha, targetAlpha, 5.0f * Time.deltaTime);
                if (Mathf.Abs(text.alpha - targetAlpha) < 0.01f) targetAlpha = 0;
                if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f) Inactive();
            }
        }

        private void Inactive()
        {
            active = false;
            transform.localPosition = initPosition;
            text.alpha = 0;
            gameObject.SetActive(false);
        }
    }
}