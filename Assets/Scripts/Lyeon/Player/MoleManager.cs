using UnityEngine;
using Wakamole.Core.Utils;
using System.Collections;
using Wakamole.Lyeon.Entity;
using System.Collections.Generic;
using Wakamole.Core.LocalData;

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

        [Header("Information")]
        [Tooltip("두더지의 기본 체력입니다.")]
        [SerializeField] private int defaultHp = 10;
        [Tooltip("두더지의 기본 등장 시간입니다.")]
        [SerializeField] private int defaultShowTime = 3;
        [Tooltip("두더지의 기본 점수입니다.")]
        [SerializeField] private int defaultScore = 1;

        private ObjectPool objectPool;
        private Dictionary<MoleKeyword, MoleData> moles = new();
        private List<MoleKeyword> keywords = new();

        public ObjectPool ObjectPool => objectPool;

        private void Awake()
        {
            foreach (MoleData moleData in moleDatas)
            {
                moles.Add(moleData.keyword, moleData);
                keywords.Add(moleData.keyword);
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
                mole.Init(defaultHp, defaultScore, defaultShowTime);
                foreach (MoleKeyword moleKeyword in keywords)
                {
                    if ((moleKeyword & keyword) != 0)
                        mole.AddKeyword(keyword, moles[keyword]);
                }
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