using UnityEngine;

public class grid_block_click : MonoBehaviour
{
    void OnMouseDown()
    {
        // get the coordinates from the name of the object
        string coords = gameObject.name.Substring(7);
        Event_Manager.Position p = new Event_Manager.Position(int.Parse(coords.Split(',')[0]), int.Parse(coords.Split(',')[1]));

        // invoke event manager
        Event_Manager.OnClicked(p);
    }

}
