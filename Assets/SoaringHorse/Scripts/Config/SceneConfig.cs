using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Scene Config", fileName = "SceneConfig")]
public class SceneConfig : ScriptableObject
{
    [SerializeField] private string _gameSceneName = "GameScene";

    public string GameSceneName { get => _gameSceneName; }
}