using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public sealed class SceneLoader : ISceneLoader, ITickable
{
    private AsyncOperation _op;

    public event Action Done;

    public void Load(string sceneName)
    {
        if (_op != null && !_op.isDone)
        {
            Debug.LogWarning("Scene is already loading");
            return;
        }

        _op = SceneManager.LoadSceneAsync(sceneName);
    }

    public void Tick()
    {
        if (_op == null)
            return;

        if (!_op.isDone)
            return;

        _op = null;
        Done?.Invoke();
    }
}
