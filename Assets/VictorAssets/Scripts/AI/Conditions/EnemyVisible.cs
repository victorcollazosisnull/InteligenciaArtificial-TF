using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyVisible : Conditional
{
    private Blackboard blackboard;

    public override void OnAwake()
    {
        blackboard = GetComponent<Blackboard>();
    }

    public override TaskStatus OnUpdate()
    {
        if (blackboard.currentTarget != null)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}