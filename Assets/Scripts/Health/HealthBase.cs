using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBase : MonoBehaviour
{
    public Image HealthBarLocal;
    public Image ArmorBarLocal;
    public GameObject HealthBar;

    [Header("Mostrar barra siempre")]
    public bool ShowHealthBar = false;

    [Header("Stats")]
    public float MaxHealth = 100f;
    public float MaxArmor = 50f;

    protected float health;
    protected float armor;
    protected float shield;

    [Header("Clasificacion")]
    public TypeAgent typeAgent;
    public UnitSC _Unity;

    public bool Imortal = false;
    public bool ShowGizmo = false;
    public Transform AimOffset;

    protected Coroutine visiblebar = null;
    private List<MonoBehaviour> listScript = new List<MonoBehaviour>();
    private List<Collider> colliderHide = new List<Collider>();

    public bool IsDead => health <= 0f;
    public float Shield => shield;
    public float HealthPercent
    {
        get
        {
            if (MaxHealth <= 0f) return 0f;
            return (health / MaxHealth) * 100f;
        }
    }
    public bool IsDangerHealth => health < MaxHealth * 0.5f;

    protected virtual void Awake()
    {
        LoadComponent();
        health = MaxHealth;
        armor = MaxArmor;
        shield = 0f;
    }

    public virtual void LoadComponent()
    {
        if (HealthBar != null)
            HealthBar.SetActive(ShowHealthBar);
        if (HealthBarLocal != null)
            HealthBarLocal.color = Color.green;

        listScript.Clear();
        foreach (var s in GetComponents<MonoBehaviour>())
            if (s != this) listScript.Add(s);

        colliderHide.Clear();
        foreach (var c in GetComponents<Collider>())
            colliderHide.Add(c);
    }

    public virtual void ApplyDamage(float damage)
    {
        if (Imortal) return;

        if (shield > 0)
        {
            float absorbed = Mathf.Min(shield, damage);
            shield -= absorbed;
            damage -= absorbed;
        }

        if (armor > 0 && damage > 0)
        {
            float absorbed = Mathf.Min(armor, damage);
            armor -= absorbed;
            damage -= absorbed;
        }

        if (damage > 0)
            health = Mathf.Max(0, health - damage);

        UpdateHealthBar();
        if (IsDead) Death();
    }

    public virtual void Heal(float amount)
    {
        health = Mathf.Min(MaxHealth, health + amount);
        UpdateHealthBar();
    }

    public virtual void AddShield(float amount)
    {
        shield += amount;
        UpdateHealthBar();
    }

    public virtual void Death() { }

    public virtual void Active()
    {
        if (HealthBarLocal != null) HealthBarLocal.enabled = true;
        if (ArmorBarLocal != null) ArmorBarLocal.enabled = true;
        foreach (var s in listScript) if (s != null) s.enabled = true;
        foreach (var c in colliderHide) if (c != null) c.enabled = true;
        health = MaxHealth;
        armor = MaxArmor;
        shield = 0f;
        UpdateHealthBar();
        gameObject.SetActive(true);
    }

    public virtual void Desactive()
    {
        if (HealthBarLocal != null) HealthBarLocal.enabled = false;
        if (ArmorBarLocal != null) ArmorBarLocal.enabled = false;
        foreach (var s in listScript) if (s != null) s.enabled = false;
        foreach (var c in colliderHide) if (c != null) c.enabled = false;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    public virtual bool IsAllied(HealthBase other) => other != null && typeAgent == other.typeAgent;

    public void UpdateHealthBar()
    {
        if (HealthBarLocal != null)
        {
            HealthBarLocal.fillAmount = MaxHealth > 0 ? health / MaxHealth : 0f;
            HealthBarLocal.color = HealthBarLocal.fillAmount < 0.5f
                ? Color.Lerp(Color.red, Color.green, HealthBarLocal.fillAmount * 2f)
                : Color.green;
        }
        if (ArmorBarLocal != null)
            ArmorBarLocal.fillAmount = MaxArmor > 0 ? armor / MaxArmor : 0f;

        if (gameObject.activeSelf && visiblebar == null && !ShowHealthBar)
            visiblebar = StartCoroutine(MostrarBarra());
    }

    protected Vector3 GetCenter() => transform.position + Vector3.up;

    private IEnumerator MostrarBarra()
    {
        if (HealthBar != null) HealthBar.SetActive(true);
        yield return new WaitForSeconds(10f);
        if (HealthBar != null) HealthBar.SetActive(false);
        visiblebar = null;
    }
}
