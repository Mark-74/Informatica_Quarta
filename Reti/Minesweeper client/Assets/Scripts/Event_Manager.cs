using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;
using System.Collections;

public class Event_Manager : MonoBehaviour
{
    public Canvas EndScreen;
    public static bool Status;
    private static Event_Manager instance;
    void Awake()
    {
        instance = this;
        Status = true;
    }

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
            instance.StartCoroutine(ShowCanvas(false));
            return;
        }
        foreach (Socket_Manager.ServerPosition pos in data.OpenedCells)
        {
            GameObject.Find($"Square {pos.X}, {pos.Y}").gameObject.GetComponent<SpriteRenderer>().sprite = spriteManager.instance.numberSprites[pos.Adjacent];
        }
        if (data.HasWon)
        {
            Debug.Log("You won!");
            instance.StartCoroutine(ShowCanvas(true));
            return;
        }
    }

    public static IEnumerator ShowCanvas(bool hasWon)
    {
        Status = false;
        Socket_Manager.CloseConnection();
        Debug.Log("Connection closed");
        yield return new WaitForSeconds(2);

        instance.EndScreen.enabled = true;
        instance.EndScreen.gameObject.SetActive(true); // Ensure the canvas is active

        instance.EndScreen.transform.Find("Title_Won").gameObject.SetActive(hasWon);
        instance.EndScreen.transform.Find("Title_Lost").gameObject.SetActive(!hasWon);
    }

}
