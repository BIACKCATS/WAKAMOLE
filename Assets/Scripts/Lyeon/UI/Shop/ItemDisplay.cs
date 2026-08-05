using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Item;
using Wakamole.Lyeon.Manager.Shop;

namespace Wakamole.Lyeon.UI.Shop
{
    public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform rect;

        private bool activeHover = false;
        private bool activeDrag = false;

        private Vector2 initPosition, targetPosition, offset;

        private IItem item;

        private List<RaycastResult> results = new();

        private ShopManager shop = null;

        public ShopManager Shop { set => shop = value; }
        public IItem Item { get => item; set => item = value; }
        public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }

        private void Start()
        {
            shop = ShopManager.Current;
        }

        private void Update()
        {
            rect.position = Vector2.Lerp(rect.position, targetPosition, 50.0f * Time.deltaTime);
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
            offset = (Vector2)rect.position - Mouse.current.position.ReadValue();
        }

        public void OnDrag(PointerEventData eventData)
        {
            targetPosition = Mouse.current.position.ReadValue() + offset;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            activeDrag = false;
            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out ItemSlot slot) &&
                    slot.TryGetComponent(out RectTransform slotRect))
                {
                    slot.Item = item;
                    targetPosition = slotRect.position;
                    return;
                }
            }
        }
    }
}