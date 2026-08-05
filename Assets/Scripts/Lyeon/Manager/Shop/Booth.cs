using UnityEngine;
using UnityEngine.UI;
using Wakamole.Lyeon.UI;
using Wakamole.Lyeon.UI.Shop;

namespace Wakamole.Lyeon.Manager.Shop
{
    public class Booth : ItemSlotList
    {
        [Header("Components")]
        [Tooltip("상점을 관리하는 ShopManager를 포함한 GameObject입니다.")]
        [SerializeField] private ShopManager shopManager;
        [Tooltip("아이템을 표시하는 Prefab입니다.")]
        [SerializeField] private ItemDisplay itemDisplay;

        private Canvas shopCanvas;

        protected void Start()
        {
            Canvas.ForceUpdateCanvases();

            if (shopManager.TryGetComponent(out Canvas canvas)) shopCanvas = canvas;
            else return;

            foreach (ItemSlot slot in itemSlots)
            {
                GameObject obj = Instantiate(itemDisplay.gameObject, shopCanvas.transform);
                if (obj.TryGetComponent(out ItemDisplay component) &&
                    obj.TryGetComponent(out RectTransform rect) &&
                    slot.TryGetComponent(out RectTransform slotRect))
                {
                    component.TargetPosition = slotRect.position;
                    rect.position = slotRect.position;
                }
            }
        }
    }
}