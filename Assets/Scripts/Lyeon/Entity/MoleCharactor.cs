using UnityEngine;
using Wakamole.Lyeon.Player;

namespace Wakamole.Lyeon.Entity
{
    public class MoleCharactor : MonoBehaviour
    {
        [SerializeField] private Mole mole;
        [SerializeField] private float moveSpeed = 8.0f;

        private bool active = false;
        private Vector3 initPosition = Vector3.zero;
        private Vector3 targetPosition = new(0, 0.1f, 0);

        public bool Active => active;

        private void Awake()
        {
            initPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            active = false;
            targetPosition = new(0, 0.1f, 0);
        }

        private void FixedUpdate()
        {
            if (targetPosition == initPosition && Vector3.Distance(transform.localPosition, targetPosition) < 0.01f) 
                transform.parent.gameObject.SetActive(false);
            if (!active && Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
                active = true;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.fixedDeltaTime * moveSpeed);
        }

        private void OnMouseDown()
        {
            mole.Hp -= PlayerManager.Current.Atk;
            if (mole.Hp <= 0)
            {
                targetPosition = initPosition;
                active = false;
            }
        }
    }
}