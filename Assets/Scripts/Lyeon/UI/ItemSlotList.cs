using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Lyeon.UI
{
    public class ItemSlotList : MonoBehaviour
    {
        [SerializeField] private List<ItemSlot> itemSlots = new();

        private void Awake()
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                itemSlots[i].Id = i;
            }
        }
    }
}