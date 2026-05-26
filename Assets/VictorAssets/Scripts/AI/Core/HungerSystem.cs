using UnityEngine;
namespace oldStructureVictorGozu
{
    public class HungerSystem : MonoBehaviour
    {
        public Blackboard blackboard;

        public float hungerIncreaseRate = 2f;

        public float maxHunger = 100f;

        public float hungryThreshold = 60f;

        private void Awake()
        {
            blackboard = GetComponent<Blackboard>();
        }

        private void Update()
        {
            blackboard.hunger += hungerIncreaseRate * Time.deltaTime;

            blackboard.hunger = Mathf.Clamp(blackboard.hunger, 0, maxHunger);

            blackboard.isHungry = blackboard.hunger >= hungryThreshold;
        }
    }
}