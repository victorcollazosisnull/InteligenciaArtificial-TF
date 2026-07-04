// HealthIALandUnicornio.cs
// Sistema de vida espec�fico del Unicornio.
// NPC pasivo: no participa en combate.

using UnityEngine;

public class HealthIALandUnicornio : HealthIALand
{
    [Header("Unicornio - Health Config")]
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
        MaxHealth = 150f;
        MaxArmor = 50f;

        typeAgent = TypeAgent.Unicornio;
        _Unity = UnitSC.Unicornio;

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

        Debug.Log("[Unicornio] murió.");
        base.Death();
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