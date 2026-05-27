using UnityEngine;
using UnityEngine.AI;

public class HealthCombat : HealthIA
{
    public WeaponType Hittype { get; protected set; }

    public override void Death()
    {
        base.Death();
    }

    public virtual void Respawn(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        Active();
    }

    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage, owner);
    }

    public virtual void ApplyDamage(float damage, WeaponType weaponType,
                                    HealthBase owner = null)
    {
        Hittype = weaponType;
        ApplyDamage(damage, owner);
    }
}
