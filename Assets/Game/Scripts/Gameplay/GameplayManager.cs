using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private CellsGrid _cellsGrid;
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _pickupableFigureLayerMask;

        private HashSet<Block> _blocks;
        private PickupableFigure _pickedFigure;

        public event Action<PickupableFigure> FigureWasPicked;
        public event Action<PickupableFigure> FigureWasPlaced;
        public event Action<PickupableFigure> FigureWasDropped;

        private void Start()
        {
            Application.targetFrameRate = 60;

            _blocks = new HashSet<Block>();

            _cellsGrid.Generate(_gridSize);
            _cellsGrid.BlocksDestroyed += OnBlocksDestroyed;

            _camera.orthographicSize = Mathf.Min(_gridSize.x, _gridSize.y);
        }

        private void OnDestroy()
        {
            _cellsGrid.BlocksDestroyed -= OnBlocksDestroyed;
        }

        private void Update()
        {
            if (_pickedFigure == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PickupFigure();
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
                    PutBlocks();
                }
            }
        }

        private void PickupFigure()
        {
            Ray2D ray = new Ray2D(GetMouseWorldPosition(), Vector2.zero);
            RaycastHit2D raycastHit = Physics2D.Raycast(ray.origin, ray.direction, 1f, _pickupableFigureLayerMask);

            if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out PickupableFigure pickupableFigure))
            {
                _pickedFigure = pickupableFigure;
                _pickedFigure.transform.SetParent(null);

                FigureWasPicked?.Invoke(_pickedFigure);
            }
        }

        private void MovePickedFigure()
        {
            Vector3 figureTargetPosition = GetMouseWorldPosition();
            figureTargetPosition.x = Mathf.Round(figureTargetPosition.x);
            figureTargetPosition.y = Mathf.Round(figureTargetPosition.y);
            figureTargetPosition.z = _pickedFigure.transform.position.z;

            _pickedFigure.transform.position = figureTargetPosition;
        }

        private void PutBlocks()
        {
            if (_cellsGrid.IsSimulationStarted)
            {
                FigureWasDropped?.Invoke(_pickedFigure);

                _pickedFigure = null;

                return;
            }

            if (_pickedFigure.IsCanPutBlocks())
            {
                Cell[] cellsUnderBlocks = _pickedFigure.GetCellsUnderBlocks();

                if (cellsUnderBlocks.Length == 0)
                {
                    return;
                }

                for (int i = 0; i < cellsUnderBlocks.Length; i++)
                {
                    cellsUnderBlocks[i].Block = _pickedFigure.Blocks[i];
                }

                foreach (Block block in _pickedFigure.Blocks)
                {
                    _blocks.Add(block);
                }

                _cellsGrid.Simulate(_pickedFigure.Blocks);

                foreach (Block block in _pickedFigure.Blocks)
                {
                    block.transform.SetParent(null);
                }

                FigureWasPlaced?.Invoke(_pickedFigure);

                Destroy(_pickedFigure.gameObject);
                _pickedFigure = null;
            }
            else
            {
                FigureWasDropped?.Invoke(_pickedFigure);

                _pickedFigure = null;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnBlocksDestroyed(Block[] blocks)
        {
            foreach (Block block in blocks)
            {
                _blocks.Remove(block);
            }

            _cellsGrid.Simulate(_blocks.ToArray());
        }
    }
}