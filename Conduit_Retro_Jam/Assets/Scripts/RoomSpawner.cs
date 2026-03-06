using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openSide;
    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        // 1. Si ya se marcó como spawned (por colisión en OnTriggerEnter), abortamos
        if (spawned) return;

        // 2. COMPROBACIÓN EXTRA: ¿Hay ya una habitación o algo aquí?
        // Creamos un radio pequeño (0.1f) para detectar si el espacio está ocupado
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            // Si encontramos algo que no sea este mismo SpawnPoint, cancelamos
            if (hitCollider.gameObject != this.gameObject)
            {
                spawned = true;
                return;
            }
        }

        // 3. Si el camino está despejado, procedemos a instanciar
        if (openSide == 1)
        {
            //Need Bottom Door
            rand = Random.Range(0, templates.bottomRooms.Length);
            Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
        }
        else if (openSide == 2)
        {
            //Need Top Door
            rand = Random.Range(0, templates.topRooms.Length);
            Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
        }
        else if (openSide == 3)
        {
            //Need Left Door
            rand = Random.Range(0, templates.leftRooms.Length);
            Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
        }
        else if (openSide == 4)
        {
            //Need Right Door
            rand = Random.Range(0, templates.rightRooms.Length);
            Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
        }

        // 4. Marcamos como finalizado
        spawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpawner otherSpawner = other.GetComponent<RoomSpawner>();

            if (templates == null)
            {
                templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            }

            if (otherSpawner != null && templates != null)
            {
                // CAMBIO AQUÍ: Solo el que tenga el ID de instancia menor (o una lógica de prioridad)
                // decide si poner la habitación cerrada, para evitar que ambos lo hagan.
                if (otherSpawner.spawned == false && spawned == false)
                {
                    // Verificamos si ya hay una habitación en esta posición exacta antes de crear la cerrada
                    // Esto evita que si una habitación real ya se puso, no se tape con una pared.
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            spawned = true; // Marcamos como ocupado para que el Invoke "Spawn" no haga nada
        }
    }
}
