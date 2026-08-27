using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wakamole.Lyeon.UI
{
    public class SceneButton : MonoBehaviour
    {
        public void StartScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void FinishGame()
        {
            Application.Quit();
        }
    }
}