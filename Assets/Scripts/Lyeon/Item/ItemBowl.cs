using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.Item
{
    public class ItemBowl : IAttackItem
    {
        public void Disable()
        {
            
        }

        public void Enable()
        {
            
        }

        public void OnAttack(IEntity entity)
        {
            if (entity is Mole) StageManager.Current.Score += GameManager.Current.Preference.HitScore;
        }
    }
}