using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsHungry : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null) return TaskStatus.Failure;
        return _Blackboard.isHungry ? TaskStatus.Success : TaskStatus.Failure;
    }
}
