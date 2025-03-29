using UnityEngine;

public class grid_block_click : MonoBehaviour
{
    public bool isOpen = false;
    private bool hasFlag = false;

    void OnMouseOver()
    {
        if(isOpen) return;

        if (Input.GetMouseButtonDown(0) && !hasFlag)
        {
            // get the coordinates from the name of the object
            string coords = gameObject.name.Substring(7);
            Event_Manager.Position p = new Event_Manager.Position(int.Parse(coords.Split(',')[0]), int.Parse(coords.Split(',')[1]));
            // invoke event manager
            Event_Manager.OnClicked(p);
        } 
        else if (Input.GetMouseButtonDown(1))
        {
            if(!hasFlag) 
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteManager.instance.flagSprite;
            else 
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteManager.instance.closedSprite;

            hasFlag = !hasFlag;
        }
    }

}
