using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
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
                UpdateScore();
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
                UpdateScore();
            }
        }

        private void UpdateScore()
        {
            scoreText.text = $"<color=red>{current}</color>/<color=red>{target}</color>";
        }
    }
}