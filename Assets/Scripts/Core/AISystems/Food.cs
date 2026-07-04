// Food.cs
// Componente que marca un objeto como comida en el escenario.
// targetAgent define qué NPC puede comer esta comida.
// FindFood filtra por targetAgent para que cada NPC solo vea su comida.
using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Comida")]
    public float nutritionValue = 50f;
    public string foodType = "Generico";

    [Header("Para quien es esta comida")]
    public TypeAgent targetAgent = TypeAgent.Orco; // cambia en el Inspector
}
