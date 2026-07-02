using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System.Collections;

public class HealthNoCombat : HealthIA
{
    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    [Header("Muerte")]
    public float tiempoMuerte = 3f;
    public override void LoadComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        base.LoadComponent();
    }
    public override void Death()
    {
        if (agent != null)
        {
            Debug.Log("[HealthNoCombat] Deteniendo NavMeshAgent.");
            agent.enabled = false;
        }

        BehaviorTree bt = GetComponent<BehaviorTree>();
        if (bt != null) bt.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        StartCoroutine(DesactivarDespues(tiempoMuerte));

        base.Death();
    }
    private IEnumerator DesactivarDespues(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        Desactive();
    }

    public override void Active()
    {
        base.Active();
    }

    public override void Desactive()
    {
        base.Desactive();
    }

    public override void ApplyDamage(float damage, HealthBase owner = null)
    {
        base.ApplyDamage(damage);
    }
}
