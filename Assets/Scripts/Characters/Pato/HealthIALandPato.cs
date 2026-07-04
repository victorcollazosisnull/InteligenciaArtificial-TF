// HealthIALandPato.cs
// Sistema de vida espec�fico del Pato.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandPato : HealthNoCombat
{
    [Header("Pato - Health Config")]
    public GameObject deathParticle;
    public GameObject healParticle;
    public GameObject shieldParticle;

    protected override void Awake()
    {
        base.Awake();
        armor = 0f;
    }

    public override void LoadComponent()
    {
        // Stats b�sicos del pato
        MaxHealth = 70f;
        MaxArmor = 30f;

        // Clasificaci�n
        typeAgent = TypeAgent.Pato;
        _Unity = UnitSC.Pato;

        // Inicializaci�n base
        base.LoadComponent();
    }

    // ?? MUERTE ???????????????????????????????????????????
    public override void Death()
    {
        if (deathParticle != null)
        {
            GameObject effect = Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        Debug.Log("[Pato] murió.");
        base.Death();
    }

    public override bool IsAllied(HealthBase other)
    {
        if (other == null) return false;
        return other.typeAgent == TypeAgent.Hada
            || other.typeAgent == TypeAgent.Unicornio
            || base.IsAllied(other);
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (healParticle != null)
            Destroy(Instantiate(healParticle, transform.position + Vector3.up, Quaternion.identity, transform), 5f);
    }

    public override void AddArmor(float amount)
    {
        base.AddArmor(amount);
        if (shieldParticle != null)
            Destroy(Instantiate(shieldParticle, transform.position + Vector3.up, Quaternion.identity, transform), 5f);
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