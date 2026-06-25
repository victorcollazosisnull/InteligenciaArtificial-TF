// AICharacterControl.cs
// Clase raiz de la jerarquia de controles del NPC.
// Contiene la referencia al sensor de vision (AIEyeNPC).
// Toda la cadena de herencia de personajes parte de aqui.
using UnityEngine;

public class AICharacterControl : MonoBehaviour
{
    protected AIEyeNPC aIEyeNPC;
    public    AIEyeNPC AIEyeNPC => aIEyeNPC;
    protected virtual void Awake()
    {
        LoadComponent();
    }

    public virtual void LoadComponent()
    {
        aIEyeNPC = GetComponent<AIEyeNPC>();
    }
}
