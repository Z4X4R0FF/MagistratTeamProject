using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameResourceAsset : ScriptableObject
{
    public ResourceType type;
    public string shownName;
    public string id;
    public Sprite icon;
    public GameObject prefab;
}

public enum ResourceType
{
    Metal,
    Uranium,
    Meals
}