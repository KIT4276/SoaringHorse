using System;

public interface IPauseService
{
    event Action PauseRequested;
    event Action ResumeRequested;

    void RequestPause();
    void RequestResume();
}
