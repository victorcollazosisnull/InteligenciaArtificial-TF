// FindFood.cs
// Busca la comida más cercana que corresponda al TypeAgent del NPC.
// El Orco busca carne (targetAgent=Orco), el Pato busca pan (targetAgent=Pato).
// El Duende puede robar la comida del Orco buscando targetAgent=Orco.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Food")]
public class FindFood : ActionNode
{
    public float searchRadius = 20f;

    [UnityEngine.Tooltip("Qué tipo de comida busca este NPC. " +
             "Duende pone Orco para robarle la comida.")]
    public TypeAgent foodFor = TypeAgent.Orco; // configura en el Inspector

    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null) return TaskStatus.Failure;

        Collider[] cols = Physics.OverlapSphere(transform.position, searchRadius);
        float closest = Mathf.Infinity;
        Transform closestFood = null;

        foreach (Collider col in cols)
        {
            Food food = col.GetComponent<Food>();
            if (food == null) continue;

            // Solo encuentra comida destinada a este NPC
            if (food.targetAgent != foodFor) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closest)
            {
                closest = dist;
                closestFood = col.transform;
            }
        }

        if (closestFood != null)
        {
            _Blackboard.foodTarget = closestFood;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
