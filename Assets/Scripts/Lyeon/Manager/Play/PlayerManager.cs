using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Entity.Component;
using Wakamole.Lyeon.GameCamera;
using Wakamole.Lyeon.UI;

namespace Wakamole.Lyeon.Manager.Play
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("차지 공격 상태를 표시할 ProgressBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ProgressBar progressBar;
        
        [Header("Informations")]
        [Tooltip("플레이어의 공격력입니다.")]
        [SerializeField] private int atk = 1;
        [Tooltip("차지 공격에 필요한 시간입니다.")]
        [SerializeField] private float chargeTime = 2.0f;
        [Tooltip("차지 공격의 데미지 배율입니다.")]
        [SerializeField] private float chargeRatio = 2.0f;
        [Tooltip("공격을 감지할 Layer입니다.")]
        [SerializeField] private LayerMask layerMask;

        private float chargingTime = 0;
        private bool charging = false;

        private int coin = 0;
        
        private StageManager stageManager = null;
        private Vector2 mousePosition;
        private Ray click;
        
        public int Coin { get => coin; set => coin = value; }
        public int Atk { get => atk; set => atk = value; }

        public bool Charged
        {
            get => chargingTime >= chargeTime;
            set
            {
                if (value) chargingTime = chargeTime;
                else chargingTime = 0;
            }
        }
        
        public float ChargeTime { get => chargeTime; set => chargeTime = value; }

        public StageManager Stage { set => stageManager = value; }

        private void Start()
        {
            stageManager = StageManager.Current;
        }

        private void Update()
        {
            // 1. 차지 공격
            if (Mouse.current.rightButton.wasPressedThisFrame) charging = true;
            else if (Mouse.current.rightButton.wasReleasedThisFrame) charging = false;

            if (charging && chargingTime < chargeTime) chargingTime += Time.deltaTime;
            else if (chargingTime < chargeTime && chargingTime > 0) chargingTime -= Time.deltaTime / 2;

            if (chargingTime > chargeTime) chargingTime = chargeTime;
            else if (chargingTime < 0) chargingTime = 0;

            progressBar.Value = chargingTime / chargeTime;

            // 2. 일반 공격
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                mousePosition = Mouse.current.position.ReadValue();
                click = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(click, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.gameObject.TryGetComponent(out MoleCharactor charactor))
                    {
                        GameObject parent = charactor.transform.parent.gameObject;
                        if (parent.TryGetComponent(out Mole mole))
                        {
                            if (Charged)
                            {
                                mole.Hp -= atk * (int)chargeRatio;
                                Charged = false;
                            }
                            else mole.Hp -= atk;
                            stageManager.Combo++;

                            if (mole.Hp <= 0)
                            {
                                stageManager.AttackedMole = mole;
                                stageManager.Score += mole.Score;
                                stageManager.Count++;
                                mole.Active = false;
                            }
                        }
                    }
                    else stageManager.Combo = 0;
                }
            }
        }
    }
}