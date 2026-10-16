using UnityEngine;

namespace Wakamole.Lyeon.UI.Stages
{
    public class StagePoint : MonoBehaviour
    {
        [SerializeField] private int stageId;
        [SerializeField] private RectTransform rect;

        public int StageId => stageId;
        public RectTransform Rect => rect;
    }
}