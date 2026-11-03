using Wakamole.Lyeon.Entity;

namespace Wakamole.Lyeon.Item
{
    public interface IItem
    {
        public void Enable();
        public void Disable();
    }

    public interface IAttackItem : IItem
    {
        public void OnAttack(IEntity entity);
    }
}