using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Food")]
public class MoveToFood : ActionNode
{
    public float stopDistance = 2f;

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null || _Blackboard == null) return TaskStatus.Failure;
        if (_Blackboard.foodTarget == null) return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, _Blackboard.foodTarget.position);
        if (dist <= stopDistance)
        {
            _AIVehicleLand.StopMoving();
            return TaskStatus.Success;
        }

        _AIVehicleLand.MoveTo(_Blackboard.foodTarget.position);
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
    }
}
