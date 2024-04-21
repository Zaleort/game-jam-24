using UnityEngine;
using FMODUnity;  // Importar el espacio de nombres de FMOD Unity

public class Repairable : MonoBehaviour
{
    public bool canRepare = false;
    public int reparedLevel = 0;
    public int repairIncrement = 1;
    public bool isRepaired = false;
    public float repairInterval = 1000f;
    private float repairTimer = 0;
    private float spaceCooldown = 0.5f;
    private float lastSpaceTime = -1f;

    [SerializeField]
    [EventRef]
    public EventReference repairSoundEvent;  // Evento de sonido para reparación
    [SerializeField]
    [EventRef]
    public EventReference repairedSoundEvent;  // Evento de sonido cuando la reparación está completa

    private FMOD.Studio.EventInstance repairSoundInstance;
    private FMOD.Studio.EventInstance repairedSoundInstance;  // Instancia para el sonido de objeto reparado

    private bool isSoundPlaying = false;

    private void Start()
    {
        repairSoundInstance = RuntimeManager.CreateInstance(repairSoundEvent.Guid);
        repairedSoundInstance = RuntimeManager.CreateInstance(repairedSoundEvent.Guid);  // Crear instancia del nuevo evento
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRepare = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRepare = false;
            StopRepairSound();
        }
    }

    private void Update()
    {
        if (canRepare && !isRepaired)  // Añadido !isRepaired para evitar acciones si el objeto ya está reparado
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (Time.time - lastSpaceTime > spaceCooldown)
                {
                    lastSpaceTime = Time.time;
                    if (repairTimer <= 0)
                    {
                        repairTimer = repairInterval / 1000f;
                        RepararObjeto();
                    }
                    if (!isSoundPlaying)
                    {
                        StartRepairSound();
                    }
                }
            }
            else
            {
                StopRepairSound();
            }

            if (repairTimer > 0)
            {
                repairTimer -= Time.deltaTime;
            }
        }
        else
        {
            StopRepairSound();
        }
    }

    void StartRepairSound()
    {
        repairSoundInstance.start();
        isSoundPlaying = true;
    }

    void StopRepairSound()
    {
        if (isSoundPlaying)
        {
            repairSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            isSoundPlaying = false;
        }
    }

    void RepararObjeto()
    {
        reparedLevel += repairIncrement;
        if (reparedLevel >= 100)
        {
            reparedLevel = 100;
            isRepaired = true;
            StopRepairSound();  // Detener el sonido de reparación
            repairedSoundInstance.start();  // Iniciar el sonido de objeto reparado
            Debug.Log("Objeto reparado completamente!");
        }
    }

    private void OnDestroy()
    {
        repairSoundInstance.release();
        repairedSoundInstance.release();  // Liberar la instancia del nuevo evento de sonido
    }
}
