using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.Item
{
    public interface IItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Cost { get; }
        public void Use(PlayerManager player);
    }
}