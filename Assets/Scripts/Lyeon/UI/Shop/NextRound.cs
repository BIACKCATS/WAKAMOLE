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
            image.color = Color.gray;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            image.color = Color.white;
            GameManager.Current.Audio.PlaySfx("Button");
            GameManager.Current.StageId++;
            SceneManager.LoadScene(sceneName);
        }
    }
}