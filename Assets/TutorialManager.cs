using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
{
    [SerializeField] private List<TutorialStep> tutorialSteps;

    private Dictionary<string, bool> _currentStepEventActions = new();

    private int _currentStepIndex;
    private TutorialStep _currentStep;
    private bool isStepButtonPressed;

    private bool isLastStep;

    private UIDocument _interface;

    private Label _tutorialHeader;
    private Label _tutorialTextHighlight;
    private Label _tutorialText;

    // Start is called before the first frame update
    private void Start()
    {
        _interface = GetComponent<UIDocument>();

        _tutorialHeader = _interface.rootVisualElement.Q<Label>("TextHeaderInfo");
        _tutorialTextHighlight = _interface.rootVisualElement.Q<Label>("TextBodyInfo1");
        _tutorialText = _interface.rootVisualElement.Q<Label>("TextBodyInfo2");
        SetTutorialStep();
    }

    // Update is called once per frame
    private void Update()
    {
        for (var i = 0; i < _currentStep.keys.Count; i++)
        {
            if (Input.GetKeyDown(_currentStep.keys[i]))
            {
                isStepButtonPressed = true;
            }
        }

        if (!isLastStep)
        {
            if (isStepButtonPressed)
            {
                if (_currentStepEventActions.Count != 0)
                {
                    if (_currentStepEventActions.All(r => r.Value))
                    {
                        StepDone();
                    }
                }
                else
                {
                    StepDone();
                }
            }
        }
    }

    private void StepDone()
    {
        if (_currentStep.events.Count != 0)
        {
            _currentStep.events.ForEach(r => r.Invoke());
        }

        _currentStepIndex++;
        SetTutorialStep();
    }

    public void UpdateEventAction(string eventActionName)
    {
        if (_currentStepEventActions.ContainsKey(eventActionName))
        {
            _currentStepEventActions[eventActionName] = true;
        }
    }

    private void SetTutorialStep()
    {
        _currentStep = tutorialSteps[_currentStepIndex];

        _tutorialHeader.text = _currentStep.stepName;
        _tutorialTextHighlight.text = _currentStep.highlightText;
        _tutorialText.text = _currentStep.text;

        if (_currentStepIndex == tutorialSteps.Count - 1) isLastStep = true;

        isStepButtonPressed = _currentStep.keys.Count == 0;
        _currentStepEventActions.Clear();
        if (_currentStep.eventActions.Count != 0)
        {
            _currentStepEventActions = _currentStep.eventActions.ToDictionary(s => s, v => false);
        }
    }

    [Serializable]
    public class TutorialStep
    {
        public string stepName;
        public string highlightText;
        public string text;
        public List<KeyCode> keys;
        public List<string> eventActions;
        public List<UnityEvent> events;
    }
}