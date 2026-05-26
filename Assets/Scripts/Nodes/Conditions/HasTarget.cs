using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class HasTarget : Conditional
{
    private Blackboard _blackboard;

    public override void OnAwake()
    {
        _blackboard = GetComponent<Blackboard>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_blackboard == null || _blackboard.currentTarget == null)
            return TaskStatus.Failure;
        return TaskStatus.Success;
    }
}
