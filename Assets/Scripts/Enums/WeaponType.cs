// WeaponType.cs
// Tipos de arma que puede recibir un NPC al ser atacado.
// HealthCombat lo usa para aplicar efectos diferenciados
// segun el tipo de dano recibido (fuego, fisico, magia, trampa).
public enum WeaponType
{
    None,
    Melee,      // golpe cuerpo a cuerpo (orco, duende)
    Arrow,      // flecha del cazador
    Trap,       // trampa colocada por el cazador
    Magic,      // magia del hada
    Fire        // dano de fuego (reservado para efectos especiales)
}
