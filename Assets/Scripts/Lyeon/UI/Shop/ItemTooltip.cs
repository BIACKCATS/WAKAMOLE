using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Wakamole.Lyeon.UI.Shop
{
    public class ItemTooltip : MonoBehaviour
    {
        public static ItemTooltip Current { get; private set; }

        [SerializeField] private TMP_Text text;
        
        private Vector2 targetPosition;

        public string ItemDesc { get; set; }

        private void Awake()
        {
            Current = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            UpdatePosition();
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            targetPosition = Mouse.current.position.ReadValue();
            targetPosition.x += 60.0f;
            targetPosition.y -= 60.0f;

            transform.position = targetPosition;
        }
    }
}