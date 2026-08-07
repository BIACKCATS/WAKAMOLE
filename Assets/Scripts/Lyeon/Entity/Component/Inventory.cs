using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.Entity.Component
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<int, ItemData> items = new();

        public Dictionary<int, ItemData> Items => items;
    }
}