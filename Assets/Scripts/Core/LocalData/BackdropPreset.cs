using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Core.LocalData
{
    [CreateAssetMenu(fileName = "BackdropPreset", menuName = "LocalDatas/BackdropPreset")]
    public class BackdropPreset : ScriptableObject
    {
        [Tooltip("생성할 장애물의 Prefab입니다.")]
        [SerializeField] public List<GameObject> backdropPrefabs;
        [Tooltip("장애물을 생성할 개수입니다.")]
        [SerializeField] public int backdropCount = 10;
        [Tooltip("장애물이 배치될 영역의 최소/최대 좌표입니다.")]
        [SerializeField] public Vector3 minPoint, maxPoint;
    }
}