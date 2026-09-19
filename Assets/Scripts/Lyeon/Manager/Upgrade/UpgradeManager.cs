using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wakamole.Lyeon.UI.Upgrade;

namespace Wakamole.Lyeon.Manager.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private List<UpgradeCard> cards;
        
        private readonly WaitForSeconds _wait = new(0.25f);

        private void OnEnable()
        {
            foreach (UpgradeCard card in cards)
                card.Init();
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