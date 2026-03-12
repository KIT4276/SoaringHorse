using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Progression Config", fileName = "ProgressionConfig")]
public class ProgressionConfig : ScriptableObject
{
    [SerializeField] private float _experienceIncrease = 0.01f;
    [SerializeField] private int _timeExperienceIncrease = 4;

    public float ExperienceIncrease { get => _experienceIncrease; }
    public int TimeExperienceIncrease { get => _timeExperienceIncrease; }
}
