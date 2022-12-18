using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuScript : MonoBehaviour
{
    [Header("Mute Button")] [SerializeField]
    private Sprite _mutedSprite;

    [SerializeField] private Sprite _unmutedSprite;
    private bool _muted;

    [Header("Settings")] [SerializeField] private VisualTreeAsset _settingsButtonsTemplate;
    private VisualElement _settingsButtons;
    private VisualElement _buttonsWrapper;

    private UIDocument _doc;

    public Button _playButton;
    public Button _settingsButton;
    public Button _exitButton;
    public Button _muteButton;
    public Button _backButton;



    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _playButton = _doc.rootVisualElement.Q<Button>("Play");
        _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
        _exitButton = _doc.rootVisualElement.Q<Button>("Exit");
        _backButton = _doc.rootVisualElement.Q<Button>("Back");
        _muteButton = _doc.rootVisualElement.Q<Button>("Mute");
        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

        _playButton.clicked += PlayButtonClicked;
        _settingsButton.clicked += SettingsButtonClicked;
        _exitButton.clicked += ExitButtonClicked;
        _muteButton.clicked += MuteButtonClicked;


        _backButton.clicked += BackButtonClicked;
    }

    private void BackButtonClicked()
    {
        _playButton = _doc.rootVisualElement.Q<Button>("Play");
        _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
        _exitButton = _doc.rootVisualElement.Q<Button>("Exit");
        _backButton = _doc.rootVisualElement.Q<Button>("Back");
        _muteButton = _doc.rootVisualElement.Q<Button>("Mute");

        if (_muteButton.visible == true)
        {
            _playButton.style.visibility = Visibility.Visible;
            _settingsButton.style.visibility = Visibility.Visible;
            _exitButton.style.visibility = Visibility.Visible;
            _muteButton.style.visibility = Visibility.Hidden;
            _backButton.style.visibility = Visibility.Hidden;
        }
    }

    private void MuteButtonClicked()
    {
        if(_muted = !_muted)
        {
            _muteButton = _doc.rootVisualElement.Q<Button>("Mute");
            _muteButton.text = "ќтключить звук";
            AudioListener.volume = _muted ? 0 : 1;
        }
        else
        {
            _muteButton = _doc.rootVisualElement.Q<Button>("Mute");
            _muteButton.text = "¬ключить звук";
            AudioListener.volume = _muted ? 1 : 0;
        }

    }

    private void PlayButtonClicked()
    {
        SceneManager.LoadScene("Loading");
    }

    private void SettingsButtonClicked()
    {
        _playButton = _doc.rootVisualElement.Q<Button>("Play");
        _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
        _exitButton = _doc.rootVisualElement.Q<Button>("Exit");
        _backButton = _doc.rootVisualElement.Q<Button>("Back");
        _muteButton = _doc.rootVisualElement.Q<Button>("Mute");

        if (_playButton.visible == true)
        {
            _playButton.style.visibility = Visibility.Hidden;
            _settingsButton.style.visibility = Visibility.Hidden;
            _exitButton.style.visibility = Visibility.Hidden;
            _muteButton.style.visibility = Visibility.Visible;
            _backButton.style.visibility = Visibility.Visible;
        }
    }


    private void ExitButtonClicked()
    {
        Application.Quit();
    }
}