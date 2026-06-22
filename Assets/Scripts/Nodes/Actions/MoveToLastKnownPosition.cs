// MoveToLastKnownPosition.cs — usa AICharacterVehicleLand
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToLastKnownPosition : ActionNode
{
    public override void OnStart()
    {
        if (_Blackboard != null && _Blackboard.hasLastKnownPosition && _AIVehicleLand != null)
            _AIVehicleLand.MoveTo(_Blackboard.lastKnownPosition);
    }

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null || _Blackboard == null) return TaskStatus.Failure;
        if (!_Blackboard.hasLastKnownPosition) return TaskStatus.Failure;
        if (_AIVehicleLand.HasReachedDestination())
        {
            _Blackboard.hasLastKnownPosition = false;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
