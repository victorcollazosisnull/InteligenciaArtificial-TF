using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsTargetInAttackRange : Conditional
{
    public float attackRange = 2.5f;
    private AIEyeNPC _aiEye;
    private Blackboard _blackboard;

    public override void OnAwake()
    {
        _aiEye = GetComponent<AIEyeNPC>();
        _blackboard = GetComponent<Blackboard>();
    }

    public override TaskStatus OnUpdate()
    {
        Transform target = null;
        if (_aiEye != null && _aiEye.Enemy != null)
            target = _aiEye.Enemy;
        else if (_blackboard != null && _blackboard.currentTarget != null)
            target = _blackboard.currentTarget;

        if (target == null) return TaskStatus.Failure;
        return Vector3.Distance(transform.position, target.position) <= attackRange
            ? TaskStatus.Success : TaskStatus.Failure;
    }
}
