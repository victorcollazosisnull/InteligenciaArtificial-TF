// IsNight.cs
// Retorna Success si TimeManager indica que es de noche.
// Usado por el Orco para decidir si va a dormir a la cueva.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsNight : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        if (TimeManager.Instance == null) return TaskStatus.Failure;
        return TimeManager.Instance.IsNight ? TaskStatus.Success : TaskStatus.Failure;
    }
}
