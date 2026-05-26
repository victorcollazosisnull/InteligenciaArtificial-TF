// Patrol.cs
// Mueve al NPC a puntos aleatorios dentro de un radio definido.
// Usado por: Orco (patrulla zona), Cazador (recorre el bosque).
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

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
        Vector3 dir = Random.insideUnitSphere * patrolRadius;
        dir += transform.position;
        dir.y = transform.position.y;
        _land.MoveTo(dir);
    }
}
