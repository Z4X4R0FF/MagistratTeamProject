using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InterfaceButtons : MonoBehaviour
{
    // Start is called before the first frame update
    //private Button Tutor;
    private VisualElement StatusGroup;
    private VisualElement InfoGroup;
    private VisualElement Encyclopedy;
    private Label EncText;
    private Button Goal;
    private Button Controls;
    private UIDocument _interface;
 
    // Start is called before the first frame update
    void Start()
    {
        _interface = GetComponent<UIDocument>();
        EncText = _interface.rootVisualElement.Q<Label>("EncText");
        EncText.text = "";

        // Tutor = _interface.rootVisualElement.Q<Button>("Tutor");
        // Tutor.clicked += TutorButtonClicked;

        Goal = _interface.rootVisualElement.Q<Button>("Goal");
        Goal.clicked += GoalButtonClicked;

        Controls = _interface.rootVisualElement.Q<Button>("Controls");
        Controls.clicked += ControlsButtonClicked;

    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TutorButtonClicked();
        }
    }

    void TutorButtonClicked()
    {
        Encyclopedy = _interface.rootVisualElement.Q<VisualElement>("Encyclopedy");
        StatusGroup = _interface.rootVisualElement.Q<VisualElement>("StatusGroup");
        InfoGroup = _interface.rootVisualElement.Q<VisualElement>("InfoGroup");


        if (StatusGroup.visible == true)
        {
            StatusGroup.style.visibility = Visibility.Hidden;
            InfoGroup.style.visibility = Visibility.Hidden;
            Encyclopedy.style.visibility = Visibility.Visible;
        }
        else
        {
            StatusGroup.style.visibility = Visibility.Visible;
            InfoGroup.style.visibility = Visibility.Visible;
            Encyclopedy.style.visibility = Visibility.Hidden;
            EncText.text = "";
        }
    }
    void GoalButtonClicked()
    {
        EncText = _interface.rootVisualElement.Q<Label>("EncText");
        EncText.text = "÷ель игры:\r\n- исследовать неизученную систему\r\n- начать добычу ресурсов: металла, энергии и еды\r\n- основать колонии \r\n- обеспечить защиту и услови€ проживани€ колонистам\r\n- достичь требуемого количества жителей";
    }

    void ControlsButtonClicked()
    {
        EncText = _interface.rootVisualElement.Q<Label>("EncText");
        EncText.text = "”правление транспортом:\r\nW Ц вперед\r\nA Ц влево\r\nS Ц назад\r\nD Ц вправо\r\nF Ц приземлитьс€/отлететь от планеты\r\nR Ц починить транспорт\r\nB Ц открыть режим строительства";
    }
 
}
