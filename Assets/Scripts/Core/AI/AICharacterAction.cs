// AICharacterAction.cs
// Rama de ACCIONES — segunda capa desde AICharacterControl.
// Contiene la referencia al Health del NPC.
// Los NPCs que combaten heredan de aqui.
using UnityEngine;

public class AICharacterAction : AICharacterControl
{
    protected HealthIA health;
    public    HealthIA Health => health;

    public override void LoadComponent()
    {
        health = GetComponent<HealthIA>();
        base.LoadComponent();
    }
}
