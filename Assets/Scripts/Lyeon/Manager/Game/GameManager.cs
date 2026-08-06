using System.Collections.Generic;
using Imaginary.Core.Data;
using UnityEngine;
using Wakamole.Lyeon.Item;
using Wakamole.Lyeon.Player;

namespace Wakamole.Lyeon.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Current { get; private set; }

        [SerializeField] private Status status;

        private int coin = 0;
        private Dictionary<int, IItem> inventory = new();
        private GameData<StatusData> statusData;

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
    }
}