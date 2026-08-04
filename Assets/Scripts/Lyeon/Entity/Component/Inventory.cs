using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.Item;

namespace Wakamole.Lyeon.Entity.Component
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<int, IItem> items = new();

        public Dictionary<int, IItem> Items => items;
    }
}