using UnityEngine;
using System.Collections.Generic;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rigthRooms;
    public GameObject closedRoom;

    // IMPORTANTE: Inicializar la lista para evitar NullReferenceException
    public List<GameObject> rooms = new List<GameObject>();

    public GameObject boss;
    public GameObject simpleEnemies;
    public int maxRooms = 15; // Límite para el bucle infinito

    private void Start()
    {
        // 1. Primero spawneamos los enemigos y el jefe
        Invoke("SpawnEnemies", 5f);
        // 2. Al final, cerramos cualquier puerta que haya quedado abierta
        Invoke("FinalClosure", 7f);
    }

    void SpawnEnemies()
    {
        // Seguridad: Si no hay habitaciones, no hacemos nada
        if (rooms.Count == 0) return;

        // Spawneamos al jefe en la ÚLTIMA habitación de la lista
        Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);

        // Spawneamos enemigos en el resto (menos en la primera y la última)
        for (int i = 1; i < rooms.Count - 1; i++)
        {
            Instantiate(simpleEnemies, rooms[i].transform.position, Quaternion.identity);
        }
    }

    void FinalClosure()
    {
        RoomSpawner[] allSpawners = Object.FindObjectsByType<RoomSpawner>(FindObjectsSortMode.None);

        foreach (RoomSpawner spawner in allSpawners)
        {
            // Si el spawner sigue vivo aquí, es porque NO creó una habitación
            if (closedRoom != null)
            {
                // Usamos la función de desplazamiento que creamos antes
                spawner.InstantiateClosedRoom(closedRoom);
            }
            Destroy(spawner.gameObject);
        }
    }
}