using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Manager;

namespace Wakamole.Lyeon.UI.Shop
{
    public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        private bool activeHover = false;
        private bool activeDrag = false;
        private Vector2 offset;

        private ShopManager shop = null;

        public ShopManager Shop { set => shop = value; }

        private void Awake()
        {
            shop = ShopManager.Current;
        }

        private void Update()
        {
            if (activeDrag) transform.position = Mouse.current.position.ReadValue() + offset;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (shop == null) return;
            shop.Tooltip.Active = true;
            activeHover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (shop == null) return;
            shop.Tooltip.Active = false;
            activeHover = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            activeDrag = true;
            offset = (Vector2)transform.position - Mouse.current.position.ReadValue();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            activeDrag = false;
        }
    }
}