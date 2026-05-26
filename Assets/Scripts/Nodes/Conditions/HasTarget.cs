// HasTarget.cs
// Retorna Success si el Blackboard tiene un objetivo activo.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class HasTarget : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null || _Blackboard.currentTarget == null)
            return TaskStatus.Failure;
        return TaskStatus.Success;
    }
}
