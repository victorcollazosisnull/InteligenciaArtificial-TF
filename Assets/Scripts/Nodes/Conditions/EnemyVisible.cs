// EnemyVisible.cs
// Retorna Success si AIEyeNPC tiene un enemigo detectado.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class EnemyVisible : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_AIEyeNPC == null || _AIEyeNPC.Enemy == null)
            return TaskStatus.Failure;
        return TaskStatus.Success;
    }
}
