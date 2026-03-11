using System;

public interface ISceneLoader
{
    void Load(string sceneName);

    event Action Done;
}
