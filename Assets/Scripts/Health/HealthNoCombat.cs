using UnityEngine;

public class HealthNoCombat : HealthIA
{
    public override void Death()
    {
        base.Death();
    }

    public override void Active()
    {
        base.Active();
    }

    public override void Desactive()
    {
        base.Desactive();
    }

    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);
    }
}
