// MoveToTarget.cs — usa AICharacterVehicleLand para moverse
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public enum TargetFaction { Enemy, Ally }

[TaskCategory("BosqueEncantado/Move")]
public class MoveToTarget : ActionNode
{
    public TargetFaction targetToFollow;
    public float stopDistance = 2.0f;

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null) return TaskStatus.Failure;

        Transform target = null;
        if (targetToFollow == TargetFaction.Enemy)
        {
            if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
                target = _AIEyeNPC.Enemy;
            else if (_Blackboard != null && _Blackboard.enemyTarget != null)
                target = _Blackboard.enemyTarget;
        }
        else
        {
            if (_AIEyeNPC != null && _AIEyeNPC.Allied != null)
                target = _AIEyeNPC.Allied;
            else if (_Blackboard != null && _Blackboard.allyTarget != null)
                target = _Blackboard.allyTarget;
        }

        if (target == null) { _AIVehicleLand.StopMoving(); return TaskStatus.Failure; }

        HealthBase targetHealth = target.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            if (targetToFollow == TargetFaction.Enemy)
            {
                if (_Blackboard != null) _Blackboard.enemyTarget = null;
                if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            }
            else
            {
                if (_Blackboard != null) _Blackboard.allyTarget = null;
                if (_AIEyeNPC != null) _AIEyeNPC.Allied = null;
            }
            _AIVehicleLand.StopMoving();
            return TaskStatus.Failure;
        }

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= stopDistance) { _AIVehicleLand.StopMoving(); return TaskStatus.Success; }

        _AIVehicleLand.MoveTo(target.position);
        return TaskStatus.Running;
    }
}
