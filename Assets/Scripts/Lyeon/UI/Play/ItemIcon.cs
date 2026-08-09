using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI.Play
{
    public class ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private int slotId;
        [SerializeField] private Image image;
        [SerializeField] private ItemTooltip tooltip;

        [SerializeField] private Sprite defaultSprite;

        private ItemData itemData;

        public ItemData Item
        {
            get => itemData;
            set
            {
                itemData = value;
                if (itemData == null) image.sprite = defaultSprite;
                else image.sprite = itemData.itemSprite;
            }
        }

        private void OnEnable()
        {
            if (itemData == null) image.sprite = defaultSprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Use Item
            GameManager.Current.UseItem(slotId);
            Item = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemData == null) return;
            tooltip.Active = true;
            tooltip.Item = itemData;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.Active = false;
        }
    }
}