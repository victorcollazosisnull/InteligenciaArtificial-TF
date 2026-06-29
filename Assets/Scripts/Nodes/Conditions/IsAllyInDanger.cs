using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsAllyInDanger : ConditionNode
{
    public float healthPercentThreshold = 100f;

    public override TaskStatus OnUpdate()
    {
        Transform ally = _AIEyeNPC != null && _AIEyeNPC.Allied != null
            ? _AIEyeNPC.Allied
            : (_Blackboard != null ? _Blackboard.allyTarget : null);

        if (ally == null) return TaskStatus.Failure;

        HealthBase hb = ally.GetComponent<HealthBase>();
        if (hb == null || hb.IsDead) return TaskStatus.Failure;

        Debug.Log("Ally Health Percent: " + hb.HealthPercent);
        return hb.HealthPercent < healthPercentThreshold
            ? TaskStatus.Success : TaskStatus.Failure;
    }
}
