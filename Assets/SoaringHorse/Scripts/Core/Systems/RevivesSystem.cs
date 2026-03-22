using System;

public sealed class RevivesSystem
{
    private int _currentRunCount;

    public int CurrentRunCount => _currentRunCount;

    public event Action<int> Changed;

    public void ResetForNewRun()
    {
        if (_currentRunCount == 0)
            return;

        _currentRunCount = 0;
        Changed?.Invoke(_currentRunCount);
    }

    public void RegisterRevive()
    {
        _currentRunCount++;
        Changed?.Invoke(_currentRunCount);
    }
}
