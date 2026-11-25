using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [CreateAssetMenu(fileName = "ItemDataList", menuName = "LocalDatas/ItemDataList")]
    public class ItemDataList : ScriptableObject
    {
        [SerializeField] public List<ItemData> itemDatas;
    }
}