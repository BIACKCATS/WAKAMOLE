using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wakamole.Lyeon.UI.Play
{
    public class ChargeFire : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private List<Sprite> fires;

        private int frame = 0;
        private float time = 0;
        private Color color;

        public float Alpha
        {
            get => image.color.a;
            set
            {
                color = image.color;
                color.a = value;
                image.color = color;
            }
        }

        private void OnEnable()
        {
            Alpha = 0;
            time = 0;
            image.sprite = fires[0];
        }

        private void Update()
        {
            if (Alpha < 0.99f) Alpha = Mathf.Lerp(Alpha, 1.0f, 15.0f * Time.deltaTime);
            else if (Alpha != 1) Alpha = 1;

            if (time <= 0.1f)
            {
                time += Time.deltaTime;
                return;
            }
            else time = 0;

            image.sprite = fires[frame++];
            if (frame >= fires.Count) frame = 0;
        }
    }
}