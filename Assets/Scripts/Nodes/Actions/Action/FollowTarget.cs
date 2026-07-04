using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Action")]
public class FollowTarget : ActionNode
{
    public float followTime = 30f;
    public float stopDistance = 2f;

    private float _startTime;

    public override void OnStart()
    {
        _startTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        Transform ally = _AIEyeNPC != null && _AIEyeNPC.Allied != null
            ? _AIEyeNPC.Allied
            : (_Blackboard != null ? _Blackboard.allyTarget : null);

        if (ally == null) return TaskStatus.Failure;

        if (Time.time - _startTime >= followTime)
            return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, ally.position);
        if (dist > stopDistance)
            _AIVehicleLand.MoveTo(ally.position);
        else
            _AIVehicleLand.StopMoving();

        return TaskStatus.Running;
    }
}
