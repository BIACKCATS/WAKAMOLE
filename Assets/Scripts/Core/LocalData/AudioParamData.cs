using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [Serializable]
    public struct ParamData
    {
        public string soundName;
        public List<SoundParam> soundParam;
    }

    [CreateAssetMenu(fileName = "AudioParamData", menuName = "LocalDatas/AudioParamData")]
    public class AudioParamData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<ParamData> param = new();

        public Dictionary<string, List<SoundParam>> Params { get; private set; }

        public void OnAfterDeserialize()
        {
            Params = new();
            if (param == null || param.Count == 0) return;
            for (int i = 0; i < param.Count; i++)
                Params.Add(param[i].soundName, param[i].soundParam);
        }

        public void OnBeforeSerialize() {}
    }
}
