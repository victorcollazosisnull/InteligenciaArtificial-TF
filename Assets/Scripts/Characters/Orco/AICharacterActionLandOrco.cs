// AICharacterActionLandOrco.cs
// FIX: agent ya no se hereda (está en rama Vehicle).
// Se obtiene via GetComponent<NavMeshAgent>() en Awake.
using UnityEngine;
using UnityEngine.AI;

public class AICharacterActionLandOrco : AICharacterCombatLand
{
    [Header("Orco - Stats")]
    public float attackDamage = 25f;
    public float attackRange = 2f;
    public float scareRange = 5f;

    [Header("Orco - FuzzySystem")]
    private FuzzyOrcSystem fuzzySystem;
    private VisionSensor vision;
    private HealthBase hp;
    private Blackboard bb;
    private NavMeshAgent _navAgent;  // FIX: cacheado manualmente, no heredado

    private HealthBase targetHealth;

    protected override void Awake()
    {
        base.Awake();
        fuzzySystem = new FuzzyOrcSystem();
        vision = GetComponent<VisionSensor>();
        hp = GetComponent<HealthBase>();
        bb = GetComponent<Blackboard>();
        _navAgent = GetComponent<NavMeshAgent>();  // FIX: GetComponent directo
        Debug.Log("[ORCO] Sistema difuso inicializado");
    }

    private void Update()
    {
        if (bb == null || hp == null || fuzzySystem == null || _navAgent == null) return;

        float distance = 30f;
        if (aIEyeNPC != null && aIEyeNPC.Enemy != null)
            distance = Vector3.Distance(transform.position, aIEyeNPC.Enemy.position);

        float ira = fuzzySystem.Evaluate(bb.hunger, hp.HealthPercent, distance);
        ApplyFuzzyValues(ira);
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

        float dist = Vector3.Distance(transform.position, aIEyeNPC.Enemy.position);
        if (dist > attackRange) return;

        HealthCombat hc = targetHealth as HealthCombat;
        if (hc != null)
            hc.ApplyDamage(attackDamage, WeaponType.Melee, GetComponent<HealthBase>());
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
        _navAgent.speed = Mathf.Lerp(3f, 8f, t);   // FIX: _navAgent en vez de agent
        attackDamage = Mathf.Lerp(20f, 40f, t);
        if (vision != null)
            vision.distance = Mathf.Lerp(10f, 20f, t);
    }
}
