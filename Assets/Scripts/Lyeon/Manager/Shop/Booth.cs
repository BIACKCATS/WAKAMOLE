using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wakamole.Core.LocalData;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.Manager.Game;
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
        [SerializeField] private ItemDataList itemDataList;

        private ItemPool objectPool;

        private void Start()
        {
            Canvas.ForceUpdateCanvases();
            objectPool = new(itemDisplay.gameObject, shopManager.gameObject, 8);
            Reroll();
        }

        public void Reroll()
        {
            List<ItemData> tempData = new();
            ItemData selectedItem = null;
            int index = 0;
            tempData.AddRange(itemDataList.itemDatas);

            foreach (ItemSlot slot in itemSlots)
            {
                if (slot.Item != null)
                {
                    objectPool.Return(slot.Item.gameObject);
                    slot.Item = null;
                }

                GameObject obj = objectPool.Get();
                if (obj.TryGetComponent(out ItemDisplay component) &&
                    obj.TryGetComponent(out RectTransform rect) &&
                    slot.TryGetComponent(out RectTransform slotRect))
                {
                    do
                    {
                        index = Random.Range(0, tempData.Count);
                        if (!GameManager.Current.Inventory.ContainsValue(tempData[index]))
                        {
                            selectedItem = tempData[index];
                            break;
                        }
                        else continue;
                    }
                    while (!GameManager.Current.Inventory.ContainsValue(selectedItem));

                    component.CurrentSlot = slot;
                    component.Item = selectedItem;
                    component.TargetPosition = slotRect.position;
                    rect.position = slotRect.position;
                    slot.Item = component;
                    tempData.RemoveAt(index);
                }
            }
        }
    }
}