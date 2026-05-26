using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition/Health")]
public class IsDangerHealthNode : Conditional
{
    private HealthBase _health;

    public override void OnAwake()
    {
        _health = GetComponent<HealthBase>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_health == null) return TaskStatus.Failure;
        return _health.IsDangerHealth ? TaskStatus.Success : TaskStatus.Failure;
    }
}
