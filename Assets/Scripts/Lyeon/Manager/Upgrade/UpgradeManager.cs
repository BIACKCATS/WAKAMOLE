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
        [SerializeField] private Canvas currentCanvas, shopCanvas;
        
        private readonly WaitForSeconds _wait = new(0.25f);

        private void OnEnable()
        {
            StartCoroutine(InitCards());
        }

        // 위치 초기화가 제대로 되지 않는 현상 방지
        private IEnumerator InitCards()
        {
            yield return null;
            foreach (UpgradeCard card in cards)
            {
                MoleData data = moles[Random.Range(0, moles.Count)];
                card.Init(data.keyword, data.moleName, data.moleDesc);
            }
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            foreach (UpgradeCard card in cards)
            {
                card.ShowCard();
                yield return _wait;
            }
        }

        public void SelectCard(UpgradeCard upgradeCard)
        {
            StartCoroutine(ShowShop());
            foreach (UpgradeCard card in cards)
            {
                if (card != upgradeCard) card.HideCard();
                else continue;
            }
        }

        private IEnumerator ShowShop()
        {
            yield return _wait;
            currentCanvas.gameObject.SetActive(false);
            shopCanvas.gameObject.SetActive(true);
        }
    }
}