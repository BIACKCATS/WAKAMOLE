using System.Collections.Generic;
using UnityEngine;

namespace Wakamole.Core.Utils
{
    public class ObjectPool
    {
        private GameObject prefab;
        private Queue<GameObject> objects = new(); // 오브젝트 풀 구현을 위한 스택

        public int Count => objects.Count;

        /// <summary>
        /// 지정된 GameObject를 관리하는 ObjectPool을 생성합니다.
        /// </summary>
        /// <param name="prefab">ObjectPool에서 관리할 GameObject입니다.</param>
        /// <param name="count">ObjectPool에서 처음 생성할 GameObject 개수입니다. 기본값은 5입니다.</param>
        public ObjectPool(GameObject prefab, int count = 5)
        {
            this.prefab = prefab;
            // 오브젝트 풀에 미리 오브젝트를 저장함
            for (int i = 0; i < count; i++)
            {
                GameObject obj = UnityEngine.Object.Instantiate(this.prefab);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }
        }
        
        public void Return(GameObject obj)
        { 
            obj.SetActive(false);
            objects.Enqueue(obj); 
        }

        public GameObject Get() { 
            GameObject obj;
            if (objects.Count > 0) obj = objects.Dequeue();
            else obj = UnityEngine.Object.Instantiate(prefab);
            obj.SetActive(true);
            return obj;
        }
    }
}