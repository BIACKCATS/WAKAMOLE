using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.Audio
{
    public struct AudioPreset { public float master, bgm, sfx; }
    
    public class AudioManager : MonoBehaviour
    {
        public float MasterVolume { get; set; } = 1.0f;
        public float BgmVolume
        {
            get => bgmSource.volume;
            set
            {
                bgmVolume = value;
                bgmSource.volume = bgmVolume * MasterVolume;
            }
        }
        public float SfxVolume
        {
            get => sfxSource.volume;
            set
            {
                sfxVolume = value;
                sfxSource.volume = sfxVolume * MasterVolume;
            }
        }

        [SerializeField] private AudioSource bgmSource, sfxSource;
        [SerializeField] private AudioData bgmData, sfxData;

        private float bgmVolume = 1.0f, sfxVolume = 1.0f;

        public void PlayBgm(int id)
        {
            bgmSource.clip = bgmData.audios[id];
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void PlayBgm(AudioClip clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void PlaySfx(int id)
        {
            sfxSource.PlayOneShot(sfxData.audios[id]);
        }

        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void StopBgm() => bgmSource.Stop();
        public void PauseBgm() => bgmSource.Pause();
    }
}