using UnityEngine;

namespace Wakamole.Lyeon.Player
{
    public struct StatusData { public int coin, atk; public float chargeTime, chargeRatio, moleSpeedPower; }
    public class Status : MonoBehaviour
    {
        [Header("Information")]
        [SerializeField] private int coin = 0;
        [SerializeField] private int atk = 0;
        [SerializeField] private float chargeTime = 0;
        [SerializeField] private float chargeRatio = 0;
        [SerializeField] private float moleSpeedPower = 1.0f;

        public int Coin { get => coin; set => coin = value; }
        public int Atk { get => atk; set => atk = value; }
        public float ChargeTime { get => chargeTime; set => chargeTime = value; }
        public float ChargeRatio { get => chargeRatio; set => chargeRatio = value; }
        public float MoleSpeed { get => moleSpeedPower; set => moleSpeedPower = value; }

        public StatusData Export() => new(){ coin = coin, atk = atk, chargeRatio = chargeRatio, chargeTime = chargeTime };
        public void Import(StatusData data)
        {
            coin = data.coin;
            atk = data.atk;
            chargeTime = data.chargeTime;
            chargeRatio = data.chargeRatio;
            moleSpeedPower = data.moleSpeedPower;
        }
    }
}