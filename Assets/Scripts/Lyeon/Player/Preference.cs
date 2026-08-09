using UnityEngine;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.Player
{
    public class Preference : MonoBehaviour
    {
        public int Stage { get; set; }

        // Item 0
        public bool ActiveBackdropScore { get; set; } = false;
        public int BackdropScore
        {
            get
            {
                if (!ActiveBackdropScore) return 0;
                return 10 * Stage;
            }
        }

        // Item 1
        public bool ActiveHitScore { get; set; } = false;
        public int HitScore
        {
            get
            {
                if (!ActiveHitScore) return 0;
                return 1;
            }
        }

        // Item 2
        public bool ActiveMolePower { get; set; } = false;
        public float MolePower
        {
            get
            {
                if (!ActiveMolePower) return 1.0f;
                return 1.5f;
            }
        }

        // Item 3
        public bool ActiveComboScore { get; set; } = false;

        // Item 4 (미구현, 구현 필요)
        public bool ActiveAlert { get; set; } = false;

        // Item 5 (소리 변경은 미구현, 구현 필요)
        public bool ActiveDiscordSound { get; set; } = false;
        public float MoleBonusTime
        {
            get
            {
                if (!ActiveDiscordSound) return 0;
                return 1.5f;
            }
        }

        // Item 6
        public bool ActiveBonusTime { get; set; } = false;

        // Item 7 (미구현, 구현 필요)
        public bool ActiveFailScore { get; set; } = false;
        public float FailScore
        {
            get
            {
                if (!ActiveFailScore) return 0;
                return 0.2f;
            }
        }

        // Item 8 (미구현, 구현 필요)
        public bool ActiveMosquito { get; set; } = false;

        // Item 9
        public bool ActiveBonusScore { get; set; } = false;
        public int BonusScore
        {
            get
            {
                if (!ActiveBonusScore) return 0;
                return 1;
            }
        }

        private void Reset()
        {
            ActiveBackdropScore = false;
            ActiveHitScore = false;
            ActiveMolePower = false;
            ActiveComboScore = false;
            ActiveAlert = false;
            ActiveDiscordSound = false;
            ActiveBonusTime = false;
            ActiveFailScore = false;
            ActiveMosquito = false;
            ActiveBonusScore = false;
        }    
    }
}