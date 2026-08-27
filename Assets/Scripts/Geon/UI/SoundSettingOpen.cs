using UnityEngine;

namespace Wakamole.Lyeon.UI
{
    public class SoundSettingOpen : MonoBehaviour
    {
        // 인스펙터에서 사운드 UI 창(PauseSoundWindow가 붙은 오브젝트)을 연결
        [SerializeField] private SoundSetAnim soundWindow;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (soundWindow != null)
                {
                    soundWindow.ToggleWindow();
                }
            }
        }
    }
}