using System;
using UnityEngine;

public class HorseshoeSystem 
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

    public void Add(int amount = 1)
    {
        if (amount <= 0)
            return;

        _currentRunCount += amount;
        Changed?.Invoke(_currentRunCount);
    }

    public bool TrySpendFromRun(int amount)
    {
        if (amount <= 0)
            return true;

        if (_currentRunCount < amount)
            return false;

        _currentRunCount -= amount;
        Changed?.Invoke(_currentRunCount);
        return true;
    }
}
