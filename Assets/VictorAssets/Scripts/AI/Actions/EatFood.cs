using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace oldStructureVictorGozu
{
    [TaskCategory("MY_AI / Food")]
    public class EatFood : Action
    {
        private Blackboard blackboard;

        public override void OnAwake()
        {
            blackboard = GetComponent<Blackboard>();
        }

        public override TaskStatus OnUpdate()
        {
            if (blackboard.foodTarget == null)
            {
                return TaskStatus.Failure;
            }

            Food food =
                blackboard.foodTarget.GetComponent<Food>();

            if (food == null)
            {
                return TaskStatus.Failure;
            }

            blackboard.hunger -= food.nutritionValue;

            if (blackboard.hunger < 0)
            {
                blackboard.hunger = 0;
            }

            blackboard.isHungry = false;

            GameObject.Destroy(blackboard.foodTarget.gameObject);

            blackboard.foodTarget = null;

            Debug.Log("Orco comió comida");

            return TaskStatus.Failure;
        }
    }
}