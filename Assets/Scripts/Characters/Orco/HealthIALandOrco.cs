// HealthIALandOrco.cs
// Sistema de vida especifico del Orco.
// Hereda de HealthIALand y personaliza la muerte
// y los efectos de dano del Orco.
using UnityEngine;

public class HealthIALandOrco : HealthIALand
{
    [Header("Orco - Health Config")]
    public GameObject bloodParticle;   // particula de sangre al morir

    public override void LoadComponent()
    {
        // Valores por defecto del Orco
        MaxHealth = 200f;
        MaxArmor  = 30f;
        typeAgent = TypeAgent.Orco;
        _Unity    = UnitSC.Orco;
        base.LoadComponent();
    }

    public override void Death()
    {
        // Efecto visual de muerte del Orco
        if (bloodParticle != null)
            Instantiate(bloodParticle, transform.position, Quaternion.identity);

        Debug.Log("[Orco] murio.");
        base.Death();
    }
}
