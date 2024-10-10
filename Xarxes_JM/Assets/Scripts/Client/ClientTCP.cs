using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientTCP : MonoBehaviour
{
    public GameObject UItextObj; // UI text object per mostrar missatges
    TextMeshProUGUI UItext;      // Component de Text per actualitzar el text de la UI
    string clientText;           // Text a mostrar en la UI
    Socket server;               // Socket del servidor

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Actualitzar el text de la UI amb els missatges rebuts
        UItext.text = clientText;
    }

    public void StartClient()
    {
        // Crear un fil per connectar al servidor
        Thread connect = new Thread(Connect);
        connect.Start();
    }

    void Connect()
    {
        // TO DO 2 - Creant el socket i connectant amb el servidor
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Connect(ipep);  // Connectar al servidor

        // TO DO 4 - Enviant el missatge al servidor
        Thread sendThread = new Thread(Send);
        sendThread.Start();

        // TO DO 7 - Iniciant la recepció del missatge
        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
    }

    void Send()
    {
        // TO DO 4 - Enviant un missatge codificat
        string message = "Hello, server!";
        byte[] data = Encoding.ASCII.GetBytes(message);
        server.Send(data);  // Enviar el missatge al servidor
    }

    void Receive()
    {
        // TO DO 7 - Rebent el missatge del servidor
        byte[] data = new byte[1024];
        int recv = server.Receive(data);  // Rebre dades del servidor
        clientText = Encoding.ASCII.GetString(data, 0, recv);  // Convertir les dades rebudes en text
    }
}
