using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    // Start is called before the first frame update
    private void Start()
    {
        startButton.onClick.AddListener(OpenExpeditionPanel);
        optionsButton.onClick.AddListener(OpenOptionsPanel);
        exitButton.onClick.AddListener(Application.Quit);
    }

    // Update is called once per frame

    private void OpenExpeditionPanel()
    {
        
    }

    private void OpenOptionsPanel()
    {
        
    }
}