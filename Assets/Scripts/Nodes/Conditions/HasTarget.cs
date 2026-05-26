using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class HasTarget : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null || _Blackboard.currentTarget == null)
            return TaskStatus.Failure;
        return TaskStatus.Success;
    }
}
