using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "LocalDatas/AudioData")]
    public class AudioData : ScriptableObject
    {
        [Tooltip("오디오 목록입니다. index가 id의 역할을 합니다.")]
        [SerializeField] public List<AudioClip> audios;
    }
}
