// IsEnemyOfType.cs
// Retorna Success si el enemigo detectado es de alguno de los TypeAgent especificados.
// En el Inspector puedes agregar multiples tipos en el array: Pato, Unicornio, etc.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsEnemyOfType : ConditionNode
{
    public TypeAgent[] targetTypes;  // pon aqui: [Pato, Unicornio] o [Orco]

    public override TaskStatus OnUpdate()
    {
        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.enemyTarget != null)
            enemy = _Blackboard.enemyTarget;

        if (enemy == null) return TaskStatus.Failure;

        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth == null) return TaskStatus.Failure;

        foreach (TypeAgent t in targetTypes)
            if (targetHealth.typeAgent == t)
                return TaskStatus.Success;

        return TaskStatus.Failure;
    }
}