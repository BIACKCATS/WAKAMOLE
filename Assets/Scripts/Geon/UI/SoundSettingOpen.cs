using UnityEngine;
using UnityEngine.UI;

namespace Wakamole.Lyeon.UI
{
    public class SoundSettingOpen : MonoBehaviour
    {
        [Header("Target UI Window")]
        [SerializeField] private SoundSetAnim soundWindow;

        private void Awake()
        {
            // 이 스크립트가 버튼에 붙어있다면 클릭 이벤트 자동 연결
            if (TryGetComponent<Button>(out var button))
            {
                button.onClick.AddListener(ToggleWindow);
            }
        }

        private void Update()
        {
            // ESC 키 입력 감지 (버튼은 켜져있으므로 항상 감지됨)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleWindow();
            }
        }

        public void ToggleWindow()
        {
            if (soundWindow == null) return;

            if (soundWindow.IsOpen)
            {
                soundWindow.Close();
            }
            else
            {
                soundWindow.Open();
            }
        }
    }
}