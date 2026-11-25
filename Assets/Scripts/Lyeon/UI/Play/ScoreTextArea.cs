using UnityEngine;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.Manager.Shop;

namespace Wakamole.Lyeon.UI.Play
{
    public struct ScoreTextData
    {
        public Vector2 position;
        public int score;
    }

    public class ScoreTextArea : MonoBehaviour
    {
        [SerializeField] private ShopManager shopManager;
        [SerializeField] private ScoreTextUI scoreText;

        [Tooltip("텍스트가 이동하는 속도입니다.")]
        [SerializeField] private float speed = 2.0f;

        private ObjectPool objectPool;

        private void Awake()
        {
            objectPool = new ObjectPool(scoreText.gameObject, gameObject, 20);
        }

        public void Show(params ScoreTextData[] scores)
        {
            foreach (ScoreTextData score in scores)
            {
                GameObject obj = objectPool.Get();
                if (obj.TryGetComponent(out ScoreTextUI component))
                {
                    component.Speed = speed * scores.Length;
                    component.SetInitialPosition(score.position);
                    component.ShowScore(score.score);
                }
            }
        }
    }
}