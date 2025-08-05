using ScoreSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay
{
    public class GameplaySceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private CellsGrid _cellsGridPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_cellsGridPrefab, Lifetime.Singleton);
            builder.Register<ScoreCounter>(Lifetime.Singleton);
        }
    }
}