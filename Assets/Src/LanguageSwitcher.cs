using UnityEngine;
using FMODUnity;
using FMOD.Studio; // Importe necesario para manejar los buses y otros elementos de FMOD Studio.

public class LanguageSwitcher : MonoBehaviour
{
    public GameObject[] languages; // Array para almacenar los GameObjects de idiomas
    private GameObject currentActiveLanguage;
    private bool isSwitching = false; // Para bloquear cambios durante una activación

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isSwitching) ToggleLanguage(languages[0], "event:/path/to/soundA");
        if (Input.GetKeyDown(KeyCode.S) && !isSwitching) ToggleLanguage(languages[1], "event:/path/to/soundS");
        if (Input.GetKeyDown(KeyCode.D) && !isSwitching) ToggleLanguage(languages[2], "event:/path/to/soundD");
        // Añade más teclas según necesites
    }

    void ToggleLanguage(GameObject selectedLanguage, string soundEventPath)
    {
        if (currentActiveLanguage == selectedLanguage)
        {
            // Si se intenta reactivar el mismo idioma, simplemente retorna
            return;
        }

        isSwitching = true;

        if (currentActiveLanguage != null)
        {
            currentActiveLanguage.SetActive(false);
            // Detener todos los sonidos asociados al idioma anterior
            Bus masterBus = RuntimeManager.GetBus("bus:/"); // Ajusta esta ruta al bus maestro o específico que desees.
            masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        selectedLanguage.SetActive(true);
        currentActiveLanguage = selectedLanguage;

        // Opcional: reproducir un sonido específico para el idioma seleccionado
        RuntimeManager.PlayOneShot(soundEventPath);

        // Desactivar el GameObject que lleva este script después de cambiar de idioma
        gameObject.SetActive(false);

        isSwitching = false;
    }
}
