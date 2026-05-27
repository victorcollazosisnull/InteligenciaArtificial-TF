// AICharacterActionLandUnicorn.cs
// Clase concreta del Unicornio. Hoja del árbol de jerarquía de controles.
// Comportamientos:
//   - Se defiende atacando con su cuerno cuando un enemigo está cerca.
//   - Huye del peligro calculando una ruta de escape.
using UnityEngine;
using UnityEngine.AI;

public class AICharacterActionLandUnicorn : AICharacterCombatLand
{
    [Header("Unicornio - Stats de Combate")]
    public float attackDamage = 25f;         // Daño del ataque (ej. embestida)
    public float attackRange = 2.5f;        // Rango para poder golpear

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    // ?? Combate ??????????????????????????????????????????????

    /// <summary>
    /// El unicornio se defiende de sus atacantes. 
    /// Llamado por el nodo de Ataque en el BT.
    /// </summary>
    public override void Attack()
    {
        if (aIEyeNPC == null || aIEyeNPC.Enemy == null) return;

        float dist = Vector3.Distance(transform.position, aIEyeNPC.Enemy.position);
        if (dist > attackRange) return;

        HealthBase targetHealth = aIEyeNPC.Enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return;

        HealthCombat hc = targetHealth as HealthCombat;
        if (hc != null)
        {
            hc.ApplyDamage(attackDamage, WeaponType.Melee, GetComponent<HealthBase>());
        }
        else
        {
            targetHealth.ApplyDamage(attackDamage);
        }

        Debug.Log($"[Unicornio] Se defiende embistiendo a {aIEyeNPC.Enemy.name}!");
    }
}