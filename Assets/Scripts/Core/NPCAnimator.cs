// NPCAnimator.cs
// Conecta el estado del NPC con el Animator y las armas (WeaponSwitcher).
// Se pone en el hijo que tiene el mesh.
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private HealthBase health;
    private WeaponSwitcher weaponSwitcher; // solo el Cazador lo tiene

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

    // ?? Generico (Orco, Duende) ??????????????????????????
    public void TriggerAttack()
    {
        animator.SetBool("IsAttacking", true);
    }

    public void StopAttack()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsRanged", false);
        // Oculta ambas armas al terminar el ataque
        if (weaponSwitcher != null) weaponSwitcher.HideAll();
    }

    // ?? Cazador melee ????????????????????????????????????
    public void TriggerMeleeAttack()
    {
        if (weaponSwitcher != null) weaponSwitcher.SetMelee();
        animator.SetBool("IsRanged", false);
        animator.SetBool("IsAttacking", true);
    }

    // ?? Cazador ranged ???????????????????????????????????
    public void TriggerRangedAttack()
    {
        if (weaponSwitcher != null) weaponSwitcher.SetBallesta();
        animator.SetBool("IsRanged", true);
        animator.SetBool("IsAttacking", true);
    }
}
