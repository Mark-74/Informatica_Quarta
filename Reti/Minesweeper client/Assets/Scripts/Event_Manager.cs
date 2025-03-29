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

    public static void OnClicked(Position p)
    {
        Socket_Manager.MakeMove(p);
    }

    public static void OnServerResponse(Position move, Socket_Manager.ServerGameData data)
    {
        if (data.HasLost)
        {
            Debug.Log("You lost!");
            GameObject.Find($"Square {move.X}, {move.Y}").gameObject.GetComponent<SpriteRenderer>().sprite = spriteManager.instance.mineSprite;
            return;
        }
        foreach (Socket_Manager.ServerPosition pos in data.OpenedCells)
        {
            GameObject.Find($"Square {pos.X}, {pos.Y}").gameObject.GetComponent<SpriteRenderer>().sprite = spriteManager.instance.numberSprites[pos.Adjacent];
        }
        if (data.HasWon)
        {
            Debug.Log("You won!");
            return;
        }
    }

}
