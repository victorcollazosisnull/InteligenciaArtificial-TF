// AICharacterActions.cs
// Segunda capa de la jerarquia de controles.
// Representa cualquier NPC que se mueve a pie (no vehiculo).
// Agrega la referencia al Health del NPC para que los nodos
// del BT puedan leer IsDead e IsDangerHealth.
using UnityEngine;

public class AICharacterActions : AICharacterControl
{
    protected HealthIA health;
    public    HealthIA Health => health;

    public override void LoadComponent()
    {
        health = GetComponent<HealthIA>();
        base.LoadComponent();
    }
}
