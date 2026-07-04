// HealthIALandHada.cs
// Sistema de vida espec�fico del Hada.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandHada : HealthNoCombat
{
    [Header("Hada - Health Config")]
    public GameObject deathParticle;

    public override void LoadComponent()
    {
        // Stats b�sicos del hada
        MaxHealth = 30f;
        MaxArmor = 20f;

        // Clasificaci�n
        typeAgent = TypeAgent.Hada;
        _Unity = UnitSC.Hada;

        // Inicializaci�n base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        if (deathParticle != null)
        {
            // Guardamos la referencia en una variable temporal
            GameObject effect = Instantiate(deathParticle, transform.position, Quaternion.identity);
            // Lo destruimos automáticamente después de 5 segundos
            Destroy(effect, 5f);
        }

        Debug.Log("[Hada] murió.");
        base.Death();
    }

    public override bool IsAllied(HealthBase other)
    {
        if (other == null) return false;
        return other.typeAgent == TypeAgent.Pato
            || other.typeAgent == TypeAgent.Unicornio
            || base.IsAllied(other);
    }

    // ?? DA�O ?????????????????????????????????????????????
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);

        // Si muri�, ejecutar muerte
        if (IsDead)
            Death();
    }
}