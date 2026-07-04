using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition/Health")]
public class IsDeadNode : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_HealthBase == null) return TaskStatus.Failure;
        return _HealthBase.IsDead ? TaskStatus.Success : TaskStatus.Failure;
    }
}
