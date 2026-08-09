using UnityEngine;
using UnityEngine.SceneManagement;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI.Shop
{
    public class StartStage : MonoBehaviour
    {
        public void LoadStage(string sceneName)
        {
            GameManager.Current.StageId++;
            SceneManager.LoadScene(sceneName);
        }
    }
}