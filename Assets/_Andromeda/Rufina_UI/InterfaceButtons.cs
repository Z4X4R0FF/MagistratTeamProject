using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

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


        if (StatusGroup.visible)
        {
            StatusGroup.style.visibility = Visibility.Hidden;
            InfoGroup.style.visibility = Visibility.Hidden;
            Encyclopedy.style.visibility = Visibility.Visible;
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            StatusGroup.style.visibility = Visibility.Visible;
            InfoGroup.style.visibility = Visibility.Visible;
            Encyclopedy.style.visibility = Visibility.Hidden;
            EncText.text = "";
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void GoalButtonClicked()
    {
        EncText = _interface.rootVisualElement.Q<Label>("EncText");
        EncText.text =
            "���� ����:\r\n- ����������� ����������� �������\r\n- ������ ������ ��������: �������, ������� � ���\r\n- �������� ������� \r\n- ���������� ������ � ������� ���������� ����������\r\n- ������� ���������� ���������� �������";
    }

    void ControlsButtonClicked()
    {
        EncText = _interface.rootVisualElement.Q<Label>("EncText");
        EncText.text =
            "���������� �����������:\r\nW � ������\r\nA � �����\r\nS � �����\r\nD � ������\r\nF � ������������/�������� �� �������\r\nR � �������� ���������\r\nB � ������� ����� �������������";
    }
}