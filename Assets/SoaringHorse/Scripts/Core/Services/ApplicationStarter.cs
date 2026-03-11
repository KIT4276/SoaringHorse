public class ApplicationStarter
{
    private IYandexService _yandex;
    private StartMenu _startMenu;

    public ApplicationStarter(IYandexService yandex, StartMenu startMenu)
    {
        _yandex = yandex;
        _startMenu = startMenu;
    }

    public void InitYandex() => 
        _yandex.Init();

    public void InitStartUI() => 
        _startMenu.Init();
}