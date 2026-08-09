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

        private int stageId = 0;
        private Dictionary<int, ItemData> inventory = new(5);

        public int StageId { get => stageId; set => stageId = value; }
        public int Coin { get => status.Coin; set => status.Coin = value; }
        public Preference Preference => preference;
        public Status Status => status;
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
            StatusData data = new()
            {
                coin = Coin,
                atk = 1,
                chargeTime = 2.0f,
                chargeRatio = 2.0f
            };
            status.Import(data);
        }

        public void UseItem(int inventoryIndex)
        {
            if (!inventory.ContainsKey(inventoryIndex) || inventory[inventoryIndex] == null) return;

            int itemId = inventory[inventoryIndex].itemId;
            switch (itemId)
            {
                case 0:
                    break;
            }
        }
    }
}