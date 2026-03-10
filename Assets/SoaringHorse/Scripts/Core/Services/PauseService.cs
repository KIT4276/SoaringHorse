using System;

public sealed class PauseService : IPauseService
{
    public event Action PauseRequested;
    public event Action ResumeRequested;

    private bool _paused;

    public void RequestPause()
    {

        if (_paused)
            return;

        _paused = true;
        PauseRequested?.Invoke();
    }

    public void RequestResume()
    {
        if (!_paused)
            return;

        _paused = false;
        ResumeRequested?.Invoke();
    }

    public void TogglePause()
    {
        if (_paused)
            RequestResume();
        else
            RequestPause();
    }

    public void ResetState()
    {
        _paused = false;
    }
}