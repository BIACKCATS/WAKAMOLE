using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.Entity
{
    public class Backdrop : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Collider boxCollider;
        [SerializeField] private BackdropData data;
        [SerializeField] private int vibrateCount = 6;

        private int hit = 0;
        public int Hp => data.objectFrame.Count - (hit + 1);

        private bool vibrate = false;
        private int currentVibrate = 0;
        private Vector3 initPosition = Vector3.zero, targetPosition = Vector3.zero;

        private void Awake()
        {
            targetPosition = initPosition = transform.position;
        }

        private void Update()
        {
            if (vibrate && Vector3.Distance(targetPosition, transform.position) < 0.01f)
            {
                if (vibrateCount >= ++currentVibrate)
                {
                    if (currentVibrate % 2 == 0) targetPosition = initPosition + (Vector3.right * 0.05f);
                    else targetPosition = initPosition - (Vector3.right * 0.05f);
                }
                else
                {
                    targetPosition = initPosition;
                    vibrate = false;
                    currentVibrate = 0;
                }
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, 50.0f * Time.deltaTime);
        }

        public void Hit()
        {
            if (Hp > 0)
            {
                sprite.sprite = data.objectFrame[++hit];
                vibrate = true;
                if (Hp == 0) boxCollider.enabled = false;
            }
            else sprite.sprite = data.objectFrame[data.objectFrame.Count - 1];
        }
    }
}