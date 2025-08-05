using ScoreSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay
{
    public class GameplaySceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private CellsGrid _cellsGrid;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cellsGrid);
            builder.Register<ScoreCounter>(Lifetime.Singleton);
        }
    }
}