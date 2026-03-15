using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour, IInitializable, ILateDisposable
{
    [SerializeField] private GameUIAnimator _gameUIAnimator;

    [Header("HUD")]
    [SerializeField] private TMP_Text _life;
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _score;

    [Header("Root Panels")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _playPanel;

    [Header("Pause Menu")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _reduceSpeedButton;
    [SerializeField] private Button _startAgainButton;
    [SerializeField] private Button _rewardLifeButton;
    [SerializeField] private Button _escButton;
    [SerializeField] private TMP_Text _finalScore;

    [Header("Reward Panel")]
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private Button _rewardYesButton;
    [SerializeField] private Button _escapeToPauseButton;

    private LiveSystem _liveSystem;
    private ScoreSystem _scoreSystem;
    private IPauseService _pauseService;
    private InputManager _inputManager;
    private PauseMenu _pauseMenu;
    private IRewardedService _rewardedService;

    private PendingReward _pendingReward = PendingReward.None;

    private bool _resumeBlocked;
    private EnvironmentMove _environmentMove;

    private enum PendingReward
    {
        None,
        Life,
        ReduceSpeed
    }

    [Inject]
    public void Construct(
        LiveSystem liveSystem,
        ScoreSystem scoreSystem,
        IPauseService pauseService,
        InputManager inputManager,
        PauseMenu pauseMenu,
        IRewardedService rewardedService,
        EnvironmentMove environmentMove)
    {
        _environmentMove = environmentMove; ;
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
        _pauseService = pauseService;
        _inputManager = inputManager;
        _pauseMenu = pauseMenu;
        _rewardedService = rewardedService;
    }

    public void Initialize()
    {
        _liveSystem.ValueIncreased += OnLifeIncreased;
        _liveSystem.ValueDecreased += OnLifeDecreased;
        _liveSystem.Death += OnDeath;

        _environmentMove.SpeedChanged += OnSpeedChange;

        _scoreSystem.ChangeValue += OnScoreChange;
        _scoreSystem.ChangeIntegerValue += OnIntScoreChange;

        _inputManager.EscPressed += OnEscPressed;

        _pauseService.PauseRequested += OnPauseRequested;
        _pauseService.ResumeRequested += OnResumeRequested;

        _rewardedService.RewardGranted += OnRewardGranted;

        _reduceSpeedButton.onClick.AddListener(OnRewardReduceSpeedButtonClicked);
        _startAgainButton.onClick.AddListener(StartAgain);
        _rewardLifeButton.onClick.AddListener(OnRewardLifeButtonClicked);

        _rewardYesButton.onClick.AddListener(OnRewardYesClicked);
        _escapeToPauseButton.onClick.AddListener(ReturnFromRewardPanel);

        _pausePanel.SetActive(false);
        _playPanel.SetActive(true);
        _rewardPanel.SetActive(false);
        _finalScore.gameObject.SetActive(false);

        RefreshHudInstant();
    }

    public void LateDispose()
    {
        _liveSystem.ValueIncreased -= OnLifeIncreased;
        _liveSystem.ValueDecreased -= OnLifeDecreased;
        _liveSystem.Death -= OnDeath;

        _environmentMove.SpeedChanged -= OnSpeedChange;

        _scoreSystem.ChangeValue -= OnScoreChange;
        _scoreSystem.ChangeIntegerValue -= OnIntScoreChange;

        _inputManager.EscPressed -= OnEscPressed;

        _pauseService.PauseRequested -= OnPauseRequested;
        _pauseService.ResumeRequested -= OnResumeRequested;

        _rewardedService.RewardGranted -= OnRewardGranted;

        _reduceSpeedButton.onClick.RemoveListener(OnRewardReduceSpeedButtonClicked);
        _startAgainButton.onClick.RemoveListener(StartAgain);
        _rewardLifeButton.onClick.RemoveListener(OnRewardLifeButtonClicked);

        _rewardYesButton.onClick.RemoveListener(OnRewardYesClicked);
        _escapeToPauseButton.onClick.RemoveListener(ReturnFromRewardPanel);
    }

    private void OnEscPressed()
    {
        if (_rewardPanel.activeSelf)
        {
            ReturnFromRewardPanel();
            return;
        }

        if (_resumeBlocked)
            return;

        _pauseService.TogglePause();
    }

    private void OnPauseRequested()
    {
        _pausePanel.SetActive(true);
        _playPanel.SetActive(false);

        ShowNormalPauseMenu();
    }

    private void OnResumeRequested()
    {
        _pausePanel.SetActive(false);
        _playPanel.SetActive(true);

        _rewardPanel.SetActive(false);
        _pendingReward = PendingReward.None;
    }

    private void OnDeath()
    {
        _resumeBlocked = true;

        _pauseService.RequestPause();

        _pausePanel.SetActive(true);
        _playPanel.SetActive(false);

        ShowDeathPauseMenu();
    }

    private void ShowNormalPauseMenu()
    {
        _rewardPanel.SetActive(false);

        _reduceSpeedButton.gameObject.SetActive(true);
        _escButton.gameObject.SetActive(true);
        _finalScore.gameObject.SetActive(false);
    }

    private void ShowDeathPauseMenu()
    {
        _rewardPanel.SetActive(false);

        _reduceSpeedButton.gameObject.SetActive(false);
        _escButton.gameObject.SetActive(false);
        _finalScore.gameObject.SetActive(true);
        _finalScore.text = $"Ñ÷¸ò {_scoreSystem.Score:F0}";
    }

    private void OpenRewardPanel(PendingReward reward)
    {
        _pendingReward = reward;
        _rewardPanel.SetActive(true);
    }

    private void ReturnFromRewardPanel()
    {
        _rewardPanel.SetActive(false);
        _pendingReward = PendingReward.None;
        if (_resumeBlocked)
            ShowDeathPauseMenu();
        else
            ShowNormalPauseMenu();
    }

    private void OnRewardLifeButtonClicked()
    {
        OpenRewardPanel(PendingReward.Life);
    }

    private void OnRewardReduceSpeedButtonClicked()
    {
        OpenRewardPanel(PendingReward.ReduceSpeed);
    }

    private void OnRewardYesClicked()
    {
        switch (_pendingReward)
        {
            case PendingReward.Life:
                _pauseMenu.ShowRewardedForLifes();
                break;

            case PendingReward.ReduceSpeed:
                _pauseMenu.ShowRewardedForReduseSpeed();
                break;
        }
    }

    private void OnRewardGranted()
    {
        if (_pendingReward == PendingReward.Life)
            _resumeBlocked = false;

        ReturnFromRewardPanel();
    }

    private void StartAgain() =>
        _pauseMenu.StartAgain();

    private void OnIntScoreChange() =>
        _gameUIAnimator.ScoreImageJuiceBump();

    private void OnLifeDecreased(int value)
    {
        _gameUIAnimator.LifeImageJuiceBumpDecreased();
        OnLifeChange(value);
    }

    private void OnLifeIncreased(int value)
    {
        _gameUIAnimator.LifeImageJuiceBumpIncrease();
        OnLifeChange(value);
    }

    private void OnLifeChange(int value) =>
        _life.text = value.ToString();

    private void OnSpeedChange(float value) =>
        _speed.text = (value * 10).ToString("F1");

    private void OnScoreChange(float value) =>
        _score.text = value.ToString("F0");

    private void RefreshHudInstant()
    {
        OnLifeChange(_liveSystem.CurrentLives);
        OnSpeedChange(_environmentMove.MoveSpeed);
        OnScoreChange(_scoreSystem.Score);
    }
}