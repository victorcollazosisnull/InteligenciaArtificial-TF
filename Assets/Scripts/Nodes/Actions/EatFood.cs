// EatFood.cs
// FIX: después de comer limpia currentTarget y foodTarget del Blackboard
// para que el Selector pueda reevaluar y el NPC retome la patrulla.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Food")]
public class EatFood : ActionNode
{
    private HungerSystem _hunger;

    public override void OnAwake()
    {
        base.OnAwake();
        _hunger = GetComponent<HungerSystem>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_Blackboard == null || _Blackboard.foodTarget == null)
            return TaskStatus.Failure;

        Food food = _Blackboard.foodTarget.GetComponent<Food>();
        if (food == null) return TaskStatus.Failure;

        // Reduce hambre
        if (_hunger != null) _hunger.Feed(food.nutritionValue);

        // Si es el Orco, llama su método Eat() para que se calme
        AICharacterActionLandOrco orco = GetComponent<AICharacterActionLandOrco>();
        if (orco != null) orco.Eat();

        // Destruye la comida
        Object.Destroy(_Blackboard.foodTarget.gameObject);

        // FIX: limpia el Blackboard para que el Selector reevalúe
        _Blackboard.foodTarget = null;
        _Blackboard.currentTarget = null;   // sin esto el NPC se queda "buscando" algo
        _Blackboard.isHungry = false; // limpiamos insHungry

        AICharacterActionLand land = GetComponent<AICharacterActionLand>();

        if (land != null)
            land.StopMoving();

        Debug.Log($"[{gameObject.name}] comió y vuelve a patrullar.");
        return TaskStatus.Failure; // ACA ESTABA EL ERROR TODO EL TIEMPO, SI DEVOLVIA SUCCESS, EL SELECTOR NO REEVALUABA Y EL NPC SE QUEDABA COMIENDO INFINITAMENTE
    }
}
