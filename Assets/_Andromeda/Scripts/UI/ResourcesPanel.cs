using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourcesPanel : MonoBehaviourSingleton<ResourcesPanel>
{
    private Label mealLabel;
    private Label metalLabel;
    private Label uraniumLabel;
    private Label peopleLabel;

    private UIDocument _interface;

    // Start is called before the first frame update
    void Start()
    {
        _interface = GetComponent<UIDocument>();

        mealLabel = _interface.rootVisualElement.Q<Label>("TextEat");
        metalLabel = _interface.rootVisualElement.Q<Label>("TextMetal");
        uraniumLabel = _interface.rootVisualElement.Q<Label>("TextEnergy");
        peopleLabel = _interface.rootVisualElement.Q<Label>("TextPeople");

        UpdateResources();
    }

    public void UpdateResources()
    {
        mealLabel.text = ResourcesManager.Instance.CurrentMeals.ToString();
        metalLabel.text = ResourcesManager.Instance.CurrentMetal.ToString();
        uraniumLabel.text = ResourcesManager.Instance.CurrentUranium.ToString();
        peopleLabel.text = $"{ResourcesManager.Instance.CurrentPeople.ToString()}/1000";
    }
}