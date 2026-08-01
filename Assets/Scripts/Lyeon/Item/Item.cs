using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.Item
{
    public interface IItem
    {
        public string Name { get; }
        public int Cost { get; }
        public void Use(PlayerManager player);
    }
}