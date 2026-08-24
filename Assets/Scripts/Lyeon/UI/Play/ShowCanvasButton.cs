using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class ShowCanvasButton : MonoBehaviour
    {
        [SerializeField] private Canvas parent;

        public void ShowCanvas(Canvas canvas)
        {
            parent.gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
        }
    }
}