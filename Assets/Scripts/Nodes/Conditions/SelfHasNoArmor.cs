using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class SelfHasNoArmor : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_HealthBase == null) return TaskStatus.Failure;
        return _HealthBase.Armor <= 0f ? TaskStatus.Success : TaskStatus.Failure;
    }
}
