using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsTargetInAttackRange : Conditional
{
    public float attackRange = 2f;

    private VisionSensor vision;

    public override void OnAwake()
    {
        vision = GetComponent<VisionSensor>();
    }

    public override TaskStatus OnUpdate()
    {
        if (vision.ViewPlayer == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(
            transform.position,
            vision.ViewPlayer.position);

        return distance <= attackRange
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}