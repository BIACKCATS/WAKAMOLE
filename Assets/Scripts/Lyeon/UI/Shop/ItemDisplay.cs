using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Item;
using Wakamole.Lyeon.Manager;

namespace Wakamole.Lyeon.UI.Shop
{
    public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        private bool activeHover = false;
        private bool activeDrag = false;

        private Vector2 offset;
        private Vector2 initPosition, targetPosition;

        private IItem item;

        private List<RaycastResult> results = new();

        private ShopManager shop = null;

        public ShopManager Shop { set => shop = value; }
        public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }

        private void Awake()
        {
            targetPosition = initPosition = transform.position;
        }

        private void Start()
        {
            shop = ShopManager.Current;
        }

        private void Update()
        {
            if (activeDrag) targetPosition = Mouse.current.position.ReadValue() + offset;
            transform.position = Vector2.Lerp(transform.position, targetPosition, 50.0f * Time.deltaTime);
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
            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out ItemSlot slot))
                {
                    slot.Item = item;
                    targetPosition = slot.gameObject.transform.position;
                    return;
                }
            }
        }
    }
}