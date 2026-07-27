using System;
using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI
{
    public class ScoreBoard : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("점수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text scoreText;

        private int target = 10;
        private int current = 0;

        /// <summary>
        /// 목표 점수입니다.
        /// </summary>
        public int Goal
        {
            set
            {
                target = value;
                UpdateBoard();
            }
        }
        /// <summary>
        /// 현재 점수입니다.
        /// </summary>
        public int Current
        {
            set
            {
                current = value;
                UpdateBoard();
            }
        }

        private void UpdateBoard()
        {
            scoreText.text = string.Format("{0} / {1}", current, target);
        }
    }
}