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

    private Label mealGainLabel;
    private Label metalGainLabel;
    private Label uraniumGainLabel;

    private UIDocument _interface;

    // Start is called before the first frame update
    void Start()
    {
        _interface = GetComponent<UIDocument>();

        mealLabel = _interface.rootVisualElement.Q<Label>("TextEat");
        metalLabel = _interface.rootVisualElement.Q<Label>("TextMetal");
        uraniumLabel = _interface.rootVisualElement.Q<Label>("TextEnergy");
        peopleLabel = _interface.rootVisualElement.Q<Label>("TextPeople");

        mealGainLabel = _interface.rootVisualElement.Q<Label>("EatGain");
        metalGainLabel = _interface.rootVisualElement.Q<Label>("MetalGain");
        uraniumGainLabel = _interface.rootVisualElement.Q<Label>("EnergyGain");

        UpdateResources();
    }

    public void UpdateResources()
    {
        mealLabel.text = ResourcesManager.Instance.CurrentMeals.ToString();
        metalLabel.text = ResourcesManager.Instance.CurrentMetal.ToString();
        uraniumLabel.text = ResourcesManager.Instance.CurrentUranium.ToString();
        peopleLabel.text = $"{ResourcesManager.Instance.CurrentPeople.ToString()}/1000";
    }

    public void UpdateResourceYield()
    {
        mealGainLabel.text = (ResourcesManager.Instance.MealsPerYield >= 0
            ? "+"
            : "") + ResourcesManager.Instance.MealsPerYield;
        metalGainLabel.text = (ResourcesManager.Instance.MetalPerYield >= 0
            ? "+"
            : "") + ResourcesManager.Instance.MetalPerYield;
        uraniumGainLabel.text = (ResourcesManager.Instance.UraniumPerYield >= 0
            ? "+"
            : "") + ResourcesManager.Instance.UraniumPerYield;
    }
}