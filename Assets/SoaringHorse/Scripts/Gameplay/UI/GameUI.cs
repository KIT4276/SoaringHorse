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
    [SerializeField] private GameObject _deathRoot;
    [SerializeField] private GameObject _pauseRoot;
    [SerializeField] private GameObject _InformPanel;

    [Header("Pause Menu")]
    [SerializeField] private Button _reduceSpeedButton;
    [SerializeField] private Button[] _startAgainButton;
    [SerializeField] private Button[] _rewardLifeButton;
    [SerializeField] private Button[] _escButton;

    [Header("Run Progress")]
    [SerializeField] private TMP_Text _finalScore;
    [SerializeField] private TMP_Text _horseshoes;
    [SerializeField] private TMP_Text _runTime;
    [SerializeField] private TMP_Text _revives;

    [Header("Meta Progress")]
    [SerializeField] private TMP_Text _bestScore;
    [SerializeField] private TMP_Text _totalHorseHoes;
    [SerializeField] private TMP_Text _bestRunTime;
    [SerializeField] private TMP_Text _bestRevives;

    private SpeedSystem _speedSystem;
    private LiveSystem _liveSystem;
    private ScoreSystem _scoreSystem;
    private HorseshoeSystem _horseshoeSystem;
    private RunTimeSystem _runTimeSystem;
    private RevivesSystem _revivesSystem;
    private IMetaProgressSyncService _metaProgressSyncService;
    private IPauseService _pauseService;
    private InputManager _inputManager;
    private PauseMenu _pauseMenu;
    private IRewardedService _rewardedService;

    private PendingReward _pendingReward = PendingReward.None;

    private bool _resumeBlocked;
    // private EnvironmentMove _environmentMove;

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
        HorseshoeSystem horseshoeSystem,
        RunTimeSystem runTimeSystem,
        RevivesSystem revivesSystem,
        IMetaProgressSyncService metaProgressSyncService,
        IPauseService pauseService,
        InputManager inputManager,
        PauseMenu pauseMenu,
        IRewardedService rewardedService,
       /* EnvironmentMove environmentMove*/
       SpeedSystem speedSystem)
    {
        //_environmentMove = environmentMove; ;
        _speedSystem = speedSystem;
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
        _horseshoeSystem = horseshoeSystem;
        _runTimeSystem = runTimeSystem;
        _revivesSystem = revivesSystem;
        _metaProgressSyncService = metaProgressSyncService;
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

        _speedSystem.CurrentSpeedChanged += OnSpeedChange;

        _scoreSystem.ChangeValue += OnScoreChange;
        _scoreSystem.ChangeIntegerValue += OnIntScoreChange;

        _inputManager.EscPressed += OnEscPressed;

        _pauseService.PauseRequested += OnPauseRequested;
        _pauseService.ResumeRequested += OnResumeRequested;

        _rewardedService.RewardGranted += OnRewardGranted;

        _reduceSpeedButton.onClick.AddListener(OnRewardReduceSpeedButtonClicked);
        foreach (var button in _startAgainButton)
            button.onClick.AddListener(StartAgain);
        foreach (var button in _rewardLifeButton)
            button.onClick.AddListener(OnRewardLifeButtonClicked);
        foreach (var button in _escButton)
            button.onClick.AddListener(OnEscPressed);

        SetGameplayUiState();

        RefreshHudInstant();
        FillMetaProgressTable();
    }

    public void LateDispose()
    {
        _liveSystem.ValueIncreased -= OnLifeIncreased;
        _liveSystem.ValueDecreased -= OnLifeDecreased;
        _liveSystem.Death -= OnDeath;

        _speedSystem.CurrentSpeedChanged -= OnSpeedChange;

        _scoreSystem.ChangeValue -= OnScoreChange;
        _scoreSystem.ChangeIntegerValue -= OnIntScoreChange;

        _inputManager.EscPressed -= OnEscPressed;

        _pauseService.PauseRequested -= OnPauseRequested;
        _pauseService.ResumeRequested -= OnResumeRequested;

        _rewardedService.RewardGranted -= OnRewardGranted;

        _reduceSpeedButton.onClick.RemoveListener(OnRewardReduceSpeedButtonClicked);
        foreach (var button in _startAgainButton)
            button.onClick.RemoveListener(StartAgain);
        foreach (var button in _rewardLifeButton)
            button.onClick.RemoveListener(OnRewardLifeButtonClicked);
        foreach (var button in _escButton)
            button.onClick.RemoveListener(OnEscPressed);
    }

    private void OnEscPressed()
    {
        if (_resumeBlocked)
            return;

        _pauseService.TogglePause();
    }

    private void OnPauseRequested() =>
        ShowCurrentPauseMenu();

    private void OnResumeRequested()
    {
        SetGameplayUiState();
        _pendingReward = PendingReward.None;
    }

    private void OnDeath()
    {
        _resumeBlocked = true;

        _pauseService.RequestPause();
    }

    private void ShowNormalPauseMenu()
    {
        _pausePanel.SetActive(true);
        _playPanel.SetActive(false);
        _deathRoot.SetActive(false);
        _pauseRoot.SetActive(true);
        SetEscButtonsActive(true);
        SetInformPanelActive(false);
    }

    private void ShowDeathPauseMenu()
    {
        _pausePanel.SetActive(true);
        _playPanel.SetActive(false);
        _deathRoot.SetActive(true);
        _pauseRoot.SetActive(false);
        SetEscButtonsActive(false);
        SetInformPanelActive(false);
        FillRunResultsTable();
        FillMetaProgressTable();
    }

    private void ShowCurrentPauseMenu()
    {
        if (_resumeBlocked)
            ShowDeathPauseMenu();
        else
            ShowNormalPauseMenu();
    }

    private void ShowPostRewardPauseMenu()
    {
        _pausePanel.SetActive(true);
        _playPanel.SetActive(false);
        _deathRoot.SetActive(false);
        _pauseRoot.SetActive(false);
        SetEscButtonsActive(false);
        SetInformPanelActive(true);
    }

    private void FillRunResultsTable()
    {
        _finalScore.text = _scoreSystem.Score.ToString("F0");
        _horseshoes.text = _horseshoeSystem.CurrentRunCount.ToString();
        _runTime.text = _runTimeSystem.CurrentRunTime.ToString("F1");
        _revives.text = _revivesSystem.CurrentRunCount.ToString();
    }

    private void FillMetaProgressTable()
    {
        _bestScore.text = _metaProgressSyncService.ReadBestScore().ToString();
        _totalHorseHoes.text = _metaProgressSyncService.ReadTotalHorseshoes().ToString();
        _bestRunTime.text = _metaProgressSyncService.ReadBestRunTime().ToString("F1");
        _bestRevives.text = _metaProgressSyncService.ReadBestRevives().ToString();
    }

    private void SetGameplayUiState()
    {
        _pausePanel.SetActive(false);
        _playPanel.SetActive(true);
        _deathRoot.SetActive(false);
        _pauseRoot.SetActive(false);
        SetInformPanelActive(false);
    }

    private void SetInformPanelActive(bool isActive)
    {
        if (_InformPanel != null)
            _InformPanel.SetActive(isActive);
    }

    private void SetEscButtonsActive(bool isActive)
    {
        foreach (var button in _escButton)
            button.gameObject.SetActive(isActive);
    }

    private void OnRewardLifeButtonClicked()
    {
        RequestReward(PendingReward.Life);
    }

    private void OnRewardReduceSpeedButtonClicked()
    {
        RequestReward(PendingReward.ReduceSpeed);
    }

    private void RequestReward(PendingReward reward)
    {
        if (_pendingReward != PendingReward.None)
            return;

        _pendingReward = reward;
        SetInformPanelActive(false);

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

        _pendingReward = PendingReward.None;
        ShowPostRewardPauseMenu();
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
        OnSpeedChange(_speedSystem.CurrentSpeed);
        OnScoreChange(_scoreSystem.Score);
    }
}
