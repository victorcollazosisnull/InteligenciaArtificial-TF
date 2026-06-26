// HealthIALandPato.cs
// Sistema de vida espec�fico del Pato.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandPato : HealthNoCombat
{
    [Header("Pato - Health Config")]
    public GameObject deathParticle;

    public override void LoadComponent()
    {
        // Stats b�sicos del pato
        MaxHealth = 40f;
        MaxArmor = 0f;

        // Clasificaci�n
        typeAgent = TypeAgent.Pato;
        _Unity = UnitSC.Pato;

        // Inicializaci�n base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        // Efecto visual opcional
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        Debug.Log("[Pato] muri�.");

        // Ejecuta l�gica base
        base.Death();

        // Como es un NPC pasivo, simplemente se desactiva
        Desactive();
    }

    public override bool IsAllied(HealthBase other)
    {
        if (other == null) return false;
        return other.typeAgent == TypeAgent.Hada
            || other.typeAgent == TypeAgent.Unicornio
            || base.IsAllied(other);
    }

    // ?? DANO ?????????????????????????????????????????????
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);

        // Si muri�, ejecutar muerte
        if (IsDead)
            Death();
    }
}