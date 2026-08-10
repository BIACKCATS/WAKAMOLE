using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.Manager.Play
{
    public class MosquitoPool
    {
        private List<Mosquito> mosquitoes;
        private int activated = 0;

        public int Activated
        {
            get
            {
                foreach (Mosquito mosquito in mosquitoes)
                {
                    if (mosquito.Active) activated++;
                }
                return activated;
            }
        }

        private GameObject obj;

        public MosquitoPool(GameObject obj, int capacity)
        {
            this.obj = obj;
            mosquitoes = new(capacity);
            for (int i = 0; i < capacity; i++)
            {
                GameObject instance = Object.Instantiate(this.obj);
                if (instance.TryGetComponent(out Mosquito mosquito))
                {
                    mosquito.Active = false;
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
                    mosquito.Active = false;
                    mosquitoes.Add(mosquito);
                    newMosquito = mosquito;
                }
            }
            
            return newMosquito;
        }
    }
}