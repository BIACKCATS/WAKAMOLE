using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.UI.Play
{
    public class ChargeHand : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;

        private Vector2 initPosition = Vector2.zero, targetPosition = Vector2.zero, randomPosition = Vector3.zero;
        private Vector2 fixedPosition = Vector2.zero;

        public void SetCharged(bool charge)
        {
            gameObject.SetActive(charge);
            if (!charge) rect.position = fixedPosition = initPosition;
        }

        private void Awake()
        {
            fixedPosition = initPosition = rect.position;
        }

        private void Update()
        {
            if (StageManager.Current == null || !StageManager.Current.Active) return;

            targetPosition = Mouse.current.position.ReadValue();
            targetPosition /= 30.0f;

            randomPosition.x = Random.Range(-15.0f, 15.0f);
            randomPosition.y = Random.Range(-15.0f, 15.0f);
            fixedPosition = initPosition + randomPosition;

            rect.position = Vector2.Lerp(rect.position, fixedPosition + targetPosition, 15.0f * Time.deltaTime);
        }
    }
}