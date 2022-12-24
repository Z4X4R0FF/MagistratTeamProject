using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private GameObject hitExplosion;
    [SerializeField] private GameObject destroyExplosion;
    [SerializeField] private GameObject destroySound;
    [SerializeField] private Transform hull;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private bool isPlayer;
    private Transform myTransform;
    private HealthAttributes _healthAttributes;
    private bool _isRecharging;
    private float CurrentHealth { get; set; }
    private float CurrentShield { get; set; }

    private float MaxHealth;
    private float MaxShield;
    private float shieldRechargeCooldown;
    [HideInInspector] public UnityEvent<HealthComponent> onEntityDestroyed;
    public EntityTag EntityTag => _healthAttributes.entityTag;
    public Transform Hull => hull;

    private void Awake()
    {
        myTransform = transform;
    }

    public EntityType GetEntityType() => _healthAttributes.entityType;

    private void Update()
    {
        if (isPlayer)
        {
            PlayerHealthDisplay.Instance.UpdatePlayerStat(PlayerHealthDisplay.PlayerStat.Health, CurrentHealth,
                _healthAttributes.health);
            PlayerHealthDisplay.Instance.UpdatePlayerStat(PlayerHealthDisplay.PlayerStat.Shield, CurrentShield,
                _healthAttributes.shield);
        }
        else
        {
            if (healthSlider != null && shieldSlider != null)
            {
                UpdateBar(healthSlider, CurrentHealth);
                UpdateBar(shieldSlider, CurrentShield);
            }
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
        if (!isPlayer)
        {
            MaxHealth =
                Mathf.FloorToInt(_healthAttributes.health * DifficultyManager.Instance.HealthScale);
            Debug.Log($"Init: hp - {healthAttributes.health}; scale - {DifficultyManager.Instance.HealthScale}");
            MaxShield =
                Mathf.FloorToInt(_healthAttributes.health * DifficultyManager.Instance.ShieldScale);
        }
        else
        {
            MaxHealth = _healthAttributes.health;
            MaxShield = _healthAttributes.shield;
        }

        CurrentHealth = MaxHealth;
        CurrentShield = MaxShield;
        if (isPlayer)
        {
            PlayerHealthDisplay.Instance.UpdatePlayerStat(PlayerHealthDisplay.PlayerStat.Health, CurrentHealth,
                MaxHealth);
            PlayerHealthDisplay.Instance.UpdatePlayerStat(PlayerHealthDisplay.PlayerStat.Shield, CurrentShield,
                MaxShield);
        }
        else
        {
            healthSlider.maxValue = CurrentHealth;
            shieldSlider.maxValue = CurrentShield;
            UpdateBar(healthSlider, CurrentHealth);
            UpdateBar(shieldSlider, CurrentShield);
        }

        hull.tag = _healthAttributes.entityTag.ToString();
        StartCoroutine(Regen());
        WorldInfo.Instance.RegisterEntity(this);
    }

    public void OnEntityHit(Vector3 pos, WeaponAttributes weaponAttributes)
    {
        var hitExp = Instantiate(hitExplosion, pos, Quaternion.identity, myTransform);
        hitExp.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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
            var destroyExp = Instantiate(destroyExplosion, myTransform.position, myTransform.rotation);
            var destrSound = Instantiate(destroySound, myTransform.position, Quaternion.identity);
            Destroy(destroyExp, 3);
            Destroy(destrSound, 5);
            if (isPlayer)
            {
                hull.gameObject.SetActive(false);
                Invoke(nameof(EndGame), 3f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void EndGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(0);
    }

    private void UpdateBar(Slider slider, float value)
    {
        slider.value = value;
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth += _healthAttributes.regenerationPS;
                CurrentHealth = CurrentHealth >= MaxHealth ? MaxHealth : CurrentHealth;
            }

            if (CurrentShield < MaxShield && _isRecharging)
            {
                CurrentShield += _healthAttributes.shieldRegenerationPS;
                CurrentShield = CurrentShield >= MaxShield ? MaxShield : CurrentShield;
            }

            yield return new WaitForSeconds(1f);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}