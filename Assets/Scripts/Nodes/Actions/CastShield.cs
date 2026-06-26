using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Action")]
public class CastShield : ActionNode
{
    private AICharacterActionLandHada _hadaActions;

    public override void OnAwake()
    {
        base.OnAwake();
        _hadaActions = GetComponent<AICharacterActionLandHada>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_hadaActions == null) return TaskStatus.Failure;

        Transform ally = _AIEyeNPC != null && _AIEyeNPC.Allied != null
            ? _AIEyeNPC.Allied
            : (_Blackboard != null ? _Blackboard.allyTarget : null);

        if (ally == null) return TaskStatus.Failure;

        HealthBase hb = ally.GetComponent<HealthBase>();
        if (hb == null || hb.IsDead) return TaskStatus.Failure;

        return _hadaActions.CastShieldOn(hb) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
