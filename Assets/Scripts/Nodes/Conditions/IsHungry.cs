using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsHungry : Conditional
{
    private Blackboard _blackboard;

    public override void OnAwake()
    {
        _blackboard = GetComponent<Blackboard>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_blackboard == null) return TaskStatus.Failure;
        return _blackboard.isHungry ? TaskStatus.Success : TaskStatus.Failure;
    }
}
