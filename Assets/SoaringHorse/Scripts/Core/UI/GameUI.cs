using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour, IInitializable, ILateDisposable
{
    [SerializeField] private GameUIAnimator _gameUIAnimator;
    [SerializeField] private TMP_Text _life;
    [SerializeField] private TMP_Text _exp;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _playPanel;
    [Space]
    [Header("Pause")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _reduceSpeedButton;
    [SerializeField] private Button _startAgainButton;

    private PauseMenu _pauseMenu;
    private LiveSystem _liveSystem;
    private ExperienceSystem _experienceSystem;
    private ScoreSystem _scoreSystem;
    private IPauseService _pauseService;
    private InputManager _inputManager;

    [Inject]
    public void Construct(LiveSystem liveSystem, ExperienceSystem experienceSystem, ScoreSystem scoreSystem,
        IPauseService pauseService, InputManager inputManager, StartMenuController startMenuController, PauseMenu pauseMenu)
    {
        _liveSystem = liveSystem;
        _experienceSystem = experienceSystem;
        _scoreSystem = scoreSystem;
        _pauseService = pauseService;
        _inputManager = inputManager;
        _pauseMenu = pauseMenu;
    }

    public void Initialize()
    {
        _liveSystem.ValueIncreased += OnLifeIncreased;
        _liveSystem.ValueDecreased += OnLifeDecreased;
        _experienceSystem.ChangeValue += OnExpChange;
        _scoreSystem.ChangeValue += OnScoreChange;
        _scoreSystem.ChangeIntegerValue += OnIntScoreChange;
        _inputManager.EscPressed += OnEscPressed;
        _pauseService.PauseRequested += OnPause;
        _pauseService.ResumeRequested += OnResume;

        _pausePanel.SetActive(false);
        _playPanel.SetActive(true);

        _exitButton.onClick.AddListener(Exit);
        _reduceSpeedButton.onClick.AddListener(ShowRewarded);
        _startAgainButton.onClick.AddListener(StartAgain);
    }

    public void LateDispose()
    {
        _liveSystem.ValueIncreased -= OnLifeIncreased;
        _liveSystem.ValueDecreased -= OnLifeDecreased;
        _experienceSystem.ChangeValue -= OnExpChange;
        _scoreSystem.ChangeValue -= OnScoreChange;
        _scoreSystem.ChangeIntegerValue -= OnIntScoreChange;
        _inputManager.EscPressed -= OnEscPressed;
        _pauseService.PauseRequested -= OnPause;
        _pauseService.ResumeRequested -= OnResume;

        _exitButton.onClick.RemoveListener(Exit);
        _reduceSpeedButton.onClick.RemoveListener(ShowRewarded);
        _startAgainButton.onClick.RemoveListener(StartAgain);
    }

    private void OnIntScoreChange()
    {
        _gameUIAnimator.ScoreImageJuiceBump();
    }

    private void OnLifeDecreased(int valueobj)
    {
        _gameUIAnimator.LifeImageJuiceBumpDecreased();
            OnLifeChange(valueobj);
    }

    private void OnLifeIncreased(int value)
    {
        _gameUIAnimator.LifeImageJuiceBumpIncrease();
        OnLifeChange(value);
    }

    private void StartAgain() =>
        _pauseMenu.StartAgain();

    private void ShowRewarded() =>
        _pauseMenu.ShowRewarded();

    private void Exit() =>
        _pauseMenu.Exit();

    private void OnResume()
    {
        _pausePanel.SetActive(false);
        _playPanel.SetActive(true);
    }

    private void OnPause()
    {
        _pausePanel.SetActive(true);
        _playPanel.SetActive(false);
    }

    public void OnEscPressed()
    {
        _pauseService.RequestPause();
    }

    private void OnLifeChange(int value) => 
        _life.text = value.ToString();

    private void OnExpChange(float value)
    {
        _exp.text = (value * 100).ToString("F0");
    }

    private void OnScoreChange(float value)
    {
        _score.text = value.ToString("F0");
    }
}
