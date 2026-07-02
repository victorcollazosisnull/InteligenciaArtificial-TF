using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class Caution : ActionNode
{
    public SharedVector3 targetPosition;
    public float stopDistance = 4.5f;

    private AICharacterVehicleLandUnicorn _unicorn;

    public override void OnAwake()
    {
        base.OnAwake();
        _unicorn = GetComponent<AICharacterVehicleLandUnicorn>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_unicorn == null) return TaskStatus.Failure;

        Vector3 destination = targetPosition.Value;
        if (destination == Vector3.zero)
        {
            Transform enemy = _AIEyeNPC != null && _AIEyeNPC.Enemy != null
                ? _AIEyeNPC.Enemy
                : (_Blackboard != null ? _Blackboard.enemyTarget : null);
            if (enemy == null) return TaskStatus.Failure;
            destination = enemy.position;
        }

        float dist = Vector3.Distance(transform.position, destination);
        if (dist <= stopDistance)
        {
            _unicorn.StopMoving();
            _unicorn.ResetSpeed();
            return TaskStatus.Success;
        }

        _unicorn.MoveCautiously(destination);
        return TaskStatus.Running;
    }
}
