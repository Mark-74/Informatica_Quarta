using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using System.Collections.Generic;

public class Socket_Manager : MonoBehaviour
{
    private static Socket sender;

    void Start()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ip = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ip, 11000);

        sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            sender.Connect(localEndPoint);
            Debug.Log("Socket connected to " + sender.RemoteEndPoint.ToString());
        }
        catch
        {
            Debug.Log("Unable to connect to remote server");
            sender = null;
        }
    }
    struct ClientGameData
    {
        public Event_Manager.Position Move { get; set; }
        public ClientGameData(Event_Manager.Position p)
        {
            Move = p;
        }
    }
    public struct ServerPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Adjacent { get; set; }

        public ServerPosition(int x, int y, int adjacent)
        {
            X = x;
            Y = y;
            Adjacent = adjacent;
        }
    }
    public struct ServerGameData
    {
        public bool HasWon { get; set; }
        public bool HasLost { get; set; }
        public List<ServerPosition> OpenedCells { get; set; }

        public ServerGameData(bool hasWon, bool hasLost, List<ServerPosition> openedCells)
        {
            HasWon = hasWon;
            HasLost = hasLost;
            OpenedCells = openedCells;
        }
    }

    private static string CreateJson(ClientGameData data)
    {
        return $"{{\"Move\":{{\"X\":{data.Move.X},\"Y\":{data.Move.Y}}}}}";
    }
    private static ServerGameData DecodeJson(string json)
    {
        return JsonUtility.FromJson<ServerGameData>(json); // Unity's built-in JSON decoder is broken bruh
    }

    public static void MakeMove(Event_Manager.Position p)
    {
        string json_data = CreateJson(new ClientGameData(p));
        Debug.Log(json_data);
        SendData(json_data);
        string json_response = ReceiveData();
        Debug.Log(json_response);
        ServerGameData response = DecodeJson(json_response);
        Debug.Log(response.OpenedCells.Count);
        Event_Manager.OnServerResponse(response.HasWon, response.HasLost, response.OpenedCells);
    }

    private static void SendData(string data)
    {
        byte[] msg = Encoding.ASCII.GetBytes(data);
        int bytesSent = sender.Send(msg);
    }

    private static string ReceiveData()
    {
        byte[] bytes = new byte[2048];
        int bytesRec = sender.Receive(bytes);
        return Encoding.ASCII.GetString(bytes, 0, bytesRec);
    }
}
