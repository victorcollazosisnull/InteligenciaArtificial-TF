using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class EnemyVisible : Conditional
{
    private Blackboard _blackboard;
    private AIEyeNPC _aiEye;

    public override void OnAwake()
    {
        _blackboard = GetComponent<Blackboard>();
        _aiEye = GetComponent<AIEyeNPC>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_blackboard != null && _blackboard.currentTarget != null)
            return TaskStatus.Success;
        if (_aiEye != null && _aiEye.Enemy != null)
            return TaskStatus.Success;
        return TaskStatus.Failure;
    }
}
