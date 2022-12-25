using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingHint : MonoBehaviourSingleton<LandingHint>
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Text text;

    private void Start()
    {
        rect.gameObject.SetActive(false);
    }

    public void OnEnableLand()
    {
        rect.gameObject.SetActive(true);
        text.text = "F - Приземлиться";
    }

    public void OnEnableTakeOff()
    {
        rect.gameObject.SetActive(true);
        text.text = "F - Взлететь";
    }

    public void DisableHint()
    {
        rect.gameObject.SetActive(false);
    }
}
