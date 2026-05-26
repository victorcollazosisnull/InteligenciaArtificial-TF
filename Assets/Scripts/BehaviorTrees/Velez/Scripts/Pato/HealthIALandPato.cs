// HealthIALandPato.cs
// Sistema de vida específico del Pato.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandPato : HealthNoCombat
{
    [Header("Pato - Health Config")]
    public GameObject deathParticle;

    public override void LoadComponent()
    {
        // Stats básicos del pato
        MaxHealth = 40f;
        MaxArmor = 0f;

        // Clasificación
        typeAgent = TypeAgent.Pato;
        _Unity = UnitSC.Pato;

        // Inicialización base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        // Efecto visual opcional
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        Debug.Log("[Pato] murió.");

        // Ejecuta lógica base
        base.Death();

        // Como es un NPC pasivo, simplemente se desactiva
        Desactive();
    }

    // ?? DANO ?????????????????????????????????????????????
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);

        // Si murió, ejecutar muerte
        if (IsDead)
            Death();
    }
}