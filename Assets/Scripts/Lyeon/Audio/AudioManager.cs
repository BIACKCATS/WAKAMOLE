using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.Audio
{
    public struct AudioPreset { public float master, bgm, sfx; }

    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Components")]
        [SerializeField] private AudioData audioData;
        [SerializeField] private AudioParamData paramData;

        [Header("Audio Preferences")]
        [SerializeField] private EventReference bgm;
        [SerializeField] private float masterVolume = 1.0f;
        [SerializeField] private float bgmVolume = 1.0f;
        [SerializeField] private float sfxVolume = 1.0f;

        private List<string> ignoreParams = new();

        private EventInstance bgmPlayer;

        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                if (value < 0) masterVolume = 0;
                else if (value > 1) masterVolume = 1;
                else masterVolume = value;

                if (bgmPlayer.isValid()) bgmPlayer.setParameterByName("Volume", masterVolume);
            }
        }

        public float BgmVolume
        {
            get => bgmVolume;
            set
            {
                if (value < 0) bgmVolume = 0;
                else if (value > 1) bgmVolume = 1;
                else bgmVolume = value;

                if (bgmPlayer.isValid()) bgmPlayer.setParameterByName("BackgroundMusicVolume", bgmVolume);
            }
        }

        public float SfxVolume
        {
            get => sfxVolume;
            set
            {
                if (value < 0) sfxVolume = 0;
                else if (value > 1) sfxVolume = 1;
                else sfxVolume = value;
            }
        }

        private void Awake()
        {
            bgmPlayer = RuntimeManager.CreateInstance(bgm);
            bgmPlayer.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

            if (masterVolume != 1) MasterVolume = masterVolume;
            else MasterVolume = 1;
            if (bgmVolume != 1) BgmVolume = bgmVolume;
            else BgmVolume = 1;
            if (sfxVolume != 1) SfxVolume = sfxVolume;
            else SfxVolume = 1;

            ignoreParams.Add("BackgroundVolume");
            ignoreParams.Add("Volume");
            ignoreParams.Add("VFXVolume");
        }

        private void InitInstance(EventInstance instance)
        {
            instance.setParameterByName("BackgroundMusicVolume", MasterVolume);
            instance.setParameterByName("Volume", MasterVolume);
            instance.setParameterByName("VFXVolume", SfxVolume);
            instance.setParameterByName("Combo", 0);
            instance.setParameterByName("ShopEnter", 0);
            instance.setParameterByName("Hz", 0);
            instance.setParameterByName("DiscordAlert", 0);
        }

        public void PlayBgm()
        {
            InitInstance(bgmPlayer);
            bgmPlayer.start();
        }

        public void SetBgmParameter(string name, float value) => bgmPlayer.setParameterByName(name, value);

        public void PlaySfx(string name)
        {
            PlaySfx(name, null);
        }

        public void PlaySfx(string name, params SoundParam[] parameters)
        {
            if (!audioData.Sounds.ContainsKey(name))
            {
                Debug.LogWarning($"{name} 이름을 가진 SFX를 재생할 수 없습니다.");
                return;
            }

            EventInstance instance = RuntimeManager.CreateInstance(audioData.Sounds[name]);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

            InitInstance(instance);
            
            if (paramData.Params.ContainsKey(name))
            {
                List<SoundParam> param = paramData.Params[name];
                foreach (SoundParam soundParam in param)
                {
                    if (ignoreParams.Contains(soundParam.name)) continue;
                    instance.setParameterByName(soundParam.name, soundParam.value);
                }
            }

            if (parameters != null)
            {
                foreach (SoundParam param in parameters)
                {
                    instance.setParameterByName(param.name, param.value);
                }
            }

            instance.start();
            instance.release();
        }
    }
}