using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.Manager.Play
{
    public class MosquitoPool
    {
        private List<Mosquito> mosquitoes;
        private int activated = 0;

        [System.Obsolete("GetCurrentCount()를 사용해주세요.")]
        public int Activated => GetCurrentCount();

        private GameObject obj;

        public int GetCurrentCount()
        {
            foreach (Mosquito mosquito in mosquitoes)
            {
                if (mosquito.Active) activated++;
            }
            return activated;
        }

        public MosquitoPool(GameObject obj, int capacity)
        {
            this.obj = obj;
            mosquitoes = new(capacity);
            for (int i = 0; i < capacity; i++)
            {
                GameObject instance = Object.Instantiate(this.obj);
                if (instance.TryGetComponent(out Mosquito mosquito))
                {
                    mosquito.SetActive(false);
                    mosquitoes.Add(mosquito);
                }
            }
        }

        public Mosquito Get()
        {
            Mosquito newMosquito = null;

            foreach (Mosquito mosquito in mosquitoes)
            {
                if (!mosquito.Active)
                {
                    newMosquito = mosquito;
                    break;
                }
            }

            if (newMosquito == null)
            {
                GameObject instance = Object.Instantiate(obj);
                if (instance.TryGetComponent(out Mosquito mosquito))
                {
                    mosquito.SetActive(false);
                    mosquitoes.Add(mosquito);
                    newMosquito = mosquito;
                }
            }
            
            return newMosquito;
        }
    }
}