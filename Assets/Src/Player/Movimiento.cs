using System.Collections;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float fuerzaMovimiento = 10f;
    public float fuerzaGiro = 100f;
    public float tiempoGiro = 0.2f;
    public float maxVelocidad = 5f;  // Maximum speed

    private bool enMovimiento = false;
    private bool girando = false;

    void Update()
    {
        // Handle movement
        if (!girando)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                enMovimiento = true;
                AplicarFuerza(transform.forward);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                enMovimiento = true;
                AplicarFuerza(-transform.forward);
            }
            else
            {
                enMovimiento = false;
                DetenerMovimiento();
            }
        }

        // Handle rotation
        if (!enMovimiento)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Girar(-90f);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Girar(90f);
            }
        }
    }

    void AplicarFuerza(Vector3 direccion)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direccion * fuerzaMovimiento);
        if (rb.velocity.magnitude > maxVelocidad)  // Limit speed
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
        if (!enMovimiento)  // Only rotate if not moving
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
    }
}
