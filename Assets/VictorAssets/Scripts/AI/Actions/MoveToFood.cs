using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MY_AI / Food")]
public class MoveToFood : Action
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
        if (blackboard.foodTarget == null)
        {
            return TaskStatus.Failure;
        }

        movement.MoveTo(
            blackboard.foodTarget.position);

        if (movement.HasReachedDestination())
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}