// EnemyVisible.cs
// FIX: chequea enemyTarget en vez de currentTarget.
// El VisionSensor escribe en enemyTarget, no en currentTarget.
// currentTarget era un campo legacy que ya no se usa consistentemente.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class EnemyVisible : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        // Primero chequea el Blackboard (escrito por VisionSensor)
        if (_Blackboard != null && _Blackboard.enemyTarget != null)
            return TaskStatus.Success;

        // Fallback: chequea AIEyeNPC directamente
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
        {
            // Sincroniza el Blackboard si no lo estaba
            if (_Blackboard != null)
                _Blackboard.enemyTarget = _AIEyeNPC.Enemy;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
