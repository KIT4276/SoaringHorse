public static class MetaSaveMapping
{
    public static void ApplyTo(this MetaSaveData data, IMetaProgress progress)
    {
        if (data == null || progress == null) return;
        progress.ApplyFromSave(data);
    }

    public static void CaptureFrom(this MetaSaveData data, IMetaProgress progress)
    {
        if (data == null || progress == null) return;

        data.bestScore = progress.BestScore;
        data.totalHorseshoes = progress.TotalHorseshoes;
        data.totalRevives = progress.BestRevives;
        data.bestRunTime = progress.BestRunTime;
        data.TouchNow();
        data.Fixup();
    }
}
