using System;

public sealed class PauseService : IPauseService
{
    public event Action PauseRequested;
    public event Action ResumeRequested;

    private bool _paused = false;



    public void RequestPause()
    {
        if (_paused)
        {
            ResumeRequested?.Invoke();
            _paused = false;
        }
        else
        {
            PauseRequested?.Invoke();
            _paused = true;
        }
    }

    public void RequestResume() => ResumeRequested?.Invoke();
}