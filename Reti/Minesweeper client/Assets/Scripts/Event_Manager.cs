using UnityEngine;

public class Event_Manager : MonoBehaviour
{
    public struct Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public static void OnClicked(Position p)
    {
        Debug.Log($"Event Manager called with parameters {p.X},{p.Y}");
    }

}
