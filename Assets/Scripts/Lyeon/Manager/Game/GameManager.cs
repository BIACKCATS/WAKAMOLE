using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Audio;
using Wakamole.Lyeon.Manager.Play;
using Wakamole.Lyeon.Player;

namespace Wakamole.Lyeon.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Current { get; private set; }

        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Status status;
        [SerializeField] private Preference preference;
        [SerializeField] private ItemDataList itemDataList;

        private int stageId = 0;
        private Dictionary<int, ItemData> inventory = new(5);

        public int StageId { get => stageId; set => stageId = value; }
        public int Coin { get => status.Coin; set => status.Coin = value; }

        public AudioManager Audio => audioManager;
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
        }

        private void Start()
        {
            audioManager.PlayBgm();
        }

        public void UseItem(int inventoryIndex)
        {
            if (!inventory.ContainsKey(inventoryIndex) || inventory[inventoryIndex] == null) return;

            int itemId = inventory[inventoryIndex].itemId;
            switch (itemId)
            {
                case 0:
                    preference.ActiveBackdropScore = true;
                    break;
                case 1:
                    preference.ActiveHitScore = true;
                    break;
                case 2:
                    preference.ActiveMolePower = true;
                    break;
                case 3:
                    preference.ActiveComboScore = true;
                    break;
                case 4:
                    preference.ActiveAlert = true;
                    if (StageManager.Current != null) StageManager.Current.StartAlert();
                    break;
                case 5:
                    preference.ActiveDiscordSound = true;
                    break;
                case 6:
                    preference.ActiveBonusTime = true;
                    break;
                case 7:
                    preference.ActiveFailScore = true;
                    break;
                case 8:
                    preference.ActiveMosquito = true;
                    if (StageManager.Current != null) StageManager.Current.StartMosquito();
                    break;
                case 9:
                    preference.ActiveBonusScore = true;
                    break;
            }
            // inventory[inventoryIndex] = null;
        }
    }
}