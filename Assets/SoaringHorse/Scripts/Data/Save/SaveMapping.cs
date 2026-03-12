public static class SaveMapping
{
    public static void ApplyTo(this SaveData data, IPlayerProgress progress)
    {
        if (data == null || progress == null) return;
        progress.ApplyFromSave(data);
    }

    public static void CaptureFrom(this SaveData data, IPlayerProgress progress)
    {
        if (data == null || progress == null) return;
        data.exp = progress.Exp;
        data.score = progress.Score;
        data.lifes = progress.Lifes;
        data.TouchNow();
        data.Fixup();
    }
}