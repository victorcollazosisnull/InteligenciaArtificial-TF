// HealthIALandCazador.cs
// Sistema de vida especifico del Cazador.
using UnityEngine;

public class HealthIALandCazador : HealthIALand
{
    [Header("Cazador - Health Config")]
    public GameObject deathParticle;

    public override void LoadComponent()
    {
        MaxHealth = 100f;
        MaxArmor  = 20f;
        typeAgent = TypeAgent.Cazador;
        _Unity    = UnitSC.Cazador;
        base.LoadComponent();
    }

    public override void Death()
    {
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        Debug.Log("[Cazador] fue derrotado.");
        base.Death();
    }
}
