// FIX: usa _AIVehicleLand.StopMoving() en vez de GetComponent<AICharacterActionLand>
// ya que StopMoving ahora está en la rama Vehicle.
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

        // Limpia el Blackboard para que el Selector reevalúe
        _Blackboard.foodTarget = null;
        _Blackboard.currentTarget = null;
        _Blackboard.isHungry = false;

        // FIX: StopMoving ahora está en _AIVehicleLand
        if (_AIVehicleLand != null)
            _AIVehicleLand.StopMoving();

        Debug.Log($"[{gameObject.name}] comió y vuelve a patrullar.");
        return TaskStatus.Failure;
    }
}
