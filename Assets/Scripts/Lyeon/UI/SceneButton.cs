using UnityEngine;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI
{
    public class SceneButton : MonoBehaviour
    {
        public void StartScene(string sceneName)
        {
            LoadingManager.Current.LoadScene(sceneName);
        }

        public void FinishGame()
        {
            Application.Quit();
        }
    }
}