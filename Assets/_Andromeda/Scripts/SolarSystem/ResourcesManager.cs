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

    private int _metalPerYield;
    private int _uraniumPerYield;
    private int _mealsPerYield;

    private void Awake()
    {
        CurrentMetal = 50000;
        CurrentUranium = 25000;
        CurrentMeals = 10000;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(HandleResourceHarvest), 0, 5);
    }

    private void Update()
    {
        if (CurrentPeople >=1000)
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
                _metalPerYield += subtract ? -value : value;
                break;
            case ResourceType.Uranium:
                _uraniumPerYield += subtract ? -value : value;
                break;
            case ResourceType.Meals:
                _mealsPerYield += subtract ? -value : value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }
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
        CurrentMetal += _metalPerYield;
        CurrentUranium += _uraniumPerYield;
        CurrentMeals += _mealsPerYield;
        ResourcesPanel.Instance.UpdateResources();
    }
}