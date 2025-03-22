using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
        Socket_Manager.MakeMove(p);
    }

    public static void OnServerResponse(bool hasWon, bool hasLost, List<Socket_Manager.ServerPosition> openedCells)
    {
        Debug.Log(openedCells.Count);
        foreach (Socket_Manager.ServerPosition pos in openedCells)
        {
            Debug.Log($"Square {pos.X}, {pos.Y}");
            GameObject.Find($"Square {pos.X}, {pos.Y}").gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

}
