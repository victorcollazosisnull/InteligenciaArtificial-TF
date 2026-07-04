// NPCAnimator.cs
// Conecta el estado del NPC con el Animator y las armas (WeaponSwitcher).
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private HealthBase health;
    private WeaponSwitcher weaponSwitcher;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
        health = GetComponentInParent<HealthBase>();
        weaponSwitcher = GetComponent<WeaponSwitcher>();
    }

    void Update()
    {
        if (health != null && health.IsDead)
        {
            animator.SetBool("IsDead", true);
            return;
        }
        if (agent != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    // ── Generico (Orco, Duende) ──────────────────────────
    public void TriggerAttack()
    {
        animator.SetBool("IsAttacking", true);
    }

    public void StopAttack()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsRanged", false);
        if (weaponSwitcher != null) weaponSwitcher.HideAll();
    }

    // ── Cazador melee ────────────────────────────────────
    public void TriggerMeleeAttack()
    {
        if (weaponSwitcher != null) weaponSwitcher.SetMelee();
        animator.SetBool("IsRanged", false);
        animator.SetBool("IsAttacking", true);
    }

    // ── Cazador ranged ───────────────────────────────────
    public void TriggerRangedAttack()
    {
        if (weaponSwitcher != null) weaponSwitcher.SetBallesta();
        animator.SetBool("IsRanged", true);
        animator.SetBool("IsAttacking", true);
    }

    // ── Dormir (Orco) ────────────────────────────────────
    public void TriggerSleep()
    {
        animator.SetBool("IsSleeping", true);
    }

    public void WakeUp()
    {
        animator.SetBool("IsSleeping", false);
    }
}
