// AICharacterVehicle.cs
// Rama de MOVIMIENTO — segunda capa desde AICharacterControl.
// Contiene toda la logica de desplazamiento: NavMeshAgent,
// MoveTo, StopMoving, HasReachedDestination.
// Los NPCs que solo se mueven (Pato, Unicornio) heredan de aqui.
using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicle : AICharacterControl
{
    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    public override void LoadComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        base.LoadComponent();
    }

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
        if (agent == null) return false;
        if (agent.pathPending) return false;
        if (agent.remainingDistance > agent.stoppingDistance) return false;
        if (agent.hasPath && agent.velocity.sqrMagnitude != 0f) return false;
        return true;
    }
}
