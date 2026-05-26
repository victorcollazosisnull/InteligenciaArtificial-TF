// Food.cs
// Componente que marca un objeto como comida en el escenario.
// FindFood lo busca por OverlapSphere y EatFood lo consume.
using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Comida")]
    public float nutritionValue = 50f;   // cuanto hambre reduce al comerla
    public string foodType      = "Generico";
}
