using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartExpeditionUi : MonoBehaviour
{
    [SerializeField] private Button systemSelectionNextButton;

    [SerializeField] private Button startGameButton;
    // Start is called before the first frame update
    void Start()
    {
        systemSelectionNextButton.onClick.AddListener(OnSystemSelected);
        startGameButton.onClick.AddListener(StartGame);
    }

    public void Init()
    {
        //generating 3 systems
    }

    private void OnSystemSelected()
    {
        
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    
}
