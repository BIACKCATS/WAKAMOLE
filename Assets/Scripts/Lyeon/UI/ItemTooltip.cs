using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private Vector2 targetPosition;

        public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
        public ItemData Item
        {
            set => text.text = $"{value.itemName}\n\n{value.itemFunc}\n\n<i><color=\"grey\"><size=28>{value.itemDesc}</size></color></i>";
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            SetTargetPosition();
            transform.position = targetPosition;
        }

        private void Update()
        {
            SetTargetPosition();
            transform.position = Vector2.Lerp(transform.position, targetPosition, 50.0f * Time.deltaTime);
        }

        private void SetTargetPosition()
        {
            targetPosition = Mouse.current.position.ReadValue();
            targetPosition.x += 60.0f;
            targetPosition.y -= 60.0f;
        }
    }
}