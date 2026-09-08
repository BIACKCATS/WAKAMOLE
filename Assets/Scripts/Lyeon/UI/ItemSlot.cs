using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI.Shop;

namespace Wakamole.Lyeon.UI
{
    public enum ItemSlotType { INVENTORY, BOOTH }

    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private int slotId;
        [SerializeField] private ItemSlotType slotType;

        private ItemDisplay data;

        public int Id { get; set; }
        public ItemDisplay Item
        {
            get => data;
            set
            {
                data = value;
                if (slotType.Equals(ItemSlotType.INVENTORY))
                {
                    if (data == null) GameManager.Current.Inventory[slotId] = null;
                    else GameManager.Current.Inventory[slotId] = value.Item;
                }
            }
        }
        public ItemSlotType SlotType => slotType;
    }
}