// Patrol.cs — usa AICharacterVehicleLand para moverse
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Patrol : ActionNode
{
    public float patrolRadius = 10f;

    public override void OnStart()
    {
        if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
        GenerateNewPoint();
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
