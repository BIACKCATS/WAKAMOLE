using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.Manager.Component;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI;
using Wakamole.Lyeon.UI.Play;
using Wakamole.Lyeon.UI.Shop;

namespace Wakamole.Lyeon.Manager.Shop
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Current { get; private set; }

        [SerializeField] CursorChange cursor;
        [SerializeField] private ItemTooltip tooltip;
        [SerializeField] private CoinText coinText;
        [SerializeField] private ItemDisplay itemDisplayPrefab;
        [SerializeField] private List<ItemSlot> inventory;

        public ItemTooltip Tooltip => tooltip;

        public void UpdateCoin()
        {
            coinText.Coin = GameManager.Current.Coin;
        }

        private void OnEnable()
        {
            Current = this;
            cursor.SetCursor(0);

            for (int i = 0; i < 5; i++)
            {
                if (!GameManager.Current.Inventory.ContainsKey(i) || GameManager.Current.Inventory[i] == null) continue;

                GameObject obj = Instantiate(itemDisplayPrefab.gameObject, gameObject.transform);
                if (obj.TryGetComponent(out ItemDisplay component) &&
                    obj.TryGetComponent(out RectTransform objRect) &&
                    inventory[i].TryGetComponent(out RectTransform rect))
                {
                    component.CurrentSlot = inventory[i];
                    component.Item = GameManager.Current.Inventory[i];
                    component.TargetPosition = rect.position;
                    objRect.position = rect.position;
                    inventory[i].Item = component;
                }
            }

            coinText.Coin = GameManager.Current.Coin;
            GameManager.Current.Audio.SetBgmParameter("ShopEnter", 100);
        }

        private void OnDisable()
        {
            GameManager.Current.Audio.SetBgmParameter("ShopEnter", 0);
            Current = null;
        }
    }
}