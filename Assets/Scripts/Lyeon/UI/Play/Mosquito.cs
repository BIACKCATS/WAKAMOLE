using UnityEngine;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.UI.Play
{
    public class Mosquito : MonoBehaviour
    {
        private Vector3 targetPosition = Vector2.zero;

        public bool Active
        {
            get => gameObject.activeSelf;
            set
            {
                if (value)
                {
                    targetPosition = new(Random.Range(-5.0f, 5.0f), Random.Range(1.0f, 4.0f), Random.Range(-4.0f, 2.0f));
                    GameManager.Current.Audio.PlaySfx("Mosquitto");
                }
                gameObject.SetActive(value);
            }
        }

        private void OnEnable()
        {
            transform.position = new(Random.Range(-5.0f, 5.0f), Random.Range(2.0f, 3.0f), Random.Range(-3.0f, 3.0f));
        }

        private void Update()
        {
            if (!Active) return;

            transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) Active = true;
        }

        public void DestroyMosquito()
        {
            if (!Active) return;
            GameManager.Current.Audio.PlaySfx("Mosquitto_Hit");
            StageManager.Current.ActiveDoubleScore = true;
            Active = false;
        }
    }
}