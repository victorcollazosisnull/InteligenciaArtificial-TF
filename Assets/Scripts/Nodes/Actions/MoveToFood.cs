// MoveToFood.cs — usa AICharacterVehicleLand
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Food")]
public class MoveToFood : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null || _Blackboard == null) return TaskStatus.Failure;
        if (_Blackboard.foodTarget == null) return TaskStatus.Failure;

        _AIVehicleLand.MoveTo(_Blackboard.foodTarget.position);
        if (_AIVehicleLand.HasReachedDestination()) return TaskStatus.Success;
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
    }
}
