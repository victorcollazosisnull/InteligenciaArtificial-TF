using UnityEngine;
namespace oldStructureVictorGozu
{
    public class HunterAI : AICharacterBase
    {
        protected override void Awake()
        {
            base.Awake();

            faction = Faction.Hunter;
        }
    }
}