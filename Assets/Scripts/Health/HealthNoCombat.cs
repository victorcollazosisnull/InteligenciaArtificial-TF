// HealthNoCombat.cs
// Rama para entidades que se destruyen pero NO inician combate:
// Pato, Unicornio, objetos del escenario.
// Diferencias respecto a HealthCombat:
//   - Sin WeaponType ni HurtingMe: no rastrean quien las ataco.
//   - ApplyDamage() simplificado: solo descuenta vida.
//   - Death() sin efectos de sangre ni particulas.
//   - Sin Respawn: se pueden destruir permanentemente.
using UnityEngine;

public class HealthNoCombat : HealthIA
{
    // ── MUERTE ───────────────────────────────────────────
    public override void Death()
    {
        // Solo desactiva la unidad, sin efectos de combate
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

    // ── DANO simplificado ────────────────────────────────
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        // No registra quien ataco, solo descuenta vida
        base.ApplyDamage(damage);
    }
}
