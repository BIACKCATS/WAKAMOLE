using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class ChargeBar : ProgressBar
    {
        [SerializeField] private CanvasGroup canvasGroup;

        protected override void Update()
        {
            base.Update();
            canvasGroup.alpha = image.fillAmount;
        }
    }
}