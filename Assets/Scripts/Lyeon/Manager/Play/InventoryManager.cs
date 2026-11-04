using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Item;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.Manager.Play
{
    public class InventoryManager : MonoBehaviour
    {
        [Tooltip("아이템 목록을 표시할 ItemIcon 스크립트를 포함한 GameObject의 목록입니다.")]
        [SerializeField] private List<ItemIcon> itemSlots;

        private List<IItem> items = new();
        private bool active = false;

        private void OnEnable()
        {
            if (GameManager.Current == null) return;
            
            ItemData temp;
            for (int i = 0; i < 5; i++)
            {
                if (!GameManager.Current.Inventory.ContainsKey(i)) continue;
                temp = GameManager.Current.Inventory[i];
                AddItem(temp.itemId);
                itemSlots[i].Item = temp;
            }
            active = items.Count > 0;
        }

        public void OnAttack(IEntity entity)
        {
            if (!active) return;
            foreach (IItem item in items)
            {
                if (item is IAttackItem attackItem) attackItem.OnAttack(entity);
            }
        }

        private void AddItem(int itemId)
        {
            IItem item = null;

            switch (itemId)
            {
                case 0:
                    item = new ItemBroom();
                    break;
                case 1:
                    item = new ItemBowl();
                    break;
                case 2:
                    item = new ItemGeometric();
                    break;
                case 3:
                    item = new ItemCoupon();
                    break;
                case 4:
                    item = new ItemAlert();
                    break;
                case 5:
                    item = new ItemMessage();
                    break;
                case 6:
                    item = new ItemClock();
                    break;
                case 7:
                    item = new ItemSleep();
                    break;
                case 8:
                    item = new ItemMosquitto();
                    break;
                case 9:
                    item = new ItemWash();
                    break;
            }

            if (item != null)
            {
                items.Add(item);
                item.Enable();
            }
            else Debug.LogError($"Unknown Item ID: {itemId}");
        }
    }
}