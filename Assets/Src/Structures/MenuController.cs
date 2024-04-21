using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button[] menuOptions;
    public AudioClip[] buttonSounds; // Arreglo de clips de audio para cada botón
    private int selectedOption = 0;
    private AudioSource audioSource; // Referencia al componente AudioSource

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSelection(1);
        }
    }

    void ChangeSelection(int direction)
    {
        selectedOption += direction;
        if (selectedOption < 0)
        {
            selectedOption = menuOptions.Length - 1;
        }
        else if (selectedOption >= menuOptions.Length)
        {
            selectedOption = 0;
        }

        Debug.Log(selectedOption.ToString());

        // Reproducir el sonido del botón seleccionado
        audioSource.PlayOneShot(buttonSounds[selectedOption]);

        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (i == selectedOption)
            {
                menuOptions[i].Select();
            }
        }
    }
}
