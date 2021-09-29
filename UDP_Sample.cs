using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDP_Sample : MonoBehaviour
{

    string iPAddress = "127.0.0.1";
    int portNum = 20433;
    static UdpClient udpClient;

    [Header("UDP Sender")]
    [SerializeField]
    bool useSend = false;

    [Header("UDP Receiver")]
    [SerializeField]
    bool useReceive = false;
    [SerializeField]
    bool showLatestMessageOnly = true;

    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        UDP_Start();
    }


    void UDP_Start()
    {
        udpClient = new UdpClient(portNum);
    }

    // Update is called once per frame
    void Update()
    {
        if(useSend) Sender();
        if(useReceive) Receiver();

    }

    void Sender()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(iPAddress), portNum);


        string sendMessage = ($"hello world {counter++}");       
        byte[] datagram = Encoding.ASCII.GetBytes(sendMessage);
        
        print($"udp send message: {sendMessage}");


        udpClient.Send(datagram, datagram.Length, remoteEP);
    }

    void Receiver()
    {
        IPEndPoint remoteEP = null;


        GetMessage(remoteEP);

    }

    void GetMessage(IPEndPoint  remoteEP)
    {
        if (udpClient.Available > 0)
        {

            byte[] data = udpClient.Receive(ref remoteEP);

            string message = Encoding.ASCII.GetString(data);

            if (udpClient.Available == 0)
            {
                ///this is the last message
                print($"udp receive message: {message}");
                return;
            }

           
            if (!showLatestMessageOnly)
            {
                ///this is the old messages
                print($"udp receive old message: {message}");
            }

            GetMessage(remoteEP);

        }
        else
        {
            print("udp client not available");
        }
    }


    private void OnDestroy()
    {
        udpClient.Close();
    }
}
