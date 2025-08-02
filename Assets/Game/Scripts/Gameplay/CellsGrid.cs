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
            List<Cell> simulationCells = cells.OrderBy(cell => cell.transform.position.y).ToList();

            while (simulationCells.Count > 0)
            {
                List<Cell> nextStepCells = new List<Cell>();

                foreach (Cell cell in simulationCells)
                {
                    Cell neighbor = GetNeighboringCell(cell);

                    if (neighbor == null || neighbor.IsFilled)
                    {
                        continue;
                    }

                    cell.IsFilled = false;
                    neighbor.IsFilled = true;

                    nextStepCells.Add(neighbor);
                }

                if (nextStepCells.Count == 0)
                {
                    break;
                }

                simulationCells = nextStepCells;

                yield return new WaitForSeconds(_simulationDelay);
            }
        }

        private Cell GetNeighboringCell(Cell cell)
        {
            Vector2Int? coordinates = GetCellCoordinates(cell);

            if (!coordinates.HasValue)
            {
                return null;
            }

            Vector2Int coords = coordinates.Value;

            if (coords.y + 1 < _cells.GetLength(0))
            {
                return _cells[coords.y + 1, coords.x];
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