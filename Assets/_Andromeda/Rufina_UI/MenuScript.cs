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

    //public VisualTreeAsset Menu;
    //public VisualTreeAsset Settings;


    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _playButton = _doc.rootVisualElement.Q<Button>("Play");
        _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
        _exitButton = _doc.rootVisualElement.Q<Button>("Exit");
        //_muteButton = _doc.rootVisualElement.Q<Button>("muteButton");
        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

        _playButton.clicked += PlayButtonClicked;
        _settingsButton.clicked += SettingsButtonClicked;
        _exitButton.clicked += ExitButtonClicked;
        //_muteButton.clicked += MuteButtonClicked;


        // _settingsButtons = _settingsButtonsTemplate.CloneTree();
        // var backButton = _settingsButtons.Q<Button>("Back");
        // backButton.clicked += BackButtonClicked;
    }

    private void BackButtonClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_playButton);
        _buttonsWrapper.Add(_settingsButton);
        _buttonsWrapper.Add(_exitButton);
    }

    private void MuteButtonClicked()
    {
        _muted = !_muted;
        var bg = _muteButton.style.backgroundImage;
        bg.value = Background.FromSprite(_muted ? _mutedSprite : _unmutedSprite);
        _muteButton.style.backgroundImage = bg;
        AudioListener.volume = _muted ? 0 : 1;
    }

    private void PlayButtonClicked()
    {
        SceneManager.LoadScene("Loading");
    }

    private void SettingsButtonClicked()
    {
        // _buttonsWrapper.Clear();
        // _buttonsWrapper.Add(_settingsButtons);
    }


    private void ExitButtonClicked()
    {
        Application.Quit();
    }
}