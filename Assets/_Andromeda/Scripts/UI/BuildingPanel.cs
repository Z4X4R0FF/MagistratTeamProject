using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField] private RectTransform buildingPanelRect;

    public bool IsOpened { get; private set; }

    private void Awake()
    {
        buildingPanelRect.DOMoveY(-200, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchPanelView();
        }
    }

    private void SwitchPanelView()
    {
        buildingPanelRect.DOMoveY(IsOpened ? -200 : 0, 1);
        IsOpened = !IsOpened;
    }
}