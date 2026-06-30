using UnityEngine;

public class FuzzyInferenceController : MonoBehaviour
{
    private AICharacterActionLandUnicorn _action;

    void Awake()
    {
        _action = GetComponent<AICharacterActionLandUnicorn>();
    }

    public float GetTendenciaActual()
    {
        return _action != null ? _action.GetTendenciaActual() : 50f;
    }
}
