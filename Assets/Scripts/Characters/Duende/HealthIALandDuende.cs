// HealthIALandDuende.cs
// Sistema de vida especifico del Duende.
using UnityEngine;

public class HealthIALandDuende : HealthIALand
{
    [Header("Duende - Health Config")]
    public GameObject poofParticle;   // efecto de desaparicion al morir

    public override void LoadComponent()
    {
        MaxHealth = 60f;
        MaxArmor  = 10f;
        typeAgent = TypeAgent.Duende;
        _Unity    = UnitSC.Duende;
        base.LoadComponent();
    }

    public override void Death()
    {
        if (poofParticle != null)
            Instantiate(poofParticle, transform.position, Quaternion.identity);

        Debug.Log("[Duende] fue cazado.");
        base.Death();
    }
}
