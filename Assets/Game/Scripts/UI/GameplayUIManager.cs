using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        private void Start()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {

        }
    }
}