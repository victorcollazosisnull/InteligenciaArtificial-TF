using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]

public class AllyVisible : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard != null && _Blackboard.allyTarget != null)
            return TaskStatus.Success;
        if (_AIEyeNPC != null && _AIEyeNPC.Allied != null)
            return TaskStatus.Success;
        return TaskStatus.Failure;
    }
}
