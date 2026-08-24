using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI.Shop
{
    public class NextRound : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite clickImage;
        [SerializeField] private string sceneName;

        public void OnPointerDown(PointerEventData eventData)
        {
            image.sprite = clickImage;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameManager.Current.StageId++;
            SceneManager.LoadScene(sceneName);
        }
    }
}