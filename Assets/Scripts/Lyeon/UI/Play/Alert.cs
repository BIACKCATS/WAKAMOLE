using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.UI.Play
{
    public class Alert : MonoBehaviour, IPointerDownHandler
    {
        [Serializable]
        public struct AlertData { public Sprite icon; public string title, desc; }

        [SerializeField] private RectTransform alertTransform;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title, desc;

        [SerializeField] private List<AlertData> datas;

        private bool active = false;
        private Timer timer;
        private Vector3 initPosition = new(0, 120, 0);
        private Vector3 showPosition = new(0, -120, 0);
        private Vector3 targetPosition;

        public bool Active => active;

        private void Start()
        {
            targetPosition = initPosition;
            alertTransform.anchoredPosition3D = initPosition;
            timer = new(1.0f);
            timer.Stop();
        }

        private void Update()
        {
            if (targetPosition == showPosition)
            {
                if (timer.Active) timer.Tick(Time.deltaTime);
                else
                {
                    targetPosition = showPosition;
                    active = false;
                    timer.Stop();
                }
            }
            alertTransform.anchoredPosition3D = Vector3.Lerp(alertTransform.anchoredPosition3D, targetPosition, 20.0f * Time.fixedDeltaTime);
        }

        public void Show()
        {
            if (active) return;
            active = true;
            int pos = UnityEngine.Random.Range(0, datas.Count);
            targetPosition = showPosition;
            icon.sprite = datas[pos].icon;
            title.text = datas[pos].title;
            desc.text = datas[pos].desc;
            timer.Restart();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!active) return;
            List<Mole> moles = new();
            moles.AddRange(StageManager.Current.MoleManager.ActivatedMoles);
            foreach (Mole mole in moles)
            {
                if (mole != null && mole.Active)
                {
                    mole.Hp--;
                    if (mole.Hp <= 0)
                    {
                        StageManager.Current.AttackedMole = mole;
                        StageManager.Current.Score += mole.Score;
                        StageManager.Current.Count++;
                        mole.Active = false;

                        // 6번 아이템에 의한 보너스 시간 추가
                        if (StageManager.Current.Count % 3 == 0 && GameManager.Current.Preference.ActiveBonusTime)
                        {
                            StageManager.Current.TimeLimit += 2.0f;
                        }
                    }
                }
            }
            targetPosition = initPosition;
            active = false;
        }
    }
}