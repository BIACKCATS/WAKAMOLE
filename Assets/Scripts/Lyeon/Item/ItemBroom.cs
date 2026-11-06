using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Play;

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
            if (entity is Backdrop && ((Backdrop)entity).Hp == 0)
                StageManager.Current.Score += GameManager.Current.Preference.BackdropScore;
        }
    }
}