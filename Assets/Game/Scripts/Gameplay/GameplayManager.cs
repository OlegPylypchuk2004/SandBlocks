using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private CellsGrid _cellsGrid;
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _pickupableFigureLayerMask;

        private PickupableFigure _pickedFigure;

        private void Start()
        {
            _cellsGrid.Generate(_gridSize);
            _camera.orthographicSize = Mathf.Max(_gridSize.x, _gridSize.y);
        }

        private void Update()
        {
            if (_pickedFigure == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray2D ray = new Ray2D(GetMouseWorldPosition(), Vector2.zero);
                    RaycastHit2D raycastHit = Physics2D.Raycast(ray.origin, ray.direction, 1f, _pickupableFigureLayerMask);

                    if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out PickupableFigure pickupableFigure))
                    {
                        _pickedFigure = pickupableFigure;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    MovePickedFigure();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_pickedFigure.IsCanPutBlocks())
                    {
                        Cell[] cellsUnderBlocks = _pickedFigure.GetCellsUnderBlocks();

                        if (cellsUnderBlocks.Length == 0)
                        {
                            return;
                        }

                        foreach (Cell cell in cellsUnderBlocks)
                        {
                            cell.IsFilled = true;
                        }

                        _cellsGrid.Simulate(cellsUnderBlocks);
                    }

                    Destroy(_pickedFigure.gameObject);
                    _pickedFigure = null;
                }
            }
        }

        private void MovePickedFigure()
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();

            if (_pickedFigure.IsCanPutBlocks())
            {
                mouseWorldPosition.x = Mathf.Round(mouseWorldPosition.x);
                mouseWorldPosition.y = Mathf.Round(mouseWorldPosition.y);
            }

            mouseWorldPosition.z = _pickedFigure.transform.position.z;

            _pickedFigure.transform.position = mouseWorldPosition;
        }

        private Vector3 GetMouseWorldPosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}