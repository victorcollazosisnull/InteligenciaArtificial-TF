// Patrol.cs
// FIX: eliminado StopMoving() de OnStart().
// Llamar StopMoving() antes de GenerateNewPoint() causaba
// un frame de idle visible antes de empezar a moverse.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Patrol : ActionNode
{
    public float patrolRadius = 10f;

    public override void OnStart()
    {
        GenerateNewPoint(); // FIX: directo al punto, sin StopMoving primero
    }

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null) return TaskStatus.Failure;
        if (_AIVehicleLand.HasReachedDestination())
            GenerateNewPoint();
        return TaskStatus.Running;
    }

    void GenerateNewPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            _AIVehicleLand.MoveTo(hit.position);
    }
}
