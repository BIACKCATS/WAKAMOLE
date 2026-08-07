using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI
{
    public enum ItemSlotType { INVENTORY, BOOTH }

    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private int slotId;
        [SerializeField] private ItemSlotType slotType;

        private ItemData data;

        public int Id { get; set; }
        public ItemData Item
        {
            get => data;
            set
            {
                data = value;
                if (slotType.Equals(ItemSlotType.INVENTORY))
                    GameManager.Current.Inventory[slotId] = value;
            }
        }
        public ItemSlotType SlotType => slotType;
    }
}