using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI
{
    public class LoadingText : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("점수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text scoreText;

        [Header("Information")]
        [Tooltip("로딩 중 표시할 텍스트입니다.")]
        [SerializeField] private List<string> texts = new();

        private void OnEnable()
        {
            scoreText.text = texts[Random.Range(0, texts.Count - 1)];
        }
    }
}