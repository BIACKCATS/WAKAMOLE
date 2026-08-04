using UnityEngine;
using Wakamole.Lyeon.Item;

namespace Wakamole.Lyeon.UI
{
    public class ItemSlot : MonoBehaviour
    {
        public int Id { get; set; }
        public IItem Item { get; set; }
    }
}