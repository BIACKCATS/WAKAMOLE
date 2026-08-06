using UnityEngine;

namespace Wakamole.Lyeon.Player
{
    public class Status : MonoBehaviour
    {
        [Header("Information")]
        [SerializeField] private int coin = 0;
        [SerializeField] private int atk = 0;
        [SerializeField] private float chargeTime = 0;
        [SerializeField] private float chargeRatio = 0;

        public int Coin { get => coin; set => coin = value; }
        public int Atk { get => atk; set => atk = value; }
        public float ChargeTime { get => chargeTime; set => chargeTime = value; }
        public float ChargeRatio { get => chargeRatio; set => chargeRatio = value; }
    }
}