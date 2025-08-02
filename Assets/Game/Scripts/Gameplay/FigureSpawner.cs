using UnityEngine;

namespace Gameplay
{
    public class FigureSpawner : MonoBehaviour
    {
        [SerializeField] private PickupableFigure[] _figurePrefabs;
        [SerializeField] private FigureSpawnPoint[] _spawnPoints;
        [SerializeField] private GameplayManager _gameplayManager;

        private void Start()
        {
            SpawnFigures();

            _gameplayManager.FigureWasPicked += OnFigureWasPicked;
            _gameplayManager.FigureWasPlaced += OnFigureWasPlaced;
            _gameplayManager.FigureWasDropped += OnFugureWasDropped;
        }

        private void OnDestroy()
        {
            _gameplayManager.FigureWasPicked -= OnFigureWasPicked;
            _gameplayManager.FigureWasPlaced -= OnFigureWasPlaced;
            _gameplayManager.FigureWasDropped -= OnFugureWasDropped;
        }

        private void SpawnFigures()
        {
            foreach (FigureSpawnPoint spawnPoint in _spawnPoints)
            {
                if (spawnPoint.Figure != null)
                {
                    continue;
                }

                PickupableFigure figure = Instantiate(_figurePrefabs[Random.Range(0, _figurePrefabs.Length)]);
                spawnPoint.Figure = figure;
            }
        }

        private void OnFigureWasPicked(PickupableFigure figure)
        {
            foreach (FigureSpawnPoint spawnPoint in _spawnPoints)
            {
                if (spawnPoint.Figure == figure)
                {
                    spawnPoint.Figure = null;

                    return;
                }
            }
        }

        private void OnFigureWasPlaced(PickupableFigure figure)
        {
            foreach (FigureSpawnPoint spawnPoint in _spawnPoints)
            {
                if (spawnPoint.Figure != null)
                {
                    return;
                }
            }

            SpawnFigures();
        }

        private void OnFugureWasDropped(PickupableFigure figure)
        {
            foreach (FigureSpawnPoint spawnPoint in _spawnPoints)
            {
                if (spawnPoint.PreviousFigure == figure)
                {
                    spawnPoint.Figure = figure;

                    return;
                }
            }
        }
    }
}