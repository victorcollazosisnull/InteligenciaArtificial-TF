using UnityEngine;

public class HealthIALandOrco : HealthIALand
{
    [Header("Orco - Health Config")]
    public GameObject bloodParticle;   

    public override void LoadComponent()
    {
        MaxHealth = 200f;
        MaxArmor  = 30f;
        typeAgent = TypeAgent.Orco;
        _Unity    = UnitSC.Orco;
        base.LoadComponent();
    }

    public override void Death()
    {
        if (bloodParticle != null)
        {
            GameObject effect = Instantiate(bloodParticle, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        Debug.Log("[Orco] murio.");
        base.Death();
    }
}
