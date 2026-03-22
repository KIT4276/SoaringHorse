public static class SaveMapping
{
    public static void ApplyTo(this RunSaveData data, IRunProgress progress)
    {
        if (data == null || progress == null) return;
        progress.ApplyFromSave(data);
    }

    public static void CaptureFrom(this RunSaveData data, IRunProgress progress)
    {
        if (data == null || progress == null) return;
        data.score = progress.Score;
        data.lifes = progress.Lifes;
        data.speed = progress.Speed;
        data.runTime = progress.RunTime;
        data.TouchNow();
        data.Fixup();
    }
}