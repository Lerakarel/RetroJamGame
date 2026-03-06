using UnityEngine;

public class Destruction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Solo destruye si el objeto que entró es un punto de generación
        // y NO es el jugador ni el suelo.
        if (other.CompareTag("SpawnPoint") || other.CompareTag("ClosedRoom"))
        {
            Destroy(other.gameObject);
        }
    }
}
