// MoveToFood.cs
// Mueve al NPC hacia la comida registrada en el Blackboard.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToFood : ActionNode
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
        if (_Blackboard.foodTarget == null)       return TaskStatus.Failure;

        _land.MoveTo(_Blackboard.foodTarget.position);

        if (_land.HasReachedDestination()) return TaskStatus.Success;
        return TaskStatus.Running;
    }
    public override void OnEnd()
    {
        if (_land != null)
            _land.StopMoving();
    }
}
