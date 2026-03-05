using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : ISceneLoader
{
    private AsyncOperation _op;

    public void Load(string sceneName)
    {
        _op = SceneManager.LoadSceneAsync(sceneName);
    }

    public bool IsDone => _op != null && _op.isDone;
}
