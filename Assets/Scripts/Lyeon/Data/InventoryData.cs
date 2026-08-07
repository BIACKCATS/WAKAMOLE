using System;
using System.Collections.Generic;

namespace Wakamole.Lyeon.Data
{
    [Serializable]
    public struct InventoryData
    {
        public List<InventorySlot> slots;
    }

    [Serializable]
    public struct InventorySlot
    {
        public int index, itemId;
    }
}