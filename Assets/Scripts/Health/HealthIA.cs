using System.Collections;
using UnityEngine;

public class HealthIA : HealthBase
{
    public HealthBase HurtingMe { get; private set; }

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public virtual void ApplyDamage(float damage, HealthBase owner = null)
    {
        if (owner != null)
        {
            HurtingMe = owner;
            StartCoroutine(LimpiarHurtingMe());
        }
        if (Imortal) return;
        base.ApplyDamage(damage);
    }

    private IEnumerator LimpiarHurtingMe()
    {
        yield return new WaitForSeconds(4f);
        HurtingMe = null;
    }

    public override void Active()
    {
        health = MaxHealth;
        armor  = MaxArmor;
        UpdateHealthBar();
        base.Active();
    }

    public override void Desactive()
    {
        base.Desactive();
    }
}
