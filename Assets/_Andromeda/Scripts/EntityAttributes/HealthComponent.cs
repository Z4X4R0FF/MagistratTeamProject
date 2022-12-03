using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private GameObject hitExplosion;
    [SerializeField] private GameObject destroyExplosion;
    [SerializeField] private Transform hull;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;
    private Transform myTransform;
    private HealthAttributes _healthAttributes;
    private bool _isRecharging;
    private float CurrentHealth { get; set; }
    private float CurrentShield { get; set; }
    private float shieldRechargeCooldown;
    [HideInInspector] public UnityEvent<HealthComponent> onEntityDestroyed;
    public EntityTag EntityTag => _healthAttributes.entityTag;
    public Transform Hull => hull;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Update()
    {
        if (healthSlider != null && shieldSlider != null)
        {
            UpdateBar(healthSlider, CurrentHealth);
            UpdateBar(shieldSlider, CurrentShield);
        }

        shieldRechargeCooldown = Mathf.Max(shieldRechargeCooldown - Time.deltaTime, 0f);
        if (shieldRechargeCooldown == 0f && !_isRecharging)
        {
            _isRecharging = true;
        }
    }

    public void Init(HealthAttributes healthAttributes)
    {
        _healthAttributes = healthAttributes;
        CurrentHealth = _healthAttributes.health;
        CurrentShield = _healthAttributes.shield;
        healthSlider.maxValue = CurrentHealth;
        shieldSlider.maxValue = CurrentShield;
        UpdateBar(healthSlider, CurrentHealth);
        UpdateBar(shieldSlider, CurrentShield);
        hull.tag = _healthAttributes.entityTag.ToString();
        StartCoroutine(Regen());
        WorldInfo.Instance.RegisterEntity(this);
    }

    public void OnEntityHit(Vector3 pos, WeaponAttributes weaponAttributes)
    {
        var hitExp = Instantiate(hitExplosion, pos, Quaternion.identity, myTransform);
        Destroy(hitExp, 2);
        if (CurrentShield == 0)
        {
            var tempHealth = CurrentHealth;
            CurrentHealth -= weaponAttributes.ignoresProtection
                ? weaponAttributes.damage
                : weaponAttributes.damage * (100 - _healthAttributes.protection) * 0.01f;
            Debug.Log($"{name} was hit for {tempHealth - CurrentHealth}");
            CurrentHealth = CurrentHealth <= 0 ? 0 : CurrentHealth;
        }
        else
        {
            CurrentShield -= weaponAttributes.damage;

            CurrentShield = CurrentShield <= 0 ? 0 : CurrentShield;
        }

        _isRecharging = false;
        shieldRechargeCooldown = _healthAttributes.shieldRechargeDelay;

        if (CurrentHealth <= 0)
        {
            onEntityDestroyed.Invoke(this);
            var destroyExp = Instantiate(destroyExplosion, myTransform.position, Quaternion.identity);
            Destroy(destroyExp, 3);
            Destroy(gameObject);
        }
    }

    private void UpdateBar(Slider slider, float value)
    {
        slider.value = value;
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            if (CurrentHealth < _healthAttributes.health)
            {
                CurrentHealth += _healthAttributes.regenerationPS;
                CurrentHealth = CurrentHealth >= _healthAttributes.health ? _healthAttributes.health : CurrentHealth;
            }

            if (CurrentShield < _healthAttributes.shield && _isRecharging)
            {
                CurrentShield += _healthAttributes.shieldRegenerationPS;
                CurrentShield = CurrentShield >= _healthAttributes.shield ? _healthAttributes.shield : CurrentShield;
            }

            yield return new WaitForSeconds(1f);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}