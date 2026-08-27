using UnityEngine;
using UnityEngine.UI;

namespace Wakamole.Lyeon.Audio
{
    public class SoundSetting : MonoBehaviour
    {
        [Header("Audio Manager Reference")]
        [SerializeField] private AudioManager audioManager;

        [Header("Master Volume UI")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Toggle masterMuteToggle;
        private float preMasterVolume = 1f;

        [Header("BGM Volume UI")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Toggle bgmMuteToggle;
        private float preBgmVolume = 1f;

        [Header("SFX Volume UI")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle sfxMuteToggle;
        private float preSfxVolume = 1f;

        private void Start()
        {
            if (audioManager == null)
            {
                Debug.LogError("AudioManager가 할당되지 않았습니다.");
                return;
            }

            // 1. 슬라이더 초기값 설정 
            if (masterSlider != null) masterSlider.value = audioManager.MasterVolume;
            if (bgmSlider != null) bgmSlider.value = audioManager.BgmVolume;
            if (sfxSlider != null) sfxSlider.value = audioManager.SfxVolume;

            // 2. 슬라이더 이벤트 연결
            if (masterSlider != null)
                masterSlider.onValueChanged.AddListener(val => audioManager.MasterVolume = val);

            if (bgmSlider != null)
                bgmSlider.onValueChanged.AddListener(val => audioManager.BgmVolume = val);

            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(val => audioManager.SfxVolume = val);

            // 3. 개별 음소거 토글 이벤트 연결
            if (masterMuteToggle != null)
                masterMuteToggle.onValueChanged.AddListener(OnMasterMuteToggled);

            if (bgmMuteToggle != null)
                bgmMuteToggle.onValueChanged.AddListener(OnBgmMuteToggled);

            if (sfxMuteToggle != null)
                sfxMuteToggle.onValueChanged.AddListener(OnSfxMuteToggled);
        }

        // Master 음소거
        private void OnMasterMuteToggled(bool isMuted)
        {
            if (masterSlider == null) return;

            if (isMuted)
            {
                // 음소거 전 볼륨 기억 
                if (masterSlider.value > 0.001f) preMasterVolume = masterSlider.value;
                masterSlider.value = 0f;
                masterSlider.interactable = false; // 슬라이더 비활성화
            }
            else
            {
                masterSlider.value = preMasterVolume;
                masterSlider.interactable = true;  // 슬라이더 다시 활성화
            }
        }

        // BGM 음소거
        private void OnBgmMuteToggled(bool isMuted)
        {
            if (bgmSlider == null) return;

            if (isMuted)
            {
                if (bgmSlider.value > 0.001f) preBgmVolume = bgmSlider.value;
                bgmSlider.value = 0f;
                bgmSlider.interactable = false;
            }
            else
            {
                bgmSlider.value = preBgmVolume;
                bgmSlider.interactable = true;
            }
        }

        // SFX 음소거
        private void OnSfxMuteToggled(bool isMuted)
        {
            if (sfxSlider == null) return;

            if (isMuted)
            {
                if (sfxSlider.value > 0.001f) preSfxVolume = sfxSlider.value;
                sfxSlider.value = 0f;
                sfxSlider.interactable = false;
            }
            else
            {
                sfxSlider.value = preSfxVolume;
                sfxSlider.interactable = true;
            }
        }
    }
}