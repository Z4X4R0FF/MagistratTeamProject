using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    
    private void Start()
    {
        startButton.onClick.AddListener(OpenExpeditionPanel);
        optionsButton.onClick.AddListener(OpenOptionsPanel);
        exitButton.onClick.AddListener(Application.Quit);
    }
    

    private void OpenExpeditionPanel()
    {
        
    }

    private void OpenOptionsPanel()
    {
        
    }
}