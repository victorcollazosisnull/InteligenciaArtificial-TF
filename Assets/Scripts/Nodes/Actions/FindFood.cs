// Busca el objeto de comida mas cercano dentro del radio de busqueda.
// Si lo encuentra lo registra en Blackboard.foodTarget y retorna Success.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Food")]
public class FindFood : ActionNode
{
    public float searchRadius = 20f;

    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null) return TaskStatus.Failure;

        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        float     closest = Mathf.Infinity;
        Transform closestFood = null;

        foreach (Collider col in colliders)
        {
            Food food = col.GetComponent<Food>();
            if (food == null) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closest)
            {
                closest     = dist;
                closestFood = col.transform;
            }
        }

        if (closestFood != null)
        {
            _Blackboard.foodTarget = closestFood;
            return TaskStatus.Success;
        }
        _Blackboard.foodTarget = null;
        return TaskStatus.Failure;
    }
}
