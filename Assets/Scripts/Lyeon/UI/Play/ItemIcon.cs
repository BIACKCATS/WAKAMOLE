using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.UI.Play
{
    public class ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private ItemTooltip tooltip;

        private ItemData itemData;

        public ItemData Item
        {
            get => itemData;
            set
            {
                itemData = value;
                image.sprite = itemData.itemSprite;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Use Item
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.Active = true;
            tooltip.Item = itemData;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.Active = false;
        }
    }
}