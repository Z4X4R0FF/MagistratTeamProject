using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthDisplay : MonoBehaviourSingleton<PlayerHealthDisplay>
{
    private UIDocument _interface;

    private Label speedLabel;
    private Label healthLabel;
    private Label shieldLabel;

    private void Start()
    {
        _interface = GetComponent<UIDocument>();

        speedLabel = _interface.rootVisualElement.Q<Label>("Speed");
        healthLabel = _interface.rootVisualElement.Q<Label>("Health");
        shieldLabel = _interface.rootVisualElement.Q<Label>("Shield");
    }

    public void UpdatePlayerStat(PlayerStat stat, float value, float maxValue)
    {
        switch (stat)
        {
            case PlayerStat.Speed:
                speedLabel.text = $"{Math.Round(value, 2)}/{maxValue}";
                break;
            case PlayerStat.Health:
                healthLabel.text = $"{Math.Round(value, 2)}/{maxValue}";
                break;
            case PlayerStat.Shield:
                shieldLabel.text = $"{Math.Round(value, 2)}/{maxValue}";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
    }

    public enum PlayerStat
    {
        Speed,
        Health,
        Shield
    }
}