using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsTargetInAttackRange : ConditionNode
{
    public float attackRange = 2.5f;

    public override TaskStatus OnUpdate()
    {
        Transform target = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            target = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            target = _Blackboard.currentTarget;

        if (target == null) return TaskStatus.Failure;
        return Vector3.Distance(transform.position, target.position) <= attackRange
            ? TaskStatus.Success : TaskStatus.Failure;
    }
}
