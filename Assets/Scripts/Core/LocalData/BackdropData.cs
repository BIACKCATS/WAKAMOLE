using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [CreateAssetMenu(fileName = "BackdropData", menuName = "LocalDatas/BackdropData")]
    public class BackdropData : ScriptableObject
    {
        [SerializeField] public List<Sprite> objectFrame;
    }
}
