// HealthIALand.cs
// FIX: Death() desactiva el Collider para que otros NPCs
// puedan pasar por encima del cadáver sin bloquearse.
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class HealthIALand : HealthCombat
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
        // 1. Detiene el NavMeshAgent
        if (agent != null) agent.enabled = false;

        // 2. Desactiva el BehaviorTree
        BehaviorTree bt = GetComponent<BehaviorTree>();
        if (bt != null) bt.enabled = false;

        // 3. FIX: Desactiva el Collider para no bloquear el paso
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 4. Espera y desactiva el NPC
        StartCoroutine(DesactivarDespues(tiempoMuerte));

        base.Death();
    }

    private IEnumerator DesactivarDespues(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        Desactive();
    }

    public override void Respawn(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;

        BehaviorTree bt = GetComponent<BehaviorTree>();
        if (bt != null) bt.enabled = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        if (agent != null) agent.enabled = true;

        Active();
    }

    public override void Active()
    {
        if (agent != null) agent.enabled = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        base.Active();
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmo) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCenter(), 0.08f);
    }
}
