using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;

public class ServerTCP : MonoBehaviour
{
    Socket socket;               // Socket del servidor
    Thread mainThread = null;     // Fil principal per acceptar connexions
    public GameObject UItextObj;  // Objecte UI per mostrar missatges
    TextMeshProUGUI UItext;       // Component de Text per mostrar el text de la UI
    string serverText;            // Text a mostrar en la UI

    public struct User
    {
        public string name;       // Nom del client
        public Socket socket;     // Socket del client
    }

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Actualitzar el text de la UI
        UItext.text = serverText;
    }

    public void StartServer()
    {
        // TO DO 1 - Crear i associar el socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket.Bind(ipep);  // Associar el socket a l'adreça i port
        socket.Listen(10);  // Posar el socket en mode d'escolta

        serverText = "Starting TCP Server...";

        // TO DO 3 - Comprovant noves connexions
        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while (true)
        {
            // Acceptar un nou client
            User newUser = new User();
            newUser.name = "";
            newUser.socket = socket.Accept();

            // TO DO 5 - Iniciant el fil de recepció de missatges
            Thread newConnection = new Thread(() => Receive(newUser));
            newConnection.Start();
        }
    }

    void Receive(User user)
    {
        byte[] data = new byte[1024];
        while (true)
        {
            // Rebre missatges del client
            int recv = user.socket.Receive(data);
            if (recv == 0) break;

            string message = Encoding.ASCII.GetString(data, 0, recv);
            serverText += "\n" + message;

            // TO DO 6 - Enviant resposta "ping"
            Send(user);  // Enviem el ping dins del mateix fil
        }
    }

    void Send(User user)
    {
        // Enviar un "ping" al client
        string pingMessage = "ping";
        byte[] data = Encoding.ASCII.GetBytes(pingMessage);
        user.socket.Send(data);
    }
}
