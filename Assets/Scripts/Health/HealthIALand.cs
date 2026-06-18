// HealthIALand.cs
// Clase intermedia para NPCs terrestres de combate.
// Hereda de HealthCombat y agrega la gestion del NavMeshAgent.
// FIX: Death() ahora desactiva el BehaviorTree y espera X segundos
// antes de desactivar el NPC completo, permitiendo animacion de muerte.
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class HealthIALand : HealthCombat
{
    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    [Header("Muerte")]
    public float tiempoMuerte = 3f; // segundos antes de desactivar el NPC

    public override void LoadComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        base.LoadComponent();
    }

    public override void Death()
    {
        // 1. Detiene el NavMeshAgent
        if (agent != null)
            agent.enabled = false;

        // 2. Desactiva el BehaviorTree — el NPC deja de pensar
        BehaviorTree bt = GetComponent<BehaviorTree>();
        if (bt != null)
            bt.enabled = false;

        // 3. Espera X segundos y desactiva el NPC (da tiempo a la animacion)
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
        if (bt != null)
            bt.enabled = true;

        if (agent != null)
            agent.enabled = true;

        Active();
    }

    public override void Active()
    {
        if (agent != null)
            agent.enabled = true;
        base.Active();
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmo) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCenter(), 0.08f);
    }
}