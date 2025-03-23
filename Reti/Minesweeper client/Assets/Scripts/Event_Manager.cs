using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;

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

    public static Color[] BombsNumberToColors = { Color.gray, Color.green, Color.yellow, Color.red, Color.magenta, Color.cyan, Color.blue, Color.black }; //TODO: add another color for 8 bombs and change current colors

    public static void OnClicked(Position p)
    {
        Debug.Log($"Event Manager called with parameters {p.X},{p.Y}");
        Socket_Manager.MakeMove(p);
    }

    public static void OnServerResponse(Position move, Socket_Manager.ServerGameData data)
    {
        Debug.Log(data);
        if (data.HasLost)
        {
            Debug.Log("You lost!");
            GameObject.Find($"Square {move.X}, {move.Y}").gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            return;
        }
        foreach (Socket_Manager.ServerPosition pos in data.OpenedCells)
        {
            Debug.Log($"Square {pos.X}, {pos.Y}");
            GameObject.Find($"Square {pos.X}, {pos.Y}").gameObject.GetComponent<SpriteRenderer>().color = BombsNumberToColors[pos.Adjacent];
        }
        if (data.HasWon)
        {
            Debug.Log("You won!");
            return;
        }
    }

}
