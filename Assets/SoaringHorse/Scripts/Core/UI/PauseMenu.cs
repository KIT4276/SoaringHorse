using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _reduceSpeedButton;
    [SerializeField] private Button _startAgainButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(Exit);
        _reduceSpeedButton.onClick.AddListener(ShowRewarded);
        _startAgainButton.onClick.AddListener(StartAgain);
    }

    private void OnDestruy()
    {
        _exitButton.onClick.RemoveListener(Exit);
        _reduceSpeedButton.onClick.RemoveListener(ShowRewarded);
        _startAgainButton.onClick.RemoveListener(StartAgain);
    }

    private void StartAgain()
    {
        //TODO
    }

    private void ShowRewarded()
    {
        //TODO
    }

    private void Exit()
    {
       //TODO 
        Application.Quit();
    }
}
