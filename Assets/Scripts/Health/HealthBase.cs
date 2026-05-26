// HealthBase.cs
// Clase raiz del sistema de Health.
// Toda entidad con vida del juego hereda de esta clase.
// NO instanciar directamente: usar HealthCombat o HealthNoCombat.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBase : MonoBehaviour
{
    // ── UI ──────────────────────────────────────────────
    public Image      HealthBarLocal;   // imagen de relleno de la barra
    public Image      ArmorBarLocal;    // imagen de relleno de armadura
    public GameObject HealthBar;        // contenedor de la barra

    [Header("Mostrar barra siempre")]
    public bool ShowHealthBar = false;

    // ── Atributos vitales ────────────────────────────────
    [Header("Stats")]
    public float MaxHealth = 100f;
    public float MaxArmor  = 50f;

    protected float health;
    protected float armor;
    // campo privado usado por LoadComponent para leer MaxHealth antes de asignarlo
    private float maxHealth;

    // ── Clasificacion de la unidad ───────────────────────
    [Header("Clasificacion")]
    public TypeAgent typeAgent;   // bando: Duende, Cazador, Orco...
    public UnitSC    _Unity;      // tipo de unidad para pool y efectos

    // ── Soporte visual ───────────────────────────────────
    public bool      Imortal   = false;
    public bool      ShowGizmo = false;
    public Transform AimOffset;

    // ── Estado de combate ────────────────────────────────
    protected Coroutine visiblebar = null;

    // listas internas para Active / Desactive
    private List<MonoBehaviour> listScript    = new List<MonoBehaviour>();
    private List<Collider>      colliderHide  = new List<Collider>();

    // ── PROPIEDADES CALCULADAS ───────────────────────────
    // El BehaviorTree las lee cada frame para tomar decisiones.

    /// <summary>true cuando la salud llega a 0. El nodo IsDeadNode lo usa.</summary>
    public bool IsDead         { get => health <= 0; }

    /// <summary>true cuando HP cae por debajo del 50%. IsDangerHealthNode lo usa.</summary>
    public bool IsDangerHealth { get => health < MaxHealth / 2f; }

    // ── INICIALIZACION ───────────────────────────────────
    // Llamar base.LoadComponent() al final en cada subclase.

    public virtual void LoadComponent()
    {
        maxHealth = MaxHealth;
        health    = MaxHealth;
        armor     = MaxArmor;

        // Configurar barra de vida
        if (HealthBar != null)
            HealthBar.SetActive(ShowHealthBar);
        if (HealthBarLocal != null)
            HealthBarLocal.color = Color.green;
        UpdateHealthBar();

        // Registrar scripts y colliders para Active / Desactive
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
            if (s != this) listScript.Add(s);

        Collider[] colliders = GetComponents<Collider>();
        foreach (var c in colliders)
            colliderHide.Add(c);
    }

    // ── METODOS VIRTUALES ────────────────────────────────

    /// <summary>Aplica dano. Las subclases agregan efectos antes de llamar base.</summary>
    public virtual void ApplyDamage(float damage)
    {
        if (Imortal) return;
        // La armadura absorbe primero; cuando llega a 0 el dano va a HP
        armor  = Mathf.Max(0, armor - damage);
        if (armor <= 0)
            health = Mathf.Max(0, health - damage);
        UpdateHealthBar();
    }

    /// <summary>Ejecuta la secuencia de muerte. Las subclases agregan efectos.</summary>
    public virtual void Death() { }

    /// <summary>Reactiva la unidad (respawn). Restaura vida y componentes.</summary>
    public virtual void Active()
    {
        if (HealthBarLocal != null) HealthBarLocal.enabled = true;
        if (ArmorBarLocal  != null) ArmorBarLocal.enabled  = true;

        foreach (var s in listScript)
            if (s != null) s.enabled = true;
        foreach (var c in colliderHide)
            if (c != null) c.enabled = true;

        health = MaxHealth;
        armor  = MaxArmor;
        UpdateHealthBar();
        gameObject.SetActive(true);
        if (HealthBarLocal != null)
            HealthBarLocal.color = Color.green;
    }

    /// <summary>Desactiva la unidad (la devuelve al pool).</summary>
    public virtual void Desactive()
    {
        if (HealthBarLocal != null) HealthBarLocal.enabled = false;
        if (ArmorBarLocal  != null) ArmorBarLocal.enabled  = false;

        foreach (var s in listScript)
            if (s != null) s.enabled = false;
        foreach (var c in colliderHide)
            if (c != null) c.enabled = false;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    // ── UTILIDADES ───────────────────────────────────────

    /// <summary>Devuelve true si otra entidad es del mismo bando.</summary>
    public bool IsAllied(HealthBase other) => typeAgent == other.typeAgent;

    /// <summary>Actualiza visualmente la barra de vida y armadura.</summary>
    public void UpdateHealthBar()
    {
        if (HealthBarLocal != null)
        {
            HealthBarLocal.fillAmount = (MaxHealth > 0) ? health / MaxHealth : 0f;
            if (HealthBarLocal.fillAmount < 0.5f)
                HealthBarLocal.color = Color.Lerp(Color.red, Color.green,
                    HealthBarLocal.fillAmount * 2f);
        }
        if (ArmorBarLocal != null)
            ArmorBarLocal.fillAmount = (MaxArmor > 0) ? armor / MaxArmor : 0f;

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
