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
        [SerializeField] private StudioEventEmitter bgmPlayer;
        [SerializeField] private AudioData audioData;
        [SerializeField] private AudioParamData paramData;

        private float masterVolume = 1.0f;
        private float bgmVolume = 1.0f;
        private float sfxVolume = 1.0f;

        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = value;
                bgmPlayer.EventInstance.setParameterByName("Volume", masterVolume);
            }
        }

        public float BgmVolume
        {
            get => bgmVolume;
            set
            {
                bgmVolume = value;
                bgmPlayer.EventInstance.setParameterByName("BackgroundMusicVolume", bgmVolume);
            }
        }

        public float SfxVolume
        {
            get => sfxVolume;
            set => sfxVolume = value;
        }

        private void Awake()
        {
            MasterVolume = 1;
            BgmVolume = 1;
        }

        public void PlayBgm()
        {
            bgmPlayer.EventInstance.setParameterByName("BackgroundMusicVolume", MasterVolume);
            bgmPlayer.EventInstance.setParameterByName("Volume", MasterVolume);
            bgmPlayer.EventInstance.setParameterByName("VFXVolume", SfxVolume);
            bgmPlayer.EventInstance.setParameterByName("Combo", 0);
            bgmPlayer.EventInstance.setParameterByName("ShopEnter", 0);
            bgmPlayer.EventInstance.start();
        }

        public void SetParameter(string name, float value) => bgmPlayer.EventInstance.setParameterByName(name, value);

        public void PlaySfx(string name)
        {
            if (!audioData.Sounds.ContainsKey(name))
            {
                Debug.LogWarning($"{name} 이름을 가진 SFX를 재생할 수 없습니다.");
                return;
            }

            EventInstance instance = RuntimeManager.CreateInstance(audioData.Sounds[name]);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

            instance.setParameterByName("BackgroundMusicVolume", MasterVolume);
            instance.setParameterByName("Volume", MasterVolume);
            instance.setParameterByName("VFXVolume", SfxVolume);
            instance.setParameterByName("Combo", 0);
            instance.setParameterByName("ShopEnter", 0);
            
            if (paramData.Params.ContainsKey(name))
            {
                List<SoundParam> param = paramData.Params[name];
                foreach (SoundParam soundParam in param)
                {
                    instance.setParameterByName(soundParam.name, soundParam.value);
                }
            }

            instance.start();
            instance.release();
        }
    }
}