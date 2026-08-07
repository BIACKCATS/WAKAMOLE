using UnityEngine;
using Wakamole.Core.LocalData;

namespace Wakamole.Lyeon.Entity
{
    public class Backdrop : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Collider boxCollider;
        [SerializeField] private BackdropData data;

        private int hit = 0;
        public int Hp => data.objectFrame.Count - (hit + 1);

        public void Hit()
        {
            if (Hp > 0)
            {
                sprite.sprite = data.objectFrame[++hit];
                if (Hp == 0) boxCollider.enabled = false;
            }
            else sprite.sprite = data.objectFrame[data.objectFrame.Count - 1];
        }
    }
}