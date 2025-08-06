using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class FigureSpawner : MonoBehaviour
    {
        [SerializeField] private PickupableFigure[] _figurePrefabs;
        [SerializeField] private FigureSpawnPoint[] _spawnPoints;
        [SerializeField] private GameplayManager _gameplayManager;

        private ColorConfig[] _colorConfigs;

        private void Start()
        {
            _colorConfigs = Resources.LoadAll<ColorConfig>("Configs/Colors");

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

                PickupableFigure figure = Instantiate(_figurePrefabs[Random.Range(0, _figurePrefabs.Length)], spawnPoint.transform);
                figure.Colors = GenerateFigureColors();
                spawnPoint.Figure = figure;
            }
        }

        private Color[] GenerateFigureColors()
        {
            int colorsAmount = Random.Range(1, 3);
            Color[] colors = new Color[colorsAmount];

            ColorConfig[] colorConfigs = _colorConfigs.OrderBy(colorConfig => Random.value)
                .Take(colorsAmount)
                .ToArray();

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = colorConfigs[i].Color;
            }

            return colors;
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