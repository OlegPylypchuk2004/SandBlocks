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

        public void Simulate(Cell[] cells)
        {
            if (_simulationCoroutine != null)
            {
                StopCoroutine(_simulationCoroutine);
                _simulationCoroutine = null;
            }

            _simulationCoroutine = StartCoroutine(SimulationCoroutine(cells));
        }

        private IEnumerator SimulationCoroutine(Cell[] cells)
        {
            cells = cells.OrderBy(cell => cell.transform.position.y)
                .ToArray();

            bool isCompleted = false;

            do
            {
                isCompleted = true;

                foreach (Cell cell in cells)
                {
                    Cell neighboringCell = GetNeighboringCell(cell);

                    if (neighboringCell == null)
                    {
                        continue;
                    }

                    if (neighboringCell.IsFilled)
                    {
                        continue;
                    }

                    isCompleted = false;
                    cell.IsFilled = false;
                    neighboringCell.IsFilled = true;
                }

                yield return new WaitForSeconds(_simulationDelay);
            }
            while (!isCompleted);
        }

        private Cell GetNeighboringCell(Cell cell)
        {
            Vector2Int coordinates = GetCellCoordinates(cell);

            if (coordinates.y + 1 < _cells.GetLength(0))
            {
                return _cells[coordinates.y + 1, coordinates.x];
            }

            return null;
        }

        private Vector2Int GetCellCoordinates(Cell cell)
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

            return new Vector2Int(-1, -1);
        }
    }
}