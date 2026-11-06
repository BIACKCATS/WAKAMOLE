using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.Item
{
    public class ItemAlert : IItem
    {
        public void Enable()
        {
            StageManager.Current.StartAlert();
        }

        public void Disable()
        {
            StageManager.Current.StopAlert();
        }
    }
}