using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.Item;

namespace Wakamole.Lyeon.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Current { get; private set; }

        private int coin = 0;
        private Dictionary<int, IItem> inventory = new();

        public int Coin { get => coin; set => coin = value; }
        public Dictionary<int, IItem> Inventory => inventory;

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}