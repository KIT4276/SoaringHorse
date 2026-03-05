using System;
using Zenject;

public sealed class ProgressAutosave : IInitializable, IDisposable
{
    private readonly IPlayerProgress _progress;
    private readonly ISaveService _save;

    public ProgressAutosave(IPlayerProgress progress, ISaveService save)
    {
        _progress = progress;
        _save = save;
    }

    public void Initialize()
    {
        _progress.Changed += OnChanged;
    }

    public void Dispose()
    {
        _progress.Changed -= OnChanged;
    }

    private void OnChanged()
    {
        _save.CommitFrom(_progress, requestCloud: true);
    }
}