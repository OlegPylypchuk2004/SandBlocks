using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private CellsGrid _cellsGrid;
        [SerializeField] private Camera _camera;
        [SerializeField] private PickedFigure _pickedFigure;

        private void Start()
        {
            _cellsGrid.Generate(_gridSize);
            _camera.orthographicSize = Mathf.Max(_gridSize.x, _gridSize.y);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MovePickedFigure();
                _pickedFigure.gameObject.SetActive(_pickedFigure.IsCanPutBlocks() && Input.GetMouseButton(0));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_pickedFigure.IsCanPutBlocks())
                {
                    foreach (Cell cell in _pickedFigure.GetCellsUnderBlocks())
                    {
                        cell.IsFilled = true;
                    }
                }
            }
        }

        private void MovePickedFigure()
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            mouseWorldPosition.x = Mathf.Round(mouseWorldPosition.x);
            mouseWorldPosition.y = Mathf.Round(mouseWorldPosition.y);
            mouseWorldPosition.z = _pickedFigure.transform.position.z;

            _pickedFigure.transform.position = mouseWorldPosition;
        }

        private Vector3 GetMouseWorldPosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}