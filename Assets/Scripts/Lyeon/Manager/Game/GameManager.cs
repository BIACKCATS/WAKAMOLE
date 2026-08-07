using System.Collections.Generic;
using Imaginary.Core.Data;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Player;

namespace Wakamole.Lyeon.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Current { get; private set; }

        [SerializeField] private Status status;
        [SerializeField] private Preference preference;
        [SerializeField] private ItemDataList itemDataList;

        private Dictionary<int, ItemData> inventory = new();
        private GameData<StatusData> statusData;

        public int Coin { get => status.Coin; set => status.Coin = value; }
        public Preference Preference => preference;
        public Dictionary<int, ItemData> Inventory => inventory;

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
            DontDestroyOnLoad(gameObject);

            // check data
            statusData = new("data0");
            if (statusData.Exists()) status.Import(statusData.Read());
            else
            {
                StatusData data = new()
                {
                    coin = 0,
                    atk = 1,
                    chargeTime = 2.0f,
                    chargeRatio = 2.0f
                };

                status.Import(data);
                statusData.Write(data);
            }
        }

        public void UseItem(int inventoryIndex)
        {
            if (inventory[inventoryIndex] == null) return;

            int itemId = inventory[inventoryIndex].itemId;
            switch (itemId)
            {
                case 0:
                    break;
            }
        }
    }
}