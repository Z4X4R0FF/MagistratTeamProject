using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    public PanelRenderer MenuScreen;
    public Button BackButton;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        BackButton = root.Q<Button>("Back");

        BackButton.clicked += BackButtonClicked;
    }

    void BackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
