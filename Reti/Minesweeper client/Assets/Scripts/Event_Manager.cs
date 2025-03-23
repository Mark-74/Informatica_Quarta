using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

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

    public static void OnServerResponse(Position move, Socket_Manager.ServerGameData data)
    {
        Debug.Log(data);
        if(data.HasWon)
        {
            Debug.Log("You won!");
            return;
        }
        if (data.HasLost)
        {
            Debug.Log("You lost!");
            GameObject.Find($"Square {move.X}, {move.Y}").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            return;
        }
        foreach (Socket_Manager.ServerPosition pos in data.OpenedCells)
        {
            Debug.Log($"Square {pos.X}, {pos.Y}");
            GameObject.Find($"Square {pos.X}, {pos.Y}").gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

}
