using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.UI.Play
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;

        private Vector2 initPosition = Vector2.zero, targetPosition = Vector2.zero, randomPosition = Vector3.zero;
        private Vector2 fixedPosition = Vector2.zero;

        private float charged = 0;

        public float Charged
        {
            get => charged;
            set
            {
                charged = value;
                
                if (charged < 0) charged = 0;
                else if (charged > 1) charged = 1;

                if (charged == 0) fixedPosition = initPosition;
            }
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

            if (charged > 0)
            {
                randomPosition.x = Random.Range(-charged * 10.0f, charged * 10.0f);
                randomPosition.y = Random.Range(-charged * 10.0f, charged * 10.0f);
                fixedPosition = Vector2.Lerp(rect.position, initPosition + randomPosition, 20.0f * Time.deltaTime);
            }

            rect.position = Vector2.Lerp(rect.position, fixedPosition + targetPosition, 15.0f * Time.deltaTime);
        }
    }
}