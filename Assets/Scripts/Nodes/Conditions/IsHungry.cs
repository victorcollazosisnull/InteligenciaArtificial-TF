// IsHungry.cs
// Retorna Success si el NPC tiene hambre (Blackboard.isHungry = true).
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsHungry : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null) return TaskStatus.Failure;
        return _Blackboard.isHungry ? TaskStatus.Success : TaskStatus.Failure;
    }
}
