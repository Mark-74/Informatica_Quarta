using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using System.Linq;

public class Socket_Manager : MonoBehaviour
{
    private static Socket sender;

    private void Awake()
    {
        sender = null;
    }

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
            Application.Quit();
        }
    }

    public static void CloseConnection()
    {
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }

    public struct ClientGameData
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

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Adjacent: {Adjacent}";
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

        public override string ToString()
        {
            return $"HasWon: {HasWon}, HasLost: {HasLost}, OpenedCells: {OpenedCells}";
        }
    }

    private static string CreateJson(ClientGameData data)
    {
        return $"{{\"Move\":{{\"X\":{data.Move.X},\"Y\":{data.Move.Y}}}}}";
    }
    private static ServerGameData DecodeJson(string json)
    {
        ServerGameData result = new ServerGameData();
        result.HasWon = json.Contains("HasWon\":true");
        result.HasLost = json.Contains("HasLost\":true");
        result.OpenedCells = new List<ServerPosition>();
        int idx_l =  json.IndexOf("["); int idx_r = json.IndexOf("]");
        json = json.Substring(idx_l+1, idx_r - idx_l-1);
        while (json.Length > 0)
        {
            
            int idx_l2 = json.IndexOf("{"); int idx_r2 = json.IndexOf("}");
            string current = json.Substring(idx_l2, idx_r2 - idx_l2 + 1);
            result.OpenedCells.Add(new ServerPosition(int.Parse("" + current[current.IndexOf('X') + 3]), int.Parse("" + current[current.IndexOf('Y') + 3]), int.Parse("" + current[current.Length-2])));

            json = json.Substring(idx_r2 + 1);
        }

        return result;
    }

    public static void MakeMove(Event_Manager.Position p)
    {
        string json_data = CreateJson(new ClientGameData(p));
        SendData(json_data);
        string json_response = ReceiveData();
        ServerGameData response = DecodeJson(json_response);
        Event_Manager.OnServerResponse(p, response);
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
