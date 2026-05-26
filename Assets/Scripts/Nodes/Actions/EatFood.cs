// EatFood.cs
// El NPC come la comida registrada en Blackboard.foodTarget.
// Reduce el hambre y destruye el objeto de comida.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Food")]
public class EatFood : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null || _Blackboard.foodTarget == null)
            return TaskStatus.Failure;

        Food food = _Blackboard.foodTarget.GetComponent<Food>();
        if (food == null) return TaskStatus.Failure;

        // Reduce hambre via HungerSystem
        HungerSystem hs = GetComponent<HungerSystem>();
        if (hs != null) hs.Feed(food.nutritionValue);

        // Si el NPC es un Orco, llama su metodo Eat() para calmarlo
        AICharacterActionLandOrco orco = GetComponent<AICharacterActionLandOrco>();
        if (orco != null) orco.Eat();

        Object.Destroy(_Blackboard.foodTarget.gameObject);
        _Blackboard.foodTarget = null;

        Debug.Log($"[{gameObject.name}] comio.");
        return TaskStatus.Success;
    }
}
