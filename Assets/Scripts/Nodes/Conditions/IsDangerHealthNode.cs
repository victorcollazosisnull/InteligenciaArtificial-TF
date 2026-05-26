using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition/Health")]
public class IsDangerHealthNode : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_HealthBase == null) return TaskStatus.Failure;
        return _HealthBase.IsDangerHealth ? TaskStatus.Success : TaskStatus.Failure;
    }
}
