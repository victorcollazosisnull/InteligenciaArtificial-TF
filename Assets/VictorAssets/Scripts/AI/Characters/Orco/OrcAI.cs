using UnityEngine;
namespace oldStructureVictorGozu
{
    public class OrcAI : AICharacterBase
    {
        protected override void Awake()
        {
            base.Awake();

            faction = Faction.Orc;
        }
    }
}