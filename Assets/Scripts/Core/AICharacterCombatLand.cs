// AICharacterCombatLand.cs
// Cuarta capa: NPC terrestre que puede combatir.
// Agrega el metodo Attack() que las subclases concretas
// (Duende, Cazador, Orco) sobreescriben con su logica propia.
// Los nodos del BT llaman Attack() sin conocer el tipo exacto de NPC.
using UnityEngine;

public class AICharacterCombatLand : AICharacterActionLand
{
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    /// <summary>
    /// Ejecuta el ataque del NPC. Cada subclase define como ataca:
    /// el orco golpea, el cazador dispara, el duende roba.
    /// </summary>
    public virtual void Attack()
    {
        Debug.Log($"[AICharacterCombatLand] {gameObject.name} ataca!");
    }
}
