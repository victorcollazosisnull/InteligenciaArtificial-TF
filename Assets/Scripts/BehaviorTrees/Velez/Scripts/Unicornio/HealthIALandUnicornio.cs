// HealthIALandUnicornio.cs
// Sistema de vida espec�fico del Unicornio.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandUnicornio : HealthCombat
{
    [Header("Unicornio - Health Config")]
    public GameObject deathParticle;

    protected override void Awake()
    {
        base.Awake();
        armor = 0f;
    }

    public override void LoadComponent()
    {
        // Stats del unicornio
        MaxHealth = 150f;
        MaxArmor = 30f;

        // Clasificaci�n
        typeAgent = TypeAgent.Unicornio;
        _Unity = UnitSC.Unicornio;

        // Inicializaci�n base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        // Part�culas opcionales
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        Debug.Log("[Unicornio] muri�.");

        // Ejecuta l�gica base
        base.Death();

        // NPC pasivo ? solo se desactiva
        Desactive();
    }

    // ?? DANO ?????????????????????????????????????????????
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);

        // Si la vida lleg� a 0, morir
        if (IsDead)
            Death();
    }
    public float GetHealthPercentage()
    {
        if (MaxHealth <= 0) return 0f;
        return HealthPercent;
    }
}