using System;

public sealed class PauseService : IPauseService
{
    public event Action PauseRequested;
    public event Action ResumeRequested;

    public void RequestPause() => PauseRequested?.Invoke();
    public void RequestResume() => ResumeRequested?.Invoke();
}