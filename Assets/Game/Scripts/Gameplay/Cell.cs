using UnityEngine;

namespace Gameplay
{
    public class Cell : MonoBehaviour
    {
        private Block _block;

        public Vector2Int Coordinates { get; set; }

        public Block Block
        {
            get
            {
                return _block;
            }
            set
            {
                _block = value;
            }
        }

        public bool IsFilled
        {
            get
            {
                return _block != null;
            }
        }
    }
}