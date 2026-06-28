// WeaponSwitcher.cs
// Activa/desactiva las armas del Cazador.
// Se pone en el mismo hijo que NPCAnimator.
// Melee y Ballesta empiezan desactivados — se activan solo al atacar.
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Armas")]
    public GameObject meleeWeapon;
    public GameObject ballestaWeapon;

    private void Awake()
    {
        // Ambas armas ocultas al inicio
        HideAll();
    }

    public void SetMelee()
    {
        if (meleeWeapon    != null) meleeWeapon.SetActive(true);
        if (ballestaWeapon != null) ballestaWeapon.SetActive(false);
    }
    public void SetBallesta()
    {
        if (meleeWeapon    != null) meleeWeapon.SetActive(false);
        if (ballestaWeapon != null) ballestaWeapon.SetActive(true);
    }
    public void HideAll()
    {
        if (meleeWeapon    != null) meleeWeapon.SetActive(false);
        if (ballestaWeapon != null) ballestaWeapon.SetActive(false);
    }
}
