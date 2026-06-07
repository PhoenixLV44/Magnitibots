using UnityEngine;

namespace Ability.Object
{
    public class TargetCursor : MonoBehaviour
    {
        [SerializeField] private int cursorSpeed = 3;
        public int CursorSpeed => cursorSpeed;
    }
}
