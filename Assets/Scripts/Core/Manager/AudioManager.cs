using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Core.Manager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Current { get; private set; }

        [Header("Components")]
        [Tooltip("AudioSource 컴포넌트를 포함하는 GameObject입니다.")]
        [SerializeField] private AudioSource audioPlayer;

        [Header("Information")]
        [Tooltip("BGM 정보가 저장된 AudioData를 사용하는 ScriptableObject입니다.")]
        [SerializeField] private AudioData bgmData;
        [Tooltip("SFX 정보가 저장된 AudioData를 사용하는 ScriptableObject입니다.")]
        [SerializeField] private AudioData sfxData;

        private void Awake()
        {
            if (Current != null) Destroy(gameObject);
            else
            {
                Current = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            // 테스트용
            PlayBgm(0);
        }

        public void PlayBgm(int id)
        {
            audioPlayer.clip = bgmData.audios[id];
            audioPlayer.loop = true;
            audioPlayer.Play();
        }

        public void StopBgm()
        {
            audioPlayer.Stop();
        }

        public void PlaySfx(int id)
        {
            audioPlayer.PlayOneShot(sfxData.audios[id]);
        }
    }
}