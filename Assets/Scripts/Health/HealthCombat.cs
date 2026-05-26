// HealthCombat.cs
// Rama para NPCs que participan en combate activo:
// Duende, Cazador, Orco, Hada, etc.
// Agrega:
//   - Death() con efectos visuales y desactivacion del NavMeshAgent.
//   - Respawn(Vector3) para reaparicion con posicion.
//   - Hittype para reaccionar segun el tipo de arma recibida.
using UnityEngine;
using UnityEngine.AI;

public class HealthCombat : HealthIA
{
    // ── Tipo de dano recibido en el ultimo golpe ─────────
    /// <summary>
    /// Permite a las subclases reaccionar segun el arma:
    /// trampa = paralisis, magia = ralentizar, etc.
    /// </summary>
    public WeaponType Hittype { get; protected set; }

    // ── MUERTE ───────────────────────────────────────────
    public override void Death()
    {
        // Las subclases agregan efectos (particulas, animacion)
        // antes de llamar base.
        base.Death();
    }

    // ── REAPARICION con posicion ─────────────────────────
    /// <summary>Reposiciona al NPC y lo reactiva.</summary>
    public virtual void Respawn(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        Active();
    }

    // ── DANO con tipo de arma ────────────────────────────
    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage, owner);
    }

    /// <summary>Sobrecarga que ademas registra el tipo de arma.</summary>
    public virtual void ApplyDamage(float damage, WeaponType weaponType,
                                    HealthBase owner = null)
    {
        Hittype = weaponType;
        ApplyDamage(damage, owner);
    }
}
