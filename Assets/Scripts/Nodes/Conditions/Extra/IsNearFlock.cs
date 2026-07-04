using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition")]
public class IsNearFlock : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        FlockSystemPato flock = GetComponent<FlockSystemPato>();
        if (flock == null) return TaskStatus.Failure;

        Vector3 pos = flock.GetFlockMovePosition();
        bool nearOthers = Vector3.Distance(pos, transform.position) > 0.5f;
        return nearOthers ? TaskStatus.Success : TaskStatus.Failure;
    }
}
