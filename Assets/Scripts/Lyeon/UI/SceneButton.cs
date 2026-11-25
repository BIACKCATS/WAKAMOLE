using UnityEngine;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI
{
    public class SceneButton : MonoBehaviour
    {
        public void StartScene(string sceneName)
        {
            GameManager.Current.Audio.PlaySfx("Button");
            LoadingManager.Current.LoadScene(sceneName);
        }

        public void FinishGame()
        {
            GameManager.Current.Audio.PlaySfx("Button");
            Application.Quit();
        }
    }
}