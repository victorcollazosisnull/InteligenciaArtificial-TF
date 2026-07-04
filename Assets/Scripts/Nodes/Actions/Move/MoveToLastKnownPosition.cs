// MoveToLastKnownPosition.cs
// FIX: retorna Failure al llegar (igual que EatFood y Flee).
// Si retorna Success el Selector queda satisfecho y no llega a Patrol.
// Failure = "terminé lo que tenía que hacer, Selector sigue evaluando".
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
            return TaskStatus.Failure; // FIX: Failure para que Selector llegue a Patrol
        }
        return TaskStatus.Running;
    }
}
