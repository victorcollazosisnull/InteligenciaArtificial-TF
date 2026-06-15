// HealthIALandUnicornio.cs
// Sistema de vida específico del Unicornio.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandUnicornio : HealthCombat
{
    [Header("Unicornio - Health Config")]
    public GameObject deathParticle;

    public override void LoadComponent()
    {
        // Stats del unicornio
        MaxHealth = 150f;
        MaxArmor = 30f;

        // Clasificación
        typeAgent = TypeAgent.Unicornio;
        _Unity = UnitSC.Unicornio;

        // Inicialización base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        // Partículas opcionales
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        Debug.Log("[Unicornio] murió.");

        // Ejecuta lógica base
        base.Death();

        // NPC pasivo ? solo se desactiva
        Desactive();
    }

    // ?? DANO ?????????????????????????????????????????????
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);

        // Si la vida llegó a 0, morir
        if (IsDead)
            Death();
    }
}