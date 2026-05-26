// IsDangerHealthNode.cs
// Retorna Success si el HP del NPC cayo por debajo del 50%.
// El BT lo usa para activar comportamiento defensivo (huir, evadir).
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition/Health")]
public class IsDangerHealthNode : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_AICharacterAction != null && _AICharacterAction.Health != null)
            return _AICharacterAction.Health.IsDangerHealth
                ? TaskStatus.Success : TaskStatus.Failure;

        Debug.LogWarning($"[IsDangerHealthNode] {gameObject.name}: sin Health.");
        return TaskStatus.Failure;
    }
}
