// AICharacterVehicleLandPato.cs
// Acciones específicas para el Pato (Deambular y Huir).
using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandPato : AICharacterVehicleLand
{
    [Header("Configuración")]
    [SerializeField] private float eatRange = 2f;

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public void EatFood()
    {
        Blackboard bb = GetComponent<Blackboard>();
        if (bb == null || bb.foodTarget == null) return;

        float dist = Vector3.Distance(transform.position,
                                      bb.foodTarget.position);
        if (dist > eatRange) return;

        Debug.Log("Pato esta comiendo");
        Destroy(bb.foodTarget.gameObject);
        bb.foodTarget = null;

        HungerSystem hs = GetComponent<HungerSystem>();
        if (hs != null) hs.Feed(80f);
    }
}