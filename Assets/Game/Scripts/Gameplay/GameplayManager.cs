using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private CellsGrid _cellsGrid;
        [SerializeField] private Camera _camera;

        private void Start()
        {
            _cellsGrid.Generate(_gridSize);
            _camera.orthographicSize = Mathf.Max(_gridSize.x, _gridSize.y);
        }
    }
}