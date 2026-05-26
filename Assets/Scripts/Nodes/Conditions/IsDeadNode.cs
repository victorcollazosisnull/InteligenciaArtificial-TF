// IsDeadNode.cs
// Retorna Success si el NPC esta muerto (IsDead = true).
// El BT lo usa como primera prioridad del Selector principal.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition/Health")]
public class IsDeadNode : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_AICharacterAction != null && _AICharacterAction.Health != null)
            return _AICharacterAction.Health.IsDead
                ? TaskStatus.Success : TaskStatus.Failure;

        Debug.LogWarning($"[IsDeadNode] {gameObject.name}: sin Health.");
        return TaskStatus.Failure;
    }
}
