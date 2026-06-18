// HungerSystem.cs
// Incrementa el hambre del NPC con el tiempo.
// Cuando supera el umbral, activa isHungry en el Blackboard
// para que el BehaviorTree priorice buscar comida.
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    private Blackboard blackboard;

    [Header("Configuracion")]
    public float hungerIncreaseRate = 2f;   // unidades de hambre por segundo
    public float maxHunger          = 100f;
    public float hungryThreshold;  // a partir de aqui isHungry = true

    private void Awake()
    {
        blackboard = GetComponent<Blackboard>();
    }

    private void Update()
    {
        blackboard.hunger  += hungerIncreaseRate * Time.deltaTime;
        blackboard.hunger   = Mathf.Clamp(blackboard.hunger, 0f, maxHunger);
        blackboard.isHungry = blackboard.hunger >= hungryThreshold;
    }

    /// <summary>Reduce el hambre al comer (llamado por EatFood).</summary>
    public void Feed(float nutritionValue)
    {
        blackboard.hunger =
        Mathf.Max(0f, blackboard.hunger - nutritionValue);

        blackboard.isHungry =
            blackboard.hunger >= hungryThreshold;

        Debug.Log("Hambre despues de comer: " + blackboard.hunger);
        Debug.Log("isHungry despues de comer: " + blackboard.isHungry);
    }
}
