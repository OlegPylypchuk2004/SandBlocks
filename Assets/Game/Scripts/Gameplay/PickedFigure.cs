using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PickedFigure : MonoBehaviour
    {
        [SerializeField] private Block[] _blocks;

        public bool IsCanPutBlocks()
        {
            foreach (Cell cellUnderBlock in GetCellsUnderBlocks())
            {
                if (cellUnderBlock == null || cellUnderBlock.IsFilled)
                {
                    return false;
                }
            }

            return true;
        }

        public Cell[] GetCellsUnderBlocks()
        {
            List<Cell> cells = new List<Cell>();

            foreach (Block block in _blocks)
            {
                Cell cellUnder = block.GetCellUnder();

                if (cellUnder != null)
                {
                    cells.Add(cellUnder);
                }
            }

            return cells.ToArray();
        }
    }
}