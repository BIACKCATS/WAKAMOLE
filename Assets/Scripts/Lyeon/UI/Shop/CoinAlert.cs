using UnityEngine;
using Wakamole.Core.Utils;

namespace Wakamole.Lyeon.UI.Shop
{
    public class CoinAlert : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private bool init = false;
        private Timer timer;

        private void Awake()
        {
            timer = new(0.25f);
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            init = false;
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        private void Update()
        {
            // 1. 먼저 알림창을 보여준다.
            if (!init)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1.0f, 10.0f * Time.deltaTime);
                if (canvasGroup.alpha > 0.99f)
                {
                    timer.Restart();
                    canvasGroup.alpha = 1.0f;
                    init = true;
                }
            }
            // 2. 일정 시간동안 유지한다.
            else if (timer.Active)
            {
                timer.Tick(Time.deltaTime);
            }
            // 3. 알림창을 숨긴다.
            else
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, 10.0f * Time.deltaTime);
                if (canvasGroup.alpha < 0.01f)
                {
                    canvasGroup.alpha = 0;
                    gameObject.SetActive(false);
                    init = false;
                }
            }
        }
    }
}