using UnityEngine;
using Wakamole.Core.Utils;
using System.Collections;
using Wakamole.Lyeon.Entity;

namespace Wakamole.Lyeon.Manager
{
    public class MoleManager : MonoBehaviour
    {
        private static WaitForSecondsRealtime _waitForSecondsRealtime = new WaitForSecondsRealtime(1.0f);
        [SerializeField] private Mole molePrefab;

        private ObjectPool objectPool;

        private void Awake()
        {
            objectPool = new(molePrefab.gameObject, 10);
            StartCoroutine(RandomMole());
        }

        IEnumerator RandomMole()
        {
            while (true)
            {
                yield return _waitForSecondsRealtime;
                GameObject obj = objectPool.Get();
                if (obj.TryGetComponent(out Mole mole))
                {
                    obj.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), 0.1f, Random.Range(5.0f, -5.0f));
                    obj.SetActive(true);
                    mole.Pool = objectPool;
                }
            }
        }
    }
}