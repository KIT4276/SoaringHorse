using TMPro;
using UnityEngine;
using Zenject;

public class GameUI : MonoBehaviour, IInitializable, ILateDisposable
{
    [SerializeField] private TMP_Text _life;
    [SerializeField] private TMP_Text _exp;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _playPanel;

    private LiveSystem _liveSystem;
    private ExperienceSystem _experienceSystem;
    private ScoreSystem _scoreSystem;
    private IPauseService _pauseService;
    private InputManager _inputManager;

    [Inject]
    public void Construct(LiveSystem liveSystem,ExperienceSystem experienceSystem,  ScoreSystem scoreSystem,
        IPauseService pauseService, InputManager inputManager)
    {
        _liveSystem = liveSystem;
        _experienceSystem = experienceSystem;
        _scoreSystem = scoreSystem;
        _pauseService = pauseService;
        _inputManager = inputManager;
    }

    public void Initialize()
    {
        _liveSystem.ChangeValue += OnLifeChange;
        _experienceSystem.ChangeValue += OnExpChange;
        _scoreSystem.ChangeValue += OnScoreChange;
        _inputManager.EscPressed += OnEscPressed;
        _pauseService.PauseRequested += OnPause;
        _pauseService.ResumeRequested += OnResume;

        _pausePanel.SetActive(false);
        _playPanel.SetActive(true);
    }

    public void LateDispose()
    {
        _liveSystem.ChangeValue -= OnLifeChange;
        _experienceSystem.ChangeValue -= OnExpChange;
        _scoreSystem.ChangeValue -= OnScoreChange;
        _inputManager.EscPressed -= OnEscPressed;
        _pauseService.PauseRequested -= OnPause;
        _pauseService.ResumeRequested -= OnResume;
    }

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

    private void OnLifeChange(int value)
    {
        _life.text = value.ToString();
    }

    private void OnExpChange(float value)
    {
        _exp.text = value.ToString("#.###");
    }

    private void OnScoreChange(float value)
    {
        _score.text = value.ToString("#.###");
    }
}
