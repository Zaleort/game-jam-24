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
    public EventReference repairSoundEvent;
    [SerializeField]
    public EventReference repairedSoundEvent;
    [SerializeField]
    public EventReference reparableNoticeEvent;

    public GameObject soundToDeactivate;  // Objeto que ser� desactivado cuando se repare
    public GameObject soundToActivate;    // Objeto que ser� activado cuando se repare
    public GameObject balizaToDeactivate;  // Objeto que ser� desactivado cuando se repare
    public GameObject balizaToActivate;    // Objeto que ser� activado cuando se repare

    private FMOD.Studio.EventInstance repairSoundInstance;
    private FMOD.Studio.EventInstance repairedSoundInstance;
    private FMOD.Studio.EventInstance reparableNoticeInstance;

    private bool isSoundPlaying = false;

    private void OnEnable()
    {
     

        if (gameObject.CompareTag("Nave"))
        {
            canRepare = false;
            reparedLevel = 0;
            repairIncrement = 25;
            isRepaired = false;
            repairInterval = 1000f;
            repairTimer = 0;
            spaceCooldown = 0.5f;
            lastSpaceTime = -1f;
        }
        else {
            canRepare = false;
            reparedLevel = 0;
            repairIncrement = 10;
            isRepaired = false;
            repairInterval = 1000f;
            repairTimer = 0;
            spaceCooldown = 0.5f;
            lastSpaceTime = -1f;
        }
    }


    private void Start()
    {
        repairSoundInstance = RuntimeManager.CreateInstance(repairSoundEvent.Guid);
        repairedSoundInstance = RuntimeManager.CreateInstance(repairedSoundEvent.Guid);
        reparableNoticeInstance = RuntimeManager.CreateInstance(reparableNoticeEvent.Guid);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRepaired)
        {
            canRepare = true;
            reparableNoticeInstance.start();
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
        if (canRepare && !isRepaired)
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
            StopRepairSound();
            repairedSoundInstance.start();  // Reproducir el sonido de objeto reparado
            Debug.Log("Objeto reparado completamente!");
            if (soundToDeactivate != null) {
                
                if (this.gameObject.CompareTag("Nave"))
                {
                    StopAllFMODSounds();
                }
                soundToDeactivate.SetActive(false);  // Desactivar el objeto seleccionado
            }
            

            if (soundToActivate != null)
                soundToActivate.SetActive(true);     // Activar el otro objeto seleccionado
            if (balizaToActivate != null)
                balizaToActivate.SetActive(true);    // Comprobaci�n de null para evitar errores
            if (balizaToDeactivate != null)
                balizaToDeactivate.SetActive(false); // Comprobaci�n de null para evitar errores
        }
    }
    void StopAllFMODSounds()
    {
        // Detiene todos los eventos de FMOD que est�n actualmente reproduci�ndose
        FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/"); // Obt�n el bus maestro
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Detiene todos los eventos en el bus maestro con un fade out
    }

    private void OnDestroy()
    {
        repairSoundInstance.release();
        repairedSoundInstance.release();
        reparableNoticeInstance.release();
    }
}
