using UnityEngine;

namespace Wakamole.Lyeon.Player
{
    public class Preference : MonoBehaviour
    {
        public int Stage { get; set; }

        public bool ActiveBackdropScore { get; set; } = false;
        public int BackdropScore
        {
            get
            {
                if (!ActiveBackdropScore) return 0;
                return 10 * Stage;
            }
        }

        
    }
}