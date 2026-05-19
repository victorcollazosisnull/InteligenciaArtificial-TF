using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MY_AI / Food")]
public class FindFood : Action
{
    private Blackboard blackboard;

    public float searchRadius = 20f;

    public override void OnAwake()
    {
        blackboard = GetComponent<Blackboard>();
    }

    public override TaskStatus OnUpdate()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, searchRadius);

        float closestDistance = Mathf.Infinity;

        Transform closestFood = null;

        foreach (Collider collider in colliders)
        {
            Food food = collider.GetComponent<Food>();

            if (food == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                collider.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFood = collider.transform;
            }
        }

        if (closestFood != null)
        {
            blackboard.foodTarget = closestFood;

            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}