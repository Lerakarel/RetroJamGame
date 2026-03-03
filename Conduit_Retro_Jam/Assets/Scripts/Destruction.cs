using UnityEngine;

public class Destruction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto NO tiene la etiqueta "suelo"
        // El símbolo '!' significa "no" o "es diferente a"
        if (!other.CompareTag("suelo") && !other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}