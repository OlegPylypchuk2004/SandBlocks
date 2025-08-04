using CustomLayoutGroup;
using System.Collections;
using System.Collections.Generic;
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

        public bool IsSimulationStarted
        {
            get
            {
                return _simulationCoroutine != null;
            }
        }

        public void Generate(Vector2Int size)
        {
            _cells = new Cell[size.y, size.x];

            for (int y = 0; y < _cells.GetLength(0); y++)
            {
                for (int x = 0; x < _cells.GetLength(1); x++)
                {
                    Cell cell = Instantiate(_cellPrefab, _layoutGroup.transform);
                    cell.Coordinates = new Vector2Int(x, y);

                    _cells[y, x] = cell;
                }
            }

            _layoutGroup.Columns = size.x;
            _layoutGroup.UpdateLayout();
        }

        public bool TrySimulate(Block[] blocks)
        {
            if (IsSimulationStarted)
            {
                return false;
            }

            _simulationCoroutine = StartCoroutine(SimulationCoroutine(blocks));

            return true;
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

            _simulationCoroutine = null;

            DestroyBlocks();
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
            if (cell == null)
            {
                return null;
            }

            Vector2Int coordinates = cell.Coordinates;

            int rowsAmount = _cells.GetLength(0);
            int columnsAmount = _cells.GetLength(1);

            if (coordinates.y + 1 < rowsAmount)
            {
                Cell belowCell = _cells[coordinates.y + 1, coordinates.x];

                if (belowCell != null && !belowCell.IsFilled)
                {
                    return belowCell;
                }

                if (coordinates.x - 1 >= 0)
                {
                    Cell belowLeft = _cells[coordinates.y + 1, coordinates.x - 1];
                    Cell upCell = _cells[coordinates.y, coordinates.x - 1];

                    if (belowLeft != null && !belowLeft.IsFilled)
                    {
                        if (upCell == null || !upCell.IsFilled)
                        {
                            return belowLeft;
                        }
                    }
                }

                if (coordinates.x + 1 < columnsAmount)
                {
                    Cell belowRight = _cells[coordinates.y + 1, coordinates.x + 1];
                    Cell upCell = _cells[coordinates.y, coordinates.x + 1];

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

        private void DestroyBlocks()
        {
            int rows = _cells.GetLength(0);
            int columns = _cells.GetLength(1);
            bool[,] visited = new bool[rows, columns];
            bool isCellsDestroyed = false;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (visited[y, x]) continue;

                    Block startBlock = _cells[y, x].Block;

                    if (startBlock == null)
                    {
                        continue;
                    }

                    List<Vector2Int> group = new List<Vector2Int>();
                    HashSet<int> touchedColumns = new HashSet<int>();

                    FloodFill(y, x, startBlock.Color, visited, group, touchedColumns);

                    if (touchedColumns.Contains(0) && touchedColumns.Contains(columns - 1))
                    {
                        foreach (var pos in group)
                        {
                            Block block = _cells[pos.y, pos.x].Block;
                            if (block != null)
                            {
                                Destroy(block.gameObject);
                                _cells[pos.y, pos.x].Block = null;

                                isCellsDestroyed = true;
                            }
                        }
                    }
                }
            }

            if (isCellsDestroyed)
            {
                //TrySimulate();
            }
        }

        private void FloodFill(int y, int x, Color targetColor, bool[,] visited, List<Vector2Int> group, HashSet<int> touchedColumns)
        {
            int rows = _cells.GetLength(0);
            int columns = _cells.GetLength(1);

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(x, y));

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                int currentX = current.x;
                int currentY = current.y;

                if (currentX < 0 || currentX >= columns || currentY < 0 || currentY >= rows)
                {
                    continue;
                }

                if (visited[currentY, currentX])
                {
                    continue;
                }

                Block block = _cells[currentY, currentX].Block;

                if (block == null || block.Color != targetColor)
                {
                    continue;
                }

                visited[currentY, currentX] = true;
                group.Add(new Vector2Int(currentX, currentY));
                touchedColumns.Add(currentX);

                queue.Enqueue(new Vector2Int(currentX + 1, currentY));
                queue.Enqueue(new Vector2Int(currentX - 1, currentY));
                queue.Enqueue(new Vector2Int(currentX, currentY + 1));
                queue.Enqueue(new Vector2Int(currentX, currentY - 1));
            }
        }
    }
}