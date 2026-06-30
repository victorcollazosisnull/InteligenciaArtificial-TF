using UnityEngine;

public class CycleDayNight : MonoBehaviour
{
    public Light sun;
    public Material skybox;
    [Tooltip("Duración en segundos de un día completo.")]
    public float cycleDuration = 30f;

    [Header("--- COLORES DEL DÍA ---")]
    public Color diaZenith = new Color(0.3f, 0.6f, 0.9f);
    public Color diaHorizon = new Color(0.7f, 0.85f, 1f);
    public Color diaFog = new Color(0.7f, 0.85f, 1f);
    public float diaIntensidadLuz = 1f;

    [Header("--- COLORES DEL ATARDECER ---")]
    public Color atardecerZenith = new Color(0.2f, 0.15f, 0.35f);
    public Color atardecerHorizon = new Color(0.95f, 0.5f, 0.2f);
    public Color atardecerFog = new Color(0.8f, 0.4f, 0.3f);
    public float atardecerIntensidadLuz = 0.6f;

    [Header("--- COLORES DE LA NOCHE ---")]
    public Color nocheZenith = new Color(0.02f, 0.02f, 0.1f);
    public Color nocheHorizon = new Color(0.05f, 0.05f, 0.15f);
    public Color nocheFog = new Color(0.02f, 0.02f, 0.08f);
    public float nocheIntensidadLuz = 0.05f;

    private float timer;

    private void Update()
    {
        // 1. Avanzar el tiempo (0.0 a 1.0)
        timer = (timer + Time.deltaTime) % cycleDuration;
        float timePercent = timer / cycleDuration;

        // FIX: sincroniza el TimeManager para que el BT del Orco sepa si es de noche
        if (TimeManager.Instance != null)
            TimeManager.Instance.SetTimePercent(timePercent);

        // 2. Rotar el sol de forma continua
        float angle = timePercent * 360f;
        sun.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        // Variables finales que enviaremos al shader/luz
        Color finalZenith;
        Color finalHorizon;
        Color finalFog;
        float finalIntensity;
        float tStars;
        float tAtmosphere;

        // 3. MÁQUINA DE TIEMPO ULTRA SUAVE (4 fases fijas de 25% de la duración del día cada una)
        if (timePercent < 0.25f)
        {
            float t = timePercent / 0.25f;
            if (t < 0.5f)
            {
                float subT = t / 0.5f;
                finalZenith = Color.Lerp(nocheZenith, atardecerZenith, subT);
                finalHorizon = Color.Lerp(nocheHorizon, atardecerHorizon, subT);
                finalFog = Color.Lerp(nocheFog, atardecerFog, subT);
                finalIntensity = Mathf.Lerp(nocheIntensidadLuz, atardecerIntensidadLuz, subT);
                tStars = Mathf.Lerp(1f, 0.4f, subT);
                tAtmosphere = Mathf.Lerp(1.0f, 1.2f, subT);
            }
            else
            {
                float subT = (t - 0.5f) / 0.5f;
                finalZenith = Color.Lerp(atardecerZenith, diaZenith, subT);
                finalHorizon = Color.Lerp(atardecerHorizon, diaHorizon, subT);
                finalFog = Color.Lerp(atardecerFog, diaFog, subT);
                finalIntensity = Mathf.Lerp(atardecerIntensidadLuz, diaIntensidadLuz, subT);
                tStars = Mathf.Lerp(0.4f, 0f, subT);
                tAtmosphere = Mathf.Lerp(1.2f, 0.6f, subT);
            }
        }
        else if (timePercent < 0.5f)
        {
            finalZenith = diaZenith;
            finalHorizon = diaHorizon;
            finalFog = diaFog;
            finalIntensity = diaIntensidadLuz;
            tStars = 0f;
            tAtmosphere = 0.6f;
        }
        else if (timePercent < 0.75f)
        {
            float t = (timePercent - 0.5f) / 0.25f;
            if (t < 0.5f)
            {
                float subT = t / 0.5f;
                finalZenith = Color.Lerp(diaZenith, atardecerZenith, subT);
                finalHorizon = Color.Lerp(diaHorizon, atardecerHorizon, subT);
                finalFog = Color.Lerp(diaFog, atardecerFog, subT);
                finalIntensity = Mathf.Lerp(diaIntensidadLuz, atardecerIntensidadLuz, subT);
                tStars = Mathf.Lerp(0f, 0.4f, subT);
                tAtmosphere = Mathf.Lerp(0.6f, 1.2f, subT);
            }
            else
            {
                float subT = (t - 0.5f) / 0.5f;
                finalZenith = Color.Lerp(atardecerZenith, nocheZenith, subT);
                finalHorizon = Color.Lerp(atardecerHorizon, nocheHorizon, subT);
                finalFog = Color.Lerp(atardecerFog, nocheFog, subT);
                finalIntensity = Mathf.Lerp(atardecerIntensidadLuz, nocheIntensidadLuz, subT);
                tStars = Mathf.Lerp(0.4f, 1f, subT);
                tAtmosphere = Mathf.Lerp(1.2f, 1.0f, subT);
            }
        }
        else
        {
            finalZenith = nocheZenith;
            finalHorizon = nocheHorizon;
            finalFog = nocheFog;
            finalIntensity = nocheIntensidadLuz;
            tStars = 1f;
            tAtmosphere = 1.0f;
        }

        sun.intensity = finalIntensity;

        if (skybox != null)
        {
            skybox.SetColor("_ZenithColor", finalZenith);
            skybox.SetColor("_HorizonColor", finalHorizon);
            skybox.SetColor("_HorizonFogColor", finalFog);
            skybox.SetFloat("_AtmosphereThickness", tAtmosphere);
            skybox.SetFloat("_EnableStars", tStars);
        }
    }
}
