using UnityEngine;

namespace Wakamole.Lyeon.Manager.Component
{
    public class CursorChange : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursor, specialCursor;
                
        /// <summary>
        /// 커서 이미지를 변경합니다.
        /// </summary>
        /// <param name="type">0: 가본 커서, 1: 타게팅 커서</param>
        public void SetCursor(int type)
        {
            switch (type)
            {
                case 0:
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case 1:
                    Cursor.SetCursor(specialCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }
}