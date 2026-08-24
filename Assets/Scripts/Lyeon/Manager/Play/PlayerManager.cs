using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Entity.Component;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.Manager.Play
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image handImage, attackImage;
        [Tooltip("차지 공격 상태를 표시할 ProgressBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private List<ChargeFire> fires;
        
        [Header("Informations")]
        [Tooltip("공격을 감지할 Layer입니다.")]
        [SerializeField] private LayerMask layerMask;

        private float chargingTime = 0;
        private bool charging = false;

        private int coin = 0;
        
        private StageManager stageManager = null;
        private Vector2 mousePosition;
        private Ray click;
        
        public int Coin { get => coin; set => coin = value; }

        public bool Charged
        {
            get => chargingTime >= GameManager.Current.Status.ChargeTime;
            set
            {
                if (value) chargingTime = GameManager.Current.Status.ChargeTime;
                else chargingTime = 0;
                foreach (ChargeFire fire in fires) fire.gameObject.SetActive(value);
            }
        }
        
        private void Start()
        {
            stageManager = StageManager.Current;
            attackImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!stageManager.Active) return;

            // 1. 차지 공격
            if (Mouse.current.rightButton.wasPressedThisFrame) charging = true;
            else if (Mouse.current.rightButton.wasReleasedThisFrame) charging = false;

            if (charging && chargingTime < GameManager.Current.Status.ChargeTime) chargingTime += Time.deltaTime;
            else if (chargingTime < GameManager.Current.Status.ChargeTime && chargingTime > 0) chargingTime -= Time.deltaTime / 2;

            if (chargingTime > GameManager.Current.Status.ChargeTime) chargingTime = GameManager.Current.Status.ChargeTime;
            else if (chargingTime < 0) chargingTime = 0;

            progressBar.Value = chargingTime / GameManager.Current.Status.ChargeTime;
            if (progressBar.Value > 0.3f) fires[0].gameObject.SetActive(true);
            if (progressBar.Value > 0.6f) fires[1].gameObject.SetActive(true);
            if (Charged) fires[2].gameObject.SetActive(true);

            // 2. 일반 공격
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                mousePosition = Mouse.current.position.ReadValue();
                click = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(click, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.gameObject.TryGetComponent(out Mosquito mosquito))
                    {
                        // 9번 아이템에 의한 모기 퇴치
                        mosquito.DestroyMosquito();
                    }
                    if (hit.collider.gameObject.TryGetComponent(out Backdrop backdrop))
                    {
                        // 1번 아이템에 의한 점수 추가 (자동 계산)
                        stageManager.Score += GameManager.Current.Preference.BackdropScore;
                        backdrop.Hit();
                    }
                    else if (hit.collider.gameObject.TryGetComponent(out MoleCharactor charactor))
                    {
                        GameObject parent = charactor.transform.parent.gameObject;
                        if (parent.TryGetComponent(out Mole mole))
                        {
                            int beforeHp = mole.Hp;
                            // 2번 아이템에 의한 점수 추가
                            if (Charged)
                            {
                                mole.Hp -= GameManager.Current.Status.Atk * (int)GameManager.Current.Status.ChargeRatio;
                                Charged = false;
                            }
                            else mole.Hp -= GameManager.Current.Status.Atk;
                            
                            if (mole.Hp < beforeHp) stageManager.Score += GameManager.Current.Preference.HitScore;
                            
                            // 3번/10번 아이템에 의한 점수 추가 (자동 계산)
                            stageManager.Score += ((++stageManager.Combo) % 5) * (int)GameManager.Current.Preference.MolePower + GameManager.Current.Preference.BonusScore;

                            // 4번 아이템에 의한 콤보점수 추가
                            if (GameManager.Current.Preference.ActiveComboScore) stageManager.Score += stageManager.Combo;

                            if (mole.Hp <= 0)
                            {
                                stageManager.AttackedMole = mole;
                                stageManager.Score += stageManager.ActiveDoubleScore ? mole.Score * 2 : mole.Score;
                                stageManager.Count++;
                                mole.Active = false;

                                // 6번 아이템에 의한 보너스 시간 추가
                                if (stageManager.Count % 3 == 0 && GameManager.Current.Preference.ActiveBonusTime)
                                {
                                    stageManager.TimeLimit += 2.0f;
                                }
                            }
                        }
                        return;
                    }
                    else stageManager.Combo = 0;
                }
            }
        }
    }
}