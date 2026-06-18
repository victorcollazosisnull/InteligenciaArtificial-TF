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

    [Header("Orco - FuzzySystem")]
    private FuzzyOrcSystem fuzzySystem;
    private VisionSensor vision;
    private HealthBase hp;
    private Blackboard bb;

    protected override void Awake()
    {
        base.Awake();

        fuzzySystem = new FuzzyOrcSystem();

        vision = GetComponent<VisionSensor>();
        hp = GetComponent<HealthBase>();
        bb = GetComponent<Blackboard>();

        Debug.Log("[ORCO] Sistema difuso inicializado");
    }
    private void Update()
    {
        if (bb == null || hp == null || fuzzySystem == null || agent == null) return;

        float distance = 30f;

        if (aIEyeNPC != null &&
            aIEyeNPC.Enemy != null)
        {
            distance = Vector3.Distance(
                transform.position,
                aIEyeNPC.Enemy.position);
        }

        float ira = fuzzySystem.Evaluate(
            bb.hunger,
            hp.HealthPercent,
            distance);


        ApplyFuzzyValues(ira);

        //Debug.Log(
        //$"[FUZZY ORCO] " +
        //$"Hambre:{bb.hunger:F1} " +
        //$"HP:{hp.HealthPercent:F1}% " +
        //$"Ira:{ira:F1} " +
        //$"Vel:{agent.speed:F1} " +
        //$"Daño:{attackDamage:F1}");
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

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

    public void Eat()
    {
        HungerSystem hs = GetComponent<HungerSystem>();
        if (hs != null) hs.Feed(100f);
        Debug.Log("[Orco] comio y se calmo.");
    }

    private void ApplyFuzzyValues(float ira)
    {
        float t = ira / 100f;

        agent.speed =
            Mathf.Lerp(3f, 8f, t);

        attackDamage =
            Mathf.Lerp(20f, 40f, t);

        if (vision != null)
        {
            vision.distance =
                Mathf.Lerp(10f, 20f, t);
        }
    }
}
