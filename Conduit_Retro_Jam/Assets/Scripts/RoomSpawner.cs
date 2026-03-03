using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openSide;
    private RoomTemplates templates;
    public bool spawned = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        // Un tiempo ligeramente mayor ayuda a que las colisiones se registren mejor
        Invoke("Spawn", 0.4f);
    }

    void Spawn()
    {
        if (!spawned)
        {
            if (templates == null) return;

            // ... (Tu código de detección de colisiones se mantiene igual)

            GameObject roomToInstantiate = null;

            if (templates.rooms.Count < templates.maxRooms)
            {
                // CAMBIO AQUÍ: Conexiones opuestas
                if (openSide == 1)
                {
                    // Spawner mira ABAJO -> Necesita habitación con puerta ARRIBA
                    roomToInstantiate = templates.topRooms[Random.Range(0, templates.topRooms.Length)];
                }
                else if (openSide == 2)
                {
                    // Spawner mira ARRIBA -> Necesita habitación con puerta ABAJO
                    roomToInstantiate = templates.bottomRooms[Random.Range(0, templates.bottomRooms.Length)];
                }
                else if (openSide == 3)
                {
                    // Spawner mira IZQUIERDA -> Necesita habitación con puerta DERECHA
                    roomToInstantiate = templates.rigthRooms[Random.Range(0, templates.rigthRooms.Length)];
                }
                else if (openSide == 4)
                {
                    // Spawner mira DERECHA -> Necesita habitación con puerta IZQUIERDA
                    roomToInstantiate = templates.leftRooms[Random.Range(0, templates.leftRooms.Length)];
                }

                if (roomToInstantiate != null)
                {
                    Instantiate(roomToInstantiate, transform.position, roomToInstantiate.transform.rotation);
                    spawned = true;
                    Destroy(gameObject);
                    return;
                }
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpawner otherSpawner = other.GetComponent<RoomSpawner>();
            if (otherSpawner != null)
            {
                // Si dos puntos de spawn chocan, bloqueamos ambos para que no intenten
                // crear dos habitaciones en el mismo sitio exacto.
                if (!spawned && !otherSpawner.spawned)
                {
                    // Solo uno sobrevive para intentar el Spawn()
                    if (gameObject.GetInstanceID() < other.gameObject.GetInstanceID())
                    {
                        spawned = true;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void InstantiateClosedRoom(GameObject prefab)
    {
        float distance = 5f;
        Vector3 offset = Vector3.zero;

        if (openSide == 1) offset = new Vector3(0, 0, -distance);
        else if (openSide == 2) offset = new Vector3(0, 0, distance);
        else if (openSide == 3) offset = new Vector3(-distance, 0, 0);
        else if (openSide == 4) offset = new Vector3(distance, 0, 0);

        Vector3 spawnPos = transform.position + offset;

        // --- DETECCIÓN DE PASILLO ---
        // Si hay una habitación al otro lado, OverlapBox detectará sus colliders
        Collider[] colliders = Physics.OverlapBox(spawnPos, new Vector3(0.5f, 0.5f, 0.5f));
        bool pathwayIsClear = true;

        foreach (var col in colliders)
        {
            // Si detectamos geometría (no spawners), el camino ya está conectado
            if (!col.CompareTag("SpawnPoint") && col.gameObject != this.gameObject)
            {
                pathwayIsClear = false;
                break;
            }
        }

        // Si el camino está despejado (da al vacío), ponemos el bloque gris
        if (pathwayIsClear)
        {
            Instantiate(prefab, spawnPos, transform.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        float distance = 5f;
        Vector3 offset = Vector3.zero;

        if (openSide == 1) offset = new Vector3(0, 0, -distance);
        else if (openSide == 2) offset = new Vector3(0, 0, distance);
        else if (openSide == 3) offset = new Vector3(-distance, 0, 0);
        else if (openSide == 4) offset = new Vector3(distance, 0, 0);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, new Vector3(1f, 1f, 1f));
    }
}