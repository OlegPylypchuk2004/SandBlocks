using CustomLayoutGroup;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class CellsGrid : MonoBehaviour
    {
        [SerializeField] private float _simulationDelay;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private WorldSpaceGridLayoutGroup _layoutGroup;

        private Cell[,] _cells;
        private Coroutine _simulationCoroutine;

        public void Generate(Vector2Int size)
        {
            _cells = new Cell[size.y, size.x];

            for (int y = 0; y < _cells.GetLength(0); y++)
            {
                for (int x = 0; x < _cells.GetLength(1); x++)
                {
                    _cells[y, x] = Instantiate(_cellPrefab, _layoutGroup.transform);
                }
            }

            _layoutGroup.Columns = size.x;
            _layoutGroup.UpdateLayout();
        }

        public void Simulate(Block[] blocks)
        {
            if (_simulationCoroutine != null)
            {
                StopCoroutine(_simulationCoroutine);
                _simulationCoroutine = null;
            }

            _simulationCoroutine = StartCoroutine(SimulationCoroutine(blocks));
        }

        private IEnumerator SimulationCoroutine(Block[] blocks)
        {
            Block[] simulationBlocks = blocks.OrderBy(block => block.Position.y).ToArray();
            bool isCellMoved = false;

            do
            {
                isCellMoved = false;

                for (int i = 0; i < simulationBlocks.Length; i++)
                {
                    Block block = simulationBlocks[i];
                    Cell neighborCell = GetNeighboringCell(block.GetCellUnder());

                    if (TryMoveBlock(block, neighborCell))
                    {
                        isCellMoved = true;
                    }
                }

                yield return new WaitForSeconds(_simulationDelay);
            }
            while (isCellMoved);
        }

        private bool TryMoveBlock(Block block, Cell cell)
        {
            if (cell == null || cell.IsFilled)
            {
                return false;
            }

            block.GetCellUnder().Block = null;
            block.Position = cell.transform.position;
            cell.Block = block;

            return true;
        }

        private Cell GetNeighboringCell(Cell cell)
        {
            Vector2Int? coordinates = GetCellCoordinates(cell);

            if (!coordinates.HasValue)
            {
                return null;
            }

            Vector2Int coords = coordinates.Value;
            int rowsAmount = _cells.GetLength(0);
            int columnsAmount = _cells.GetLength(1);

            if (coords.y + 1 < rowsAmount)
            {
                Cell belowCell = _cells[coords.y + 1, coords.x];

                if (belowCell != null && !belowCell.IsFilled)
                {
                    return belowCell;
                }

                if (coords.x - 1 >= 0)
                {
                    Cell belowLeft = _cells[coords.y + 1, coords.x - 1];
                    Cell upCell = _cells[coords.y, coords.x - 1];

                    if (belowLeft != null && !belowLeft.IsFilled)
                    {
                        if (upCell == null || !upCell.IsFilled)
                        {
                            return belowLeft;
                        }
                    }
                }

                if (coords.x + 1 < columnsAmount)
                {
                    Cell belowRight = _cells[coords.y + 1, coords.x + 1];
                    Cell upCell = _cells[coords.y, coords.x + 1];

                    if (belowRight != null && !belowRight.IsFilled)
                    {
                        if (upCell == null || !upCell.IsFilled)
                        {
                            return belowRight;
                        }
                    }
                }
            }

            return null;
        }

        private Vector2Int? GetCellCoordinates(Cell cell)
        {
            for (int y = 0; y < _cells.GetLength(0); y++)
            {
                for (int x = 0; x < _cells.GetLength(1); x++)
                {
                    if (_cells[y, x] == cell)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return null;
        }
    }
}