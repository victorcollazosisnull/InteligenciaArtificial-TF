// AICharacterActionLandCazador.cs
// Clase concreta del Cazador. Hoja del arbol de jerarquia de controles.
// Comportamientos:
//   - Busca y caza Patos y Unicornios (ataque a distancia con flecha).
//   - Se defiende contra el Orco (contraataque melee).
//   - Coloca trampas en el terreno.
using UnityEngine;

public class AICharacterActionLandCazador : AICharacterCombatLand
{
    [Header("Cazador - Stats")]
    public float arrowDamage    = 30f;
    public float arrowRange     = 12f;
    public float meleeDamage    = 15f;
    public float meleeRange     = 2f;

    [Header("Trampa")]
    public GameObject trapPrefab;
    public float      trapDamage    = 20f;
    public float      trapCooldown  = 15f;
    private float     trapTimer     = 0f;

    private Blackboard blackboard;


    public override void LoadComponent()
    {
        blackboard = GetComponent<Blackboard>();
        base.LoadComponent();
    }

    private void Update()
    {
        if (trapTimer > 0f) trapTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Ataca al objetivo: flecha si es presa (pato/unicornio),
    /// melee si es el Orco que lo esta atacando.
    /// </summary>
    public override void Attack()
    {
        if (aIEyeNPC == null || aIEyeNPC.Enemy == null) return;

        Transform enemy = aIEyeNPC.Enemy;
        float dist = Vector3.Distance(transform.position, enemy.position);

        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return;

        // Melee contra el Orco si esta cerca
        if (targetHealth.typeAgent == TypeAgent.Orco && dist <= meleeRange)
        {
            HealthCombat hc = targetHealth as HealthCombat;
            if (hc != null)
                hc.ApplyDamage(meleeDamage, WeaponType.Melee,
                               GetComponent<HealthBase>());
            Debug.Log("[Cazador] se defiende del Orco con golpe melee.");
            return;
        }

        // Flecha contra patos y unicornios
        if (dist <= arrowRange)
        {
            HealthCombat hc = targetHealth as HealthCombat;
            if (hc != null)
                hc.ApplyDamage(arrowDamage, WeaponType.Arrow,
                               GetComponent<HealthBase>());
            else
                targetHealth.ApplyDamage(arrowDamage);
            Debug.Log($"[Cazador] dispara flecha a {enemy.name}");
        }
    }

    /// <summary>
    /// Coloca una trampa en la posicion actual del cazador.
    /// Llamado por el nodo PlaceTrap del BehaviorTree.
    /// </summary>
    public void PlaceTrap()
    {
        if (trapTimer > 0f || trapPrefab == null) return;

        Vector3 trapPos = transform.position;
        Instantiate(trapPrefab, trapPos, Quaternion.identity);

        if (blackboard != null)
        {
            blackboard.trapPosition = trapPos;
            blackboard.trapPlaced   = true;
        }

        trapTimer = trapCooldown;
        Debug.Log("[Cazador] coloco una trampa.");
    }
}
