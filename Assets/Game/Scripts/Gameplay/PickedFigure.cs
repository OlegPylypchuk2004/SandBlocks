using UnityEngine;

namespace Gameplay
{
    public class PickedFigure : MonoBehaviour
    {
        [SerializeField] private Block[] _blocks;

        public bool IsCanPutBlocks()
        {
            foreach (Block block in _blocks)
            {
                Cell cellUnder = block.GetCellUnder();

                if (cellUnder == null || cellUnder.IsFilled)
                {
                    return false;
                }
            }

            return true;
        }
    }
}