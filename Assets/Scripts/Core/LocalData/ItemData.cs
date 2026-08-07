using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "LocalDatas/ItemData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] public Sprite itemSprite;
        [SerializeField] public int itemId;
        [SerializeField] public int itemCost;
        [SerializeField] public string itemName;
        [SerializeField, TextArea] public string itemDesc;
        [SerializeField, TextArea] public string itemFunc;
    }
}