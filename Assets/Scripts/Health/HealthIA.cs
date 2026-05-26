// HealthIA.cs
// Extiende HealthBase conectando el sistema de salud con los
// componentes de IA del NPC: AIEyeNPC (vision) y AICharacterActions.
// Es el puente entre la vida del NPC y su BehaviorTree.
using System.Collections;
using UnityEngine;

public class HealthIA : HealthBase
{
    // ── Quien me ataco (memoria de combate) ─────────────
    /// <summary>
    /// Guarda la referencia al Health de quien nos ataco durante 4 segundos.
    /// Los nodos del BT pueden usarlo para enfocar la respuesta.
    /// </summary>
    public HealthBase HurtingMe { get; private set; }

    // ── INICIALIZACION ───────────────────────────────────
    public override void LoadComponent()
    {
        // SIEMPRE llamar base al final
        base.LoadComponent();
    }

    // ── DANO: registra quien ataco y reacciona ───────────
    /// <param name="damage">Puntos de dano a aplicar.</param>
    /// <param name="owner">Health de quien disparo (puede ser null).</param>
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

    // Coroutine: olvida al atacante tras 4 segundos
    private IEnumerator LimpiarHurtingMe()
    {
        yield return new WaitForSeconds(4f);
        HurtingMe = null;
    }

    // ── REAPARICION ──────────────────────────────────────
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
