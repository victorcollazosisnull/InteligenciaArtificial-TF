// AICharacterActionLand.cs
// Tercera capa: NPC terrestre que se mueve a pie.
// Agrega el NavMeshAgent para navegacion en el terreno.
// Todos los NPCs del bosque encantado heredan de aqui
// (via AICharacterCombatLand).
using UnityEngine;
using UnityEngine.AI;

public class AICharacterActionLand : AICharacterActions
{
    protected NavMeshAgent agent;
    public    NavMeshAgent Agent => agent;

    public override void LoadComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        base.LoadComponent();
    }

    // ── Movimiento basico ────────────────────────────────

    public void MoveTo(Vector3 destination)
    {
        if (agent != null && agent.isOnNavMesh)
            agent.SetDestination(destination);
    }

    public void StopMoving()
    {
        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();
    }

    public bool HasReachedDestination()
    {
        if (agent == null || agent.pathPending) return false;
        return agent.remainingDistance <= agent.stoppingDistance;
    }
}
