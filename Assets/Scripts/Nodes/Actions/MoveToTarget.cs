// MoveToTarget.cs
// Mueve al NPC hacia el objetivo actual registrado en el Blackboard.
// Retorna Failure si no hay objetivo. Running mientras se mueve.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToTarget : ActionNode
{
    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null || _Blackboard == null) return TaskStatus.Failure;
        if (_Blackboard.currentTarget == null)    return TaskStatus.Failure;

        _land.MoveTo(_Blackboard.currentTarget.position);
        return TaskStatus.Running;
    }
}
