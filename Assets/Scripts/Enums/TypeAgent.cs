// TypeAgent.cs
// Define los bandos posibles de cada entidad en el juego.
// El BehaviorTree y el VisionSensor lo usan para distinguir
// aliados de enemigos sin conocer el tipo concreto de NPC.
public enum TypeAgent
{
    Duende,
    Cazador,
    Orco,
    Pato,
    Unicornio,
    Hada,
    Neutral
}
