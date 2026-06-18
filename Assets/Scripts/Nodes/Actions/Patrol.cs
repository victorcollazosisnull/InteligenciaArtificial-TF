using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Patrol : ActionNode
{
    public float patrolRadius = 10f;

    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override void OnStart()
    {
        _land.StopMoving();
        GenerateNewPoint();
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null) return TaskStatus.Failure;

        if (_land.HasReachedDestination())
            GenerateNewPoint();

        return TaskStatus.Running;
    }

    void GenerateNewPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomDirection,
            out hit,
            patrolRadius,
            NavMesh.AllAreas))
        {
            _land.MoveTo(hit.position);
        }
    }
}
