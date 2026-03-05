public interface ISceneLoader
{
    void Load(string sceneName);
    bool IsDone { get; }
}
