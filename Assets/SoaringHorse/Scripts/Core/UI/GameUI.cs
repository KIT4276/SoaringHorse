using TMPro;
using UnityEngine;
using Zenject;

public class GameUI : MonoBehaviour, IInitializable, ILateDisposable
{
    [SerializeField] private TMP_Text _life;
    [SerializeField] private TMP_Text _exp;
    [SerializeField] private TMP_Text _score;

    private LiveSystem _liveSystem;
    private ExperienceSystem _experienceSystem;
    private ScoreSystem _scoreSystem;

    [Inject]
    public void Construct(LiveSystem liveSystem,ExperienceSystem experienceSystem,  ScoreSystem scoreSystem)
    {
        _liveSystem = liveSystem;
        _experienceSystem = experienceSystem;
        _scoreSystem = scoreSystem;
    }

    public void Initialize()
    {
        _liveSystem.ChangeValue += OnLifeChange;
        _experienceSystem.ChangeValue += OnExpChange;
        _scoreSystem.ChangeValue += OnScoreChange;
    }

    public void LateDispose()
    {
        _liveSystem.ChangeValue -= OnLifeChange;
        _experienceSystem.ChangeValue -= OnExpChange;
        _scoreSystem.ChangeValue -= OnScoreChange;
    }

    private void OnLifeChange(int value) => 
        _life.text = value.ToString();

    private void OnExpChange(float value) => 
        _exp.text = value.ToString("#.###");

    private void OnScoreChange(float value) => 
        _score.text = value.ToString("#.###");
}
