// HealthIALandHada.cs
// Sistema de vida específico del Hada.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandHada : HealthNoCombat
{
    [Header("Hada - Health Config")]
    public GameObject deathParticle;

    public override void LoadComponent()
    {
        // Stats básicos del hada
        MaxHealth = 30f;
        MaxArmor = 20f;

        // Clasificación
        typeAgent = TypeAgent.Hada;
        _Unity = UnitSC.Hada;

        // Inicialización base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        // Efecto visual opcional
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        Debug.Log("[Hada] murió.");

        // Ejecuta lógica base
        base.Death();

        // Como es un NPC pasivo, simplemente se desactiva
        Desactive();
    }

    // ?? DAÑO ?????????????????????????????????????????????
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);

        // Si murió, ejecutar muerte
        if (IsDead)
            Death();
    }
}