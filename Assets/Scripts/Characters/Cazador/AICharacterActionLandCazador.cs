// AICharacterActionLandCazador.cs
// FIX: el Orco solo recibe daño melee, nunca de flecha.
// Si el Orco está lejos del meleeRange simplemente no hace daño.
// La flecha es exclusiva para Patos y Unicornios.
using UnityEngine;

public class AICharacterActionLandCazador : AICharacterCombatLand
{
    [Header("Cazador - Stats")]
    public float arrowDamage = 30f;
    public float arrowRange = 12f;
    public float meleeDamage = 15f;
    public float meleeRange = 2.5f;  // FIX: subido a 2.5 para coincidir con stopDistance

    [Header("Trampa")]
    public GameObject trapPrefab;
    public float trapDamage = 20f;
    public float trapCooldown = 15f;
    private float trapTimer = 0f;

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

    public override void Attack()
    {
        if (aIEyeNPC == null || aIEyeNPC.Enemy == null) return;

        Transform enemy = aIEyeNPC.Enemy;
        float dist = Vector3.Distance(transform.position, enemy.position);
        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return;

        // Melee contra el Orco
        if (targetHealth.typeAgent == TypeAgent.Orco)
        {
            if (dist <= meleeRange)
            {
                HealthCombat hc = targetHealth as HealthCombat;
                if (hc != null)
                    hc.ApplyDamage(meleeDamage, WeaponType.Melee, GetComponent<HealthBase>());
                Debug.Log("[Cazador] se defiende del Orco con golpe melee.");
            }
            return; // FIX: siempre return si es Orco — nunca usa la flecha contra el
        }

        // Flecha contra patos y unicornios UNICAMENTE
        if (dist <= arrowRange)
        {
            HealthCombat hc = targetHealth as HealthCombat;
            if (hc != null)
                hc.ApplyDamage(arrowDamage, WeaponType.Arrow, GetComponent<HealthBase>());
            else
                targetHealth.ApplyDamage(arrowDamage);
            Debug.Log($"[Cazador] dispara flecha a {enemy.name}");
        }
    }

    public void PlaceTrap()
    {
        if (trapTimer > 0f || trapPrefab == null) return;
        Vector3 trapPos = transform.position;
        Instantiate(trapPrefab, trapPos, Quaternion.identity);
        if (blackboard != null)
        {
            blackboard.trapPosition = trapPos;
            blackboard.trapPlaced = true;
        }
        trapTimer = trapCooldown;
        Debug.Log("[Cazador] coloco una trampa.");
    }
}
