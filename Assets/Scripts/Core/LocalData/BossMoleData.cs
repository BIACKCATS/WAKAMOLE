using System;
using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.Entity;

namespace Wakamole.Core.LocalData
{
    [Serializable]
    public struct BossKeywordData
    {
        [Tooltip("해당 설정을 적용할 특성입니다.")]
        public BossKeyword keyword;
        [Tooltip("두더지의 이름입니다.")]
        public string moleName;
    }

    [CreateAssetMenu(fileName = "BossMoleData", menuName = "LocalDatas/BossMoleData")]
    public class BossMoleData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<BossKeywordData> bossMoles = new();

        public Dictionary<BossKeyword, string> BossMoleName { get; private set; }

        public void OnAfterDeserialize()
        {
            BossMoleName = new();
            if (bossMoles == null || bossMoles.Count == 0) return;
            for (int i = 0; i < bossMoles.Count; i++)
            {
                if (BossMoleName.ContainsKey(bossMoles[i].keyword))
                {
                    Debug.Log($"{i}번째 항목의 키워드 \"{bossMoles[i].keyword}\"가 이미 존재합니다. 해당 키워드의 값 \"{BossMoleName[bossMoles[i].keyword]}\"가 \"{bossMoles[i].moleName}\"로 덮어씌워집니다.");
                    BossMoleName[bossMoles[i].keyword] = bossMoles[i].moleName;
                    continue;
                }
                BossMoleName.Add(bossMoles[i].keyword, bossMoles[i].moleName);
            }
        }

        public void OnBeforeSerialize() {}
    }
}
