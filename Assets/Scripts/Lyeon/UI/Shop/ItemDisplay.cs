using UnityEngine;
using UnityEngine.EventSystems;

namespace Wakamole.Lyeon.UI.Shop
{
    public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool activeHover = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemTooltip.Current.gameObject.SetActive(true);
            activeHover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemTooltip.Current.gameObject.SetActive(false);
            activeHover = false;
        }
    }
}