// NPCAnimator.cs
// Script independiente que conecta el estado del NPC con el Animator.
// Se pone en el hijo que tiene el mesh (no en el padre con la IA).
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;     
    private HealthBase health;        

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
        health = GetComponentInParent<HealthBase>();
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

    public void TriggerAttack()
    {
        animator.SetBool("IsAttacking", true);
    }

    public void StopAttack()
    {
        animator.SetBool("IsAttacking", false);
    }
}