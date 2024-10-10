using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    Socket socket;               // Socket UDP del client
    public GameObject UItextObj;  // UI per mostrar missatges
    TextMeshProUGUI UItext;       // Text per actualitzar la UI
    string clientText;            // Text a mostrar en la UI

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    public void StartClient()
    {
        // Crear un fil per enviar el missatge
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        // Actualitzar el text de la UI
        UItext.text = clientText;
    }

    void Send()
    {
        // TO DO 2 - Creant i associant el socket
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // TO DO 2.1 - Enviant handshake
        string handshake = "Hello UDP Server";
        byte[] data = Encoding.ASCII.GetBytes(handshake);
        socket.SendTo(data, ipep);  // Enviar el handshake

        // TO DO 5 - Iniciant la recepció del missatge
        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
    }

    void Receive()
    {
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        // Rebent la resposta del servidor
        int recv = socket.ReceiveFrom(data, ref Remote);
        clientText = Encoding.ASCII.GetString(data, 0, recv);
    }
}
