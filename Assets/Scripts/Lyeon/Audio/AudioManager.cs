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
            bgmPlayer.EventInstance.start();
        }

        public void PlaySfx(string name)
        {
            if (!audioData.Sounds.ContainsKey(name))
            {
                Debug.LogWarning($"{name} 이름을 가진 SFX를 재생할 수 없습니다.");
                return;
            }

            EventInstance instance = RuntimeManager.CreateInstance(audioData.Sounds[name]);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

            /// 여기다 Param 데이터 지정
            /// 아래 코드 변형해서 만일 audioParamData에 해당 이름이 존재하는 경우 해당 값으로 덮어씌우기
            /// 작성하기

            /*if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!string.IsNullOrEmpty(parameters[i].name))
                    {
                        instance.setParameterByName(parameters[i].name, parameters[i].value);
                    }
                }
            } */

            instance.start();
            instance.release();
        }
    }
}