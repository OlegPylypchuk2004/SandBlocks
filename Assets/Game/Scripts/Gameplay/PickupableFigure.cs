using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PickupableFigure : MonoBehaviour
    {
        [SerializeField] private Block[] _blocks;

        private Color _color;

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;

                foreach (Block block in _blocks)
                {
                    block.ApplyColor(_color);
                }
            }
        }

        public bool IsCanPutBlocks()
        {
            Cell[] cellsUnderBlocks = GetCellsUnderBlocks();

            if (cellsUnderBlocks.Length == 0 || cellsUnderBlocks.Length < _blocks.Length)
            {
                return false;
            }

            foreach (Cell cellUnderBlock in cellsUnderBlocks)
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