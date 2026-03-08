using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _loadButton;

    private StartMenuController _controller;

    [Inject]
    public void Construct(StartMenuController startMenuController)
        => _controller = startMenuController;

    //public void Awake()
    //{
    //}

    public void Init()
    {
        _startGameButton.onClick.AddListener(StartNewGame);
        _loadButton.onClick.AddListener(LoadGame);

        SetButtonsState();
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(StartNewGame);
        _loadButton.onClick.RemoveListener(LoadGame);
    }

    private void SetButtonsState()
    {
        _loadButton.interactable = _controller.HasSave();
    }

    private void LoadGame()
    {
        _startGameButton.interactable = false;
        _loadButton.interactable = false;
        _controller.LoadGame();
        this.gameObject.SetActive(false);
    }

    private void StartNewGame()
    {
        _startGameButton.interactable = false;
        _loadButton.interactable = false;
        _controller.StartNewGame();
        this.gameObject.SetActive(false);
    }
}
