using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private GameObject hitExplosion;
    [SerializeField] private GameObject destroyExplosion;
    [SerializeField] private Transform hull;
    private Transform myTransform;
    private HealthAttributes _healthAttributes;
    public int CurrentHealth { get; private set; }
    [HideInInspector] public UnityEvent<HealthComponent> onEntityDestroyed;
    public EntityTag EntityTag => _healthAttributes.entityTag;

    private void Awake()
    {
        myTransform = transform;
    }

    public void Init(HealthAttributes healthAttributes)
    {
        _healthAttributes = healthAttributes;
        hull.tag = _healthAttributes.entityTag.ToString();
        WorldInfo.Instance.RegisterEntity(this);
    }

    public void OnEntityHit(Vector3 pos, WeaponAttributes weaponAttributes)
    {
        var go = Instantiate(hitExplosion, pos, Quaternion.identity, myTransform);
        Destroy(go, 2);

        CurrentHealth -= weaponAttributes.ignoresProtection
            ? weaponAttributes.damage
            : (int)(weaponAttributes.damage * _healthAttributes.protection * 0.01f);

        if (CurrentHealth <= 0)
        {
            onEntityDestroyed.Invoke(this);
            Destroy(gameObject);
        }
    }
}