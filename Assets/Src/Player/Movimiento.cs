using System.Collections;
using UnityEngine;
using FMODUnity;  // Importa el namespace de FMODUnity

public class Movimiento : MonoBehaviour
{
    public float fuerzaMovimiento = 10f;
    public float fuerzaGiro = 100f;
    public float tiempoGiro = 0.2f;
    public float maxVelocidad = 5f;  // Velocidad máxima

    private bool enMovimiento = false;
    private bool girando = false;

    // Referencias a los eventos de sonido de FMOD usando EventReference
    public EventReference sonidoMovimientoAdelante;
    public EventReference sonidoMovimientoAtras;
    public EventReference sonidoGiroDerecha;
    public EventReference sonidoGiroIzquierda;

    private FMOD.Studio.EventInstance instanciaMovimientoAdelante;
    private FMOD.Studio.EventInstance instanciaMovimientoAtras;
    private FMOD.Studio.EventInstance instanciaGiroDerecha;
    private FMOD.Studio.EventInstance instanciaGiroIzquierda;

    void Start()
    {
        // Crear instancias de los eventos de FMOD usando RuntimeManager.CreateInstance con EventReference
        instanciaMovimientoAdelante = RuntimeManager.CreateInstance(sonidoMovimientoAdelante);
        instanciaMovimientoAtras = RuntimeManager.CreateInstance(sonidoMovimientoAtras);
        instanciaGiroDerecha = RuntimeManager.CreateInstance(sonidoGiroDerecha);
        instanciaGiroIzquierda = RuntimeManager.CreateInstance(sonidoGiroIzquierda);

    }

    void Update()
    {
        // Manejo del movimiento
        if (!girando)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (!enMovimiento)
                {
                    instanciaMovimientoAdelante.start();  // Iniciar sonido de movimiento adelante
                    enMovimiento = true;
                }
                AplicarFuerza(transform.forward);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (!enMovimiento)
                {
                    instanciaMovimientoAtras.start();  // Iniciar sonido de movimiento atrás
                    enMovimiento = true;
                }
                AplicarFuerza(-transform.forward);
            }
            else
            {
                if (enMovimiento)
                {
                    instanciaMovimientoAdelante.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);  // Detener sonido adelante
                    instanciaMovimientoAtras.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);  // Detener sonido atrás
                    enMovimiento = false;
                }
                DetenerMovimiento();
            }
        }

        // Manejo de la rotación
        if (!enMovimiento)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                instanciaGiroIzquierda.start();  // Iniciar sonido de giro a la izquierda
                Girar(-90f);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                instanciaGiroDerecha.start();  // Iniciar sonido de giro a la derecha
                Girar(90f);
            }
        }
    }

    void AplicarFuerza(Vector3 direccion)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direccion * fuerzaMovimiento);
        if (rb.velocity.magnitude > maxVelocidad)
        {
            rb.velocity = rb.velocity.normalized * maxVelocidad;
        }
    }

    void DetenerMovimiento()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Girar(float angulo)
    {
        if (!enMovimiento)
        {
            StartCoroutine(EjecutarGiro(angulo));
        }
    }

    IEnumerator EjecutarGiro(float angulo)
    {
        girando = true;
        Quaternion inicioRotacion = transform.rotation;
        Quaternion objetivoRotacion = Quaternion.Euler(transform.eulerAngles + new Vector3(0, angulo, 0));

        float tiempoPasado = 0f;
        while (tiempoPasado < tiempoGiro)
        {
            float rotacionInterpolada = Mathf.SmoothStep(0f, 1f, tiempoPasado / tiempoGiro);
            transform.rotation = Quaternion.Slerp(inicioRotacion, objetivoRotacion, rotacionInterpolada);
            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        transform.rotation = objetivoRotacion;
        girando = false;
        // Detener el sonido de giro adecuado
        if (angulo > 0)
        {
            instanciaGiroDerecha.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            instanciaGiroIzquierda.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
