using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviourSingleton<ResourcesManager>
{
    public int CurrentMetal { get; private set; }

    public int CurrentUranium { get; private set; }

    public int CurrentMeals { get; private set; }

    public int CurrentPeople { get; private set; }

    public int MetalPerYield { get; private set; }
    public int UraniumPerYield { get; private set; }
    public int MealsPerYield { get; private set; }

    private void Awake()
    {
        CurrentMetal = 1000;
        CurrentUranium = 1500;
        CurrentMeals = 1000;

        MetalPerYield = 0;
        UraniumPerYield = 0;
        MealsPerYield = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(HandleResourceHarvest), 0, 5);
    }

    private void Update()
    {
        if (CurrentPeople >= 1000)
        {
            TutorialManager.Instance.UpdateEventAction("isTargetReached");
        }
    }

    // Update is called once per frame
    public void UpdateResourceYield(ResourceType resourceType, int value, bool subtract)
    {
        switch (resourceType)
        {
            case ResourceType.Metal:
                MetalPerYield += subtract ? -value : value;
                break;
            case ResourceType.Uranium:
                UraniumPerYield += subtract ? -value : value;
                break;
            case ResourceType.Meals:
                MealsPerYield += subtract ? -value : value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }

        ResourcesPanel.Instance.UpdateResourceYield();
    }

    public bool WithdrawResource(ResourceType resourceType, int value)
    {
        bool result;
        switch (resourceType)
        {
            case ResourceType.Metal:
                result = CurrentMetal - value >= 0;
                CurrentMetal -= result ? value : 0;
                break;
            case ResourceType.Uranium:
                result = CurrentUranium - value >= 0;
                CurrentUranium -= result ? value : 0;
                break;
            case ResourceType.Meals:
                result = CurrentMeals - value >= 0;
                CurrentMeals -= result ? value : 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }

        ResourcesPanel.Instance.UpdateResources();
        return result;
    }

    public void UpdatePeople(int value, bool subtract)
    {
        CurrentPeople += subtract ? -value : value;
        ResourcesPanel.Instance.UpdateResources();
    }

    private void HandleResourceHarvest()
    {
        CurrentMetal += MetalPerYield;
        CurrentUranium += UraniumPerYield;
        CurrentMeals += MealsPerYield;
        ResourcesPanel.Instance.UpdateResources();
    }
}