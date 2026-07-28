using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wakamole.Lyeon.Manager
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Current { get; private set; }
        
        [SerializeField] private CanvasGroup canvasGroup;
        
        /// <summary>
        /// Scene을 로드합니다.
        /// </summary>
        /// <param name="scene">이동할 Scene입니다.</param>
        public void LoadScene(Scene scene) { LoadScene(scene.name); }

        /// <summary>
        /// Scene을 로드합니다.
        /// </summary>
        /// <param name="sceneName">이동할 Scene의 이름입니다.</param>
        public void LoadScene(string sceneName)
        {
            gameObject.SetActive(true);
            StartCoroutine(Load(sceneName));
        }

        private IEnumerator Load(string sceneName) {
            
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (!async.isDone) {
                yield return null;
                if (async.progress >= 0.9f) {
                    async.allowSceneActivation = true;

                    yield break;
                }
            }
        }

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(this);
                return;
            }

            Current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}