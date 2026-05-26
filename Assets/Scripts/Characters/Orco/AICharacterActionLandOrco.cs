// AICharacterActionLandOrco.cs
// Clase concreta del Orco. Hoja del arbol de jerarquia de controles.
// Comportamientos:
//   - Ahuyenta a los duendes (los persigue y golpea).
//   - Ataca a los cazadores.
//   - Se calma cuando come (hambre baja).
// El BehaviorTree del Orco lee AIEyeNPC.Enemy y Blackboard
// para decidir entre atacar, comer o patrullar.
using UnityEngine;

public class AICharacterActionLandOrco : AICharacterCombatLand
{
    [Header("Orco - Stats")]
    public float attackDamage   = 25f;
    public float attackRange    = 2f;
    public float scareRange     = 5f;   // rango en el que ahuyenta duendes

    private HealthBase targetHealth;

    private void Awake() => LoadComponent();

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    /// <summary>
    /// Golpe melee del orco.
    /// Busca el HealthBase del objetivo actual y aplica dano.
    /// </summary>
    public override void Attack()
    {
        if (aIEyeNPC == null || aIEyeNPC.Enemy == null) return;

        targetHealth = aIEyeNPC.Enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return;

        float dist = Vector3.Distance(transform.position,
                                      aIEyeNPC.Enemy.position);
        if (dist > attackRange) return;

        // Aplica dano con tipo Melee
        HealthCombat hc = targetHealth as HealthCombat;
        if (hc != null)
            hc.ApplyDamage(attackDamage, WeaponType.Melee,
                           GetComponent<HealthBase>());
        else
            targetHealth.ApplyDamage(attackDamage);

        Debug.Log($"[Orco] golpea a {aIEyeNPC.Enemy.name} por {attackDamage}");
    }

    /// <summary>
    /// El orco come y se calma: resetea el hambre.
    /// Llamado por el nodo EatFood.
    /// </summary>
    public void Eat()
    {
        HungerSystem hs = GetComponent<HungerSystem>();
        if (hs != null) hs.Feed(100f);
        Debug.Log("[Orco] comio y se calmo.");
    }
}
