using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviourSingleton<DifficultyManager>
{
    [SerializeField] private float healthScaleFactor;

    [SerializeField] private float shieldScaleFactor;


    public float HealthScale { get; private set; } = 1;
    public float ShieldScale { get; private set; } = 1;


// Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(UpdateScale), 0, 300);
    }

    private void UpdateScale()
    {
        HealthScale += healthScaleFactor;
        ShieldScale += shieldScaleFactor;
    }
}