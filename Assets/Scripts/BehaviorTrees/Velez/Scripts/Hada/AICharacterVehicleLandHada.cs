using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandHada : AICharacterVehicleLand
{
    private float _defaultSpeed;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (agent != null)
            _defaultSpeed = agent.speed;
    }
}
