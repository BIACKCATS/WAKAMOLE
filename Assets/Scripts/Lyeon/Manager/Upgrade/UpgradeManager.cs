using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.UI.Upgrade;

namespace Wakamole.Lyeon.Manager.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private List<UpgradeCard> cards;
        [SerializeField] private List<MoleData> moles;
        
        private readonly WaitForSeconds _wait = new(0.25f);

        private void OnEnable()
        {
            foreach (UpgradeCard card in cards)
            {
                MoleData data = moles[Random.Range(0, moles.Count)];
                card.Init(data.keyword, data.moleName, data.moleDesc);
            }
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            foreach (UpgradeCard card in cards)
            {
                card.ShowCard();
                yield return _wait;
            }
        }

        public void SelectCard(UpgradeCard upgradeCard)
        {
            foreach (UpgradeCard card in cards)
            {
                if (card != upgradeCard) card.HideCard();
                else continue;
            }
        }
    }
}