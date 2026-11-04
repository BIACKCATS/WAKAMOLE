using UnityEngine;
using Wakamole.Lyeon.Entity;

namespace Wakamole.Lyeon.Item
{
    public class ItemBroom : IAttackItem
    {
        public void Disable()
        {
            
        }

        public void Enable()
        {
            
        }

        public void OnAttack(IEntity entity)
        {
            Debug.Log(ToString() + " Attack check " + entity.ToString());
        }
    }
}