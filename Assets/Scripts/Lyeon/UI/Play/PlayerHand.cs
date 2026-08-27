using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.UI.Play
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;

        private Vector2 initPosition = Vector2.zero, targetPosition = Vector2.zero;

        private void Awake()
        {
            initPosition = rect.position;
        }

        private void Update()
        {
            if (StageManager.Current == null || !StageManager.Current.Active) return;

            targetPosition = Mouse.current.position.ReadValue();
            targetPosition /= 30.0f;

            rect.position = Vector2.Lerp(rect.position, initPosition + targetPosition, 15.0f * Time.deltaTime);
        }
    }
}