using UnityEngine;

public class AICharacterActionLandUnicorn : AICharacterCombatLand
{
    [Header("Unicornio - Stats de Combate")]
    public float attackDamage = 25f;
    public float attackRange = 2.5f;

    [Header("Unicornio - Fuzzy Stats")]
    public float minAttackDamage = 15f;
    public float maxAttackDamage = 35f;
    public float herdRadius = 8f;

    private FuzzyUnicornSystem _fuzzySystem;
    private VisionSensor _vision;
    private Blackboard _blackboard;
    private HealthBase _health;
    [SerializeField] private float _tendenciaActual = 50f;

    protected override void Awake()
    {
        base.Awake();
        _fuzzySystem = new FuzzyUnicornSystem();
        _vision = GetComponent<VisionSensor>();
        _blackboard = GetComponent<Blackboard>();
        _health = GetComponent<HealthBase>();
    }

    void Update()
    {
        if (_blackboard == null || _health == null) return;

        Transform enemy = _blackboard.enemyTarget;
        if (enemy == null)
        {
            _tendenciaActual = 50f;
            return;
        }

        float healthPercent = _health.HealthPercent;
        float distance = _vision != null ? _vision.GetDistanceToClosestEnemy() : 30f;
        float enemyHealth = 100f;
        HealthBase enemyHealthComp = enemy.GetComponent<HealthBase>();
        if (enemyHealthComp != null) enemyHealth = enemyHealthComp.HealthPercent;
        float nearbyAllies = CountNearbyAllies();

        _tendenciaActual = _fuzzySystem.Evaluate(healthPercent, enemyHealth, distance, nearbyAllies);
        ApplyFuzzyValues(_tendenciaActual);
    }

    public float GetTendenciaActual() => _tendenciaActual;

    private void ApplyFuzzyValues(float tendency)
    {
        float t = tendency / 100f;
        attackDamage = Mathf.Lerp(minAttackDamage, maxAttackDamage, t);
        _vision.distance = Mathf.Lerp(5f, 15f, t);
    }

    private int CountNearbyAllies()
    {
        if (_vision == null) return 0;
        Collider[] colliders = new Collider[16];
        int count = Physics.OverlapSphereNonAlloc(transform.position, herdRadius, colliders, _vision.allyLayers);
        int allies = 0;
        for (int i = 0; i < count; i++)
        {
            if (colliders[i].gameObject == gameObject) continue;
            HealthBase hb = colliders[i].GetComponent<HealthBase>();
            if (hb != null && _health != null && _health.IsAllied(hb))
                allies++;
        }
        return allies;
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public override void Attack()
    {
        if (aIEyeNPC == null || aIEyeNPC.Enemy == null) return;

        float dist = Vector3.Distance(transform.position, aIEyeNPC.Enemy.position);
        if (dist > attackRange) return;

        HealthBase targetHealth = aIEyeNPC.Enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return;

        HealthCombat hc = targetHealth as HealthCombat;
        if (hc != null)
            hc.ApplyDamage(attackDamage, WeaponType.Melee, GetComponent<HealthBase>());
        else
            targetHealth.ApplyDamage(attackDamage);

        Debug.Log($"[Unicornio] Se defiende embistiendo a {aIEyeNPC.Enemy.name}!");
    }
}
