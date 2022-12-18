using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceVein : MonoBehaviour
{
    [SerializeField] private ResourceType type;

    public ResourceType GetResourceType()
    {
        return type;
    }
}