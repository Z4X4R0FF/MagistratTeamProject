using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuildingPanel : MonoBehaviourSingleton<BuildingPanel>
{
    [SerializeField] private RectTransform buildingPanelRect;

    public bool IsOpened { get; private set; }

    private bool _isBuildingEnabled;
    private void Update()
    {
        if (_isBuildingEnabled)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                SwitchPanelView();
            }
        }
    }

    private void SwitchPanelView()
    {
        buildingPanelRect.DOMoveY(IsOpened ? -200 : 0, 1);
        IsOpened = !IsOpened;
    }

    public void EnableBuilding(bool enable)
    {
        if (IsOpened) SwitchPanelView();
        _isBuildingEnabled = enable;
    }
}