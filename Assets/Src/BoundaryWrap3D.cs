using UnityEngine;

public class BoundaryWrap3D : MonoBehaviour
{
    // Define los límites del área jugable en 3D
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;
    public float minZ = -10f;
    public float maxZ = 10f;

    void Update()
    {
        Vector3 newPosition = transform.position;

        // Verificar y ajustar la posición horizontal si sale de los límites
        if (newPosition.x > maxX)
        {
            newPosition.x = minX;
        }
        else if (newPosition.x < minX)
        {
            newPosition.x = maxX;
        }

        // Verificar y ajustar la posición vertical si sale de los límites
        if (newPosition.y > maxY)
        {
            newPosition.y = minY;
        }
        else if (newPosition.y < minY)
        {
            newPosition.y = maxY;
        }

        // Verificar y ajustar la posición en z si sale de los límites
        if (newPosition.z > maxZ)
        {
            newPosition.z = minZ;
        }
        else if (newPosition.z < minZ)
        {
            newPosition.z = maxZ;
        }

        // Aplicar la nueva posición
        transform.position = newPosition;
    }
}
