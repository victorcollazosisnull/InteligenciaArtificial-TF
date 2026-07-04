// HasTarget.cs
// FIX: chequea hasLastKnownPosition en vez de currentTarget.
// currentTarget se limpia cuando el enemigo desaparece del campo de vision,
// pero hasLastKnownPosition se mantiene hasta que el NPC llega al punto.
// Esto permite que el Orco vaya a buscar al enemigo donde lo vio por ultima vez
// en vez de quedarse quieto en idle.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class HasTarget : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null) return TaskStatus.Failure;
        return _Blackboard.hasLastKnownPosition
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}