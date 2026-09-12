using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Vector2 mouseOffset = new(60.0f, 60.0f);

        private Vector2 targetPosition;

        public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
        public ItemData Item
        {
            set => text.text = $"{value.itemName}\n\n{value.itemFunc}\n\n<i><color=#cccccc><size=28>{value.itemDesc}</size></color></i>";
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            SetTargetPosition();
            rect.position = targetPosition;
        }

        private void Update()
        {
            SetTargetPosition();
            rect.position = Vector2.Lerp(rect.position, targetPosition, 50.0f * Time.deltaTime);
        }

        private void SetTargetPosition()
        {
            targetPosition = Mouse.current.position.ReadValue();
            targetPosition += mouseOffset;
        }
    }
}