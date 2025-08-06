using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PickupableFigure : MonoBehaviour
    {
        [SerializeField] private Block[] _blocks;

        [field: SerializeField] public float MaxXPosition { get; private set; }
        [field: SerializeField] public float MaxYPosition { get; private set; }

        private Color[] _colors;

        public Block[] Blocks => _blocks;

        public Color[] Colors
        {
            get
            {
                return _colors;
            }
            set
            {
                _colors = value;

                foreach (Block block in _blocks)
                {
                    int colorIndex = Mathf.Clamp(block.ColorIndex, 0, _colors.Length - 1);
                    block.Color = _colors[colorIndex];
                }
            }
        }

        public void Pickup()
        {
            foreach (Block block in _blocks)
            {
                block.SpriteOrderInLayer = 100;
            }
        }

        public void Drop()
        {
            foreach (Block block in _blocks)
            {
                block.SpriteOrderInLayer = 0;
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