// IsTargetInAttackRange.cs
// Retorna Success si el enemigo esta dentro del rango de ataque configurado.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsTargetInAttackRange : ActionNode
{
    public float attackRange = 2.5f;

    public override TaskStatus OnUpdate()
    {
        if (_AIEyeNPC == null || _AIEyeNPC.Enemy == null)
            return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position,
                                      _AIEyeNPC.Enemy.position);
        return dist <= attackRange ? TaskStatus.Success : TaskStatus.Failure;
    }
}
