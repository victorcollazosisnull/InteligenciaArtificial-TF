using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MoveToTarget : Action
{
    private Blackboard blackboard;
    private MovementController movement;

    public override void OnAwake()
    {
        blackboard = GetComponent<Blackboard>();
        movement = GetComponent<MovementController>();
    }

    public override TaskStatus OnUpdate()
    {
        if (blackboard.currentTarget == null)
        {
            return TaskStatus.Failure;
        }

        movement.MoveTo(blackboard.currentTarget.position);

        return TaskStatus.Running;
    }
}