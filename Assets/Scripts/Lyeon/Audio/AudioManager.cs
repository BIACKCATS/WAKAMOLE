using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.Audio
{
    public struct AudioPreset { public float master, bgm, sfx; }
    
    public class AudioManager : MonoBehaviour
    {
        private float bgmVolume = 1.0f;
        private float sfxVolume = 1.0f;

        public float MasterVolume { get; set; } = 1.0f;
        public float BgmVolume
        {
            get => bgmVolume * MasterVolume;
            set => bgmVolume = value;
        }
        public float SfxVolume
        {
            get => sfxVolume * MasterVolume;
            set => sfxVolume = value;
        }
    }
}