// AICharacterActionLandDuende.cs
// Clase concreta del Duende. Hoja del arbol de jerarquia de controles.
// Comportamientos:
//   - Roba comida al Orco (se acerca y toma su comida).
//   - Caza Hadas (las persigue y captura).
//   - Huye cuando el Orco lo detecta.
using UnityEngine;

public class AICharacterActionLandDuende : AICharacterCombatLand
{
    [Header("Duende - Stats")]
    public float stealRange     = 1.5f;   // rango para robar comida
    public float attackDamage   = 10f;    // dano al cazar hadas
    public float attackRange    = 1.8f;

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public override void Attack()
    {
        if (aIEyeNPC == null || aIEyeNPC.Enemy == null) return;

        float dist = Vector3.Distance(transform.position,
                                      aIEyeNPC.Enemy.position);
        if (dist > attackRange) return;

        HealthBase targetHealth = aIEyeNPC.Enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return;

        HealthCombat hc = targetHealth as HealthCombat;
        if (hc != null)
            hc.ApplyDamage(attackDamage, WeaponType.Melee,
                           GetComponent<HealthBase>());
        else
            targetHealth.ApplyDamage(attackDamage);

        Debug.Log($"[Duende] ataca a {aIEyeNPC.Enemy.name}");
    }

    public void StealFood()
    {
        Blackboard bb = GetComponent<Blackboard>();
        if (bb == null || bb.foodTarget == null) return;

        float dist = Vector3.Distance(transform.position,
                                      bb.foodTarget.position);
        if (dist > stealRange) return;

        Debug.Log("[Duende] robo la comida del Orco!");
        Destroy(bb.foodTarget.gameObject);
        bb.foodTarget = null;

        HungerSystem hs = GetComponent<HungerSystem>();
        if (hs != null) hs.Feed(80f);
    }
}
