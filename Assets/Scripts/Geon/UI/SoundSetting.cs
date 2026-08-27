using UnityEngine;
using UnityEngine.UI;
using Wakamole.Lyeon.Audio;

namespace Wakamole.Lyeon.UI
{
    public class SoundSetting : MonoBehaviour
    {
        [Header("UI Sliders")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("UI Mute Toggles (체크 시 음소거)")]
        [SerializeField] private Toggle masterMuteToggle;
        [SerializeField] private Toggle bgmMuteToggle;
        [SerializeField] private Toggle sfxMuteToggle;

        private float lastMasterVol = 1f;
        private float lastBgmVol = 1f;
        private float lastSfxVol = 1f;

        public void RefreshSliders()
        {
            AudioManager audioManager = FindFirstObjectByType<AudioManager>();
            if (audioManager == null) return;

            RemoveAllListeners();

            float mVol = audioManager.MasterVolume;
            float bVol = audioManager.BgmVolume;
            float sVol = audioManager.SfxVolume;

            // 소리가 켜져 있을 때만 복원용 볼륨값 저장
            if (mVol > 0f) lastMasterVol = mVol;
            if (bVol > 0f) lastBgmVol = bVol;
            if (sVol > 0f) lastSfxVol = sVol;

            // 슬라이더 동기화
            if (masterSlider != null) masterSlider.value = mVol;
            if (bgmSlider != null) bgmSlider.value = bVol;
            if (sfxSlider != null) sfxSlider.value = sVol;

            // 음소거 토글 동기화 (볼륨이 0이면 체크 ON)
            if (masterMuteToggle != null) masterMuteToggle.SetIsOnWithoutNotify(mVol == 0f);
            if (bgmMuteToggle != null) bgmMuteToggle.SetIsOnWithoutNotify(bVol == 0f);
            if (sfxMuteToggle != null) sfxMuteToggle.SetIsOnWithoutNotify(sVol == 0f);

            BindEvents(audioManager);
        }

        private void RemoveAllListeners()
        {
            masterSlider?.onValueChanged.RemoveAllListeners();
            bgmSlider?.onValueChanged.RemoveAllListeners();
            sfxSlider?.onValueChanged.RemoveAllListeners();

            masterMuteToggle?.onValueChanged.RemoveAllListeners();
            bgmMuteToggle?.onValueChanged.RemoveAllListeners();
            sfxMuteToggle?.onValueChanged.RemoveAllListeners();
        }

        private void BindEvents(AudioManager audioManager)
        {
            // Master Slider & Mute Toggle
            masterSlider?.onValueChanged.AddListener(val => {
                audioManager.MasterVolume = val;
                if (val > 0f) lastMasterVol = val;
                masterMuteToggle?.SetIsOnWithoutNotify(val == 0f);
            });

            masterMuteToggle?.onValueChanged.AddListener(isMuted => {
                float targetVol = isMuted ? 0f : (lastMasterVol > 0f ? lastMasterVol : 1f);
                audioManager.MasterVolume = targetVol;
                masterSlider?.SetValueWithoutNotify(targetVol);
            });

            // BGM Slider & Mute Toggle
            bgmSlider?.onValueChanged.AddListener(val => {
                audioManager.BgmVolume = val;
                if (val > 0f) lastBgmVol = val;
                bgmMuteToggle?.SetIsOnWithoutNotify(val == 0f);
            });

            bgmMuteToggle?.onValueChanged.AddListener(isMuted => {
                float targetVol = isMuted ? 0f : (lastBgmVol > 0f ? lastBgmVol : 1f);
                audioManager.BgmVolume = targetVol;
                bgmSlider?.SetValueWithoutNotify(targetVol);
            });

            // SFX Slider & Mute Toggle
            sfxSlider?.onValueChanged.AddListener(val => {
                audioManager.SfxVolume = val;
                if (val > 0f) lastSfxVol = val;
                sfxMuteToggle?.SetIsOnWithoutNotify(val == 0f);
            });

            sfxMuteToggle?.onValueChanged.AddListener(isMuted => {
                float targetVol = isMuted ? 0f : (lastSfxVol > 0f ? lastSfxVol : 1f);
                audioManager.SfxVolume = targetVol;
                sfxSlider?.SetValueWithoutNotify(targetVol);
            });
        }
    }
}