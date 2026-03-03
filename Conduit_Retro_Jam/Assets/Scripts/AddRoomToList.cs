using UnityEngine;

public class AddRoomToList : MonoBehaviour
{
    private RoomTemplates templates;

    void Start()
    {
        GameObject roomsObj = GameObject.FindGameObjectWithTag("Rooms");
        if (roomsObj != null)
        {
            templates = roomsObj.GetComponent<RoomTemplates>();
            templates.rooms.Add(this.gameObject);
        }
        else
        {
            Debug.LogError("¡No se encontró un objeto con el tag 'Rooms'!");
        }
    }
}