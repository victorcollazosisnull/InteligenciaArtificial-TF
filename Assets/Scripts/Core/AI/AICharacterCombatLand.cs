// AICharacterCombatLand.cs
// Capa de COMBATE terrestre.
// Hereda de AICharacterActionLand (rama de acciones).
// Agrega Attack() que cada NPC concreto sobreescribe.
using UnityEngine;

public class AICharacterCombatLand : AICharacterActionLand
{
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public virtual void Attack()
    {
        Debug.Log($"[AICharacterCombatLand] {gameObject.name} ataca!");
    }
}
 