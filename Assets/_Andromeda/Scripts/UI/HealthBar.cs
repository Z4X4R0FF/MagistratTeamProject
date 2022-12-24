using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        healthSlider.onValueChanged.AddListener(SetVisibility);
        shieldSlider.onValueChanged.AddListener(SetVisibility);
        canvas.enabled = false;
    }

    private void SetVisibility(float val)
    {
        if (Math.Abs(healthSlider.value - healthSlider.maxValue) < 1 &&
            Math.Abs(shieldSlider.value - shieldSlider.maxValue) < 1)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }
    }
}