using UnityEngine;
using Wakamole.Lyeon.Entity;

namespace Wakamole.Core.LocalData
{
    [CreateAssetMenu(fileName = "MoleData", menuName = "LocalDatas/MoleData")]
    public class MoleData : ScriptableObject
    {
        [Tooltip("해당 설정을 적용할 특성입니다.")]
        [SerializeField] public MoleKeyword keyword;

        [Tooltip("해당 키워드의 체력입니다.")]
        [SerializeField] public int hp;
        [SerializeField] public bool isFixedHp = false;

        [Tooltip("해당 키워드의 등장 시간입니다.")]
        [SerializeField] public float showTime;
        [SerializeField] public bool isFixedTime = false;

        [Tooltip("두더지의 획득 가능한 점수입니다.")]
        [SerializeField] public int score;
        [SerializeField] public bool isFixedScore = false;
    }
}
