using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ServerUDP : MonoBehaviour
{
    Socket socket;               // Socket UDP del servidor
    public GameObject UItextObj;  // UI per mostrar missatges
    TextMeshProUGUI UItext;       // Text per actualitzar la UI
    string serverText;            // Text a mostrar en la UI

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    public void StartServer()
    {
        // TO DO 1 - Creant i associant el socket UDP
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);  // Associar el socket

        serverText = "Starting UDP Server...";

        // TO DO 3 - Iniciant la recepció de missatges
        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
    }

    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;
        byte[] data = new byte[1024];

        while (true)
        {
            // Rebent missatges dels clients
            int recv = socket.ReceiveFrom(data, ref Remote);
            string message = Encoding.ASCII.GetString(data, 0, recv);
            serverText += "\nMessage received: " + message;

            // TO DO 4 - Enviant ping de resposta
            Thread sendThread = new Thread(() => Send(Remote));
            sendThread.Start();
        }
    }

    void Send(EndPoint Remote)
    {
        // Enviar un "ping" al client
        string ping = "Ping from Server";
        byte[] data = Encoding.ASCII.GetBytes(ping);
        socket.SendTo(data, Remote);
    }
}
