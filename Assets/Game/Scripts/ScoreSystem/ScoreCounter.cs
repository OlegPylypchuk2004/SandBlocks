using Gameplay;
using System;

namespace ScoreSystem
{
    public class ScoreCounter : IDisposable
    {
        private CellsGrid _cellsGrid;
        private int _score;

        public event Action<int> ScoreChanged;

        public int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                _score = value;

                ScoreChanged?.Invoke(Score);
            }
        }

        public ScoreCounter(CellsGrid cellsGrid)
        {
            _cellsGrid = cellsGrid;
            _cellsGrid.BlocksDestroyed += OnBlocksDestroyed;
        }

        public void Dispose()
        {
            _cellsGrid.BlocksDestroyed -= OnBlocksDestroyed;
        }

        private void OnBlocksDestroyed(Block[] blocks)
        {
            Score += blocks.Length * 5;
        }
    }
}