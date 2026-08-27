using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [Serializable]
    public struct SoundData
    {
        public EventReference soundRef;
        public string soundName;
    }

    [CreateAssetMenu(fileName = "AudioData", menuName = "LocalDatas/AudioData")]
    public class AudioData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SoundData> sounds = new();

        public Dictionary<string, EventReference> Sounds { get; private set; }

        public void OnAfterDeserialize()
        {
            Sounds = new();
            if (sounds == null || sounds.Count == 0) return;
            for (int i = 0; i < sounds.Count; i++)
            {
                Sounds.Add(sounds[i].soundName, sounds[i].soundRef);
            }
        }

        public void OnBeforeSerialize() {}
    }
}
