using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wakamole.Lyeon.Manager.Game
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Current { get; private set; }
        
        [SerializeField] private Animator animator;

        private WaitForSeconds _wait = new(0.5f);
        
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
            yield return _wait;

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (!async.isDone) {
                yield return null;
                if (async.progress == 1.0f)
                {
                    animator.SetTrigger(Animator.StringToHash("Finish"));
                    yield return _wait;
                    gameObject.SetActive(false);
                    yield break;
                }
                else if (async.progress >= 0.9f) async.allowSceneActivation = true;
            }
        }

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
            DontDestroyOnLoad(gameObject);

            gameObject.SetActive(false);
        }
    }
}