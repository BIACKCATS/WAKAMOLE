using UnityEngine;
using Wakamole.Core.Utils;
using System.Collections;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Player;
using System.Collections.Generic;
using Wakamole.Core.LocalData;
using System.Linq;

namespace Wakamole.Lyeon.Player
{
    public class MoleManager : MonoBehaviour
    {
        // 개발용, 추후 삭제
        private WaitForSecondsRealtime _waitForSecondsRealtime = new(1.0f);

        [Header("Components")]
        [Tooltip("게임에 생성할 두더지 Prefab입니다.")]
        [SerializeField] private Mole molePrefab;
        [Tooltip("생성할 두더지의 특성입니다.")]
        [SerializeField] private List<MoleData> moleDatas;

        private ObjectPool objectPool;
        private Dictionary<MoleKeyword, MoleProfile> moles = new();

        public ObjectPool ObjectPool => objectPool;

        private void Awake()
        {
            MoleProfile preset;
            foreach (MoleData moleData in moleDatas)
            {
                preset = new()
                {
                    showTime = moleData.showTime,
                    score = moleData.score,
                    hp = moleData.hp
                };
                moles.Add(moleData.keyword, preset);
            }
        }

        private void Start()
        {
            objectPool = new(molePrefab.gameObject, 10);
            StartCoroutine(RandomMole());
        }

        /// <summary>
        /// 랜덤한 키워드의 두더지를 생성합니다.
        /// </summary>
        public void ShowMole()
        {
            List<MoleKeyword> keywords = moles.Keys.ToList();
            ShowMole(keywords[Random.Range(0, keywords.Count - 1)]);
        }

        /// <summary>
        /// 지정된 키워드를 가진 두더지를 생성합니다.
        /// </summary>
        /// <param name="keyword">두더지에게 할당될 키워드입니다.</param>
        public void ShowMole(MoleKeyword keyword)
        {
            GameObject obj = objectPool.Get();
            if (obj.TryGetComponent(out Mole mole))
            {
                mole.SetProfile(keyword, moles[keyword]);
                mole.Manager = this;

                obj.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), 0.1f, Random.Range(5.0f, -5.0f));
                obj.SetActive(true);
            }
        }

        private IEnumerator RandomMole()
        {
            // 테스트용
            while (PlayerManager.Current.Active)
            {
                yield return _waitForSecondsRealtime;
                ShowMole();
            }
        }
    }
}