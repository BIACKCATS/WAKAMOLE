using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Lyeon.UI
{
    public class ItemSlotList : MonoBehaviour
    {
        [SerializeField] protected List<ItemSlot> itemSlots = new();

        public List<ItemSlot> Slots => itemSlots;

        protected virtual void Awake()
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                itemSlots[i].Id = i;
            }
        }
    }
}