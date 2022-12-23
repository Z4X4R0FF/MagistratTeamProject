using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text buildingName;
    [SerializeField] private Text cost;
    [SerializeField] private Text hotkeyText;
    [SerializeField] private string hotKey;
    [SerializeField] private BuildingAttributes buildingAttributes;
    [SerializeField] private BuildingPanel buildingControlPanel;

    private void Awake()
    {
        image.sprite = buildingAttributes.icon;
        buildingName.text = buildingAttributes.name;
        cost.text = buildingAttributes.metalCost.ToString();
        hotkeyText.text = hotKey;
    }

    private void Update()
    {
        if (buildingControlPanel.IsOpened)
        {
            if (Input.GetKeyDown(hotKey))
            {
                if (BuildingPlacement.Instance.PlaceBuilding(buildingAttributes))
                {
                    switch (hotKey)
                    {
                        case "1":
                        case "2":
                        case "3":
                        {
                            TutorialManager.Instance.UpdateEventAction("isHarvesterBuilt");
                            break;
                        }
                        case "4":
                        {
                            TutorialManager.Instance.UpdateEventAction("isHouseBuilt");
                            break;
                        }
                        case "5":
                        {
                            TutorialManager.Instance.UpdateEventAction("isTurretBuilt");
                            break;
                        }
                    }
                }
            }
        }
    }
}