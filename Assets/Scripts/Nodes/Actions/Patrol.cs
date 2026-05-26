// Patrol.cs
// FIX: genera un nuevo punto cada vez que OnStart() es llamado,
// así el NPC retoma la patrulla correctamente después de comer o atacar.
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

    // OnStart se llama cada vez que el nodo se activa (no solo la primera vez)
    public override void OnStart()
    {
        Debug.Log("PATRULLANDO OTRA VEZ");
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

        if (UnityEngine.AI.NavMesh.SamplePosition(
            randomDirection,
            out hit,
            patrolRadius,
            UnityEngine.AI.NavMesh.AllAreas))
        {
            _land.MoveTo(hit.position);

            Debug.Log($"[{gameObject.name}] nuevo punto patrol: {hit.position}");
        }
    }
}
