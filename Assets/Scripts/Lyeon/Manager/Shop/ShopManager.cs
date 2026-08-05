using UnityEngine;
using Wakamole.Lyeon.UI.Shop;

namespace Wakamole.Lyeon.Manager.Shop
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Current { get; private set; }

        [SerializeField] private ItemTooltip tooltip;

        public ItemTooltip Tooltip => tooltip;

        private void Awake()
        {
            Current = this;
        }

        private void OnDisable()
        {
            Current = null;
        }
    }
}