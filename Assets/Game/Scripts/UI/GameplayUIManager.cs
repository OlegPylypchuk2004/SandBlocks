using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PausePopup _pausePopup;

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
            _pausePopup.Appear();
        }
    }
}