using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class EnemyVisible : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard != null && _Blackboard.currentTarget != null)
            return TaskStatus.Success;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            return TaskStatus.Success;
        return TaskStatus.Failure;
    }
}
