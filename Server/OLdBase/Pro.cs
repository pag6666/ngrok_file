using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace OLdBase
{
    internal partial class Pro
    {
        static List<string> user_list = new List<string>();
        static async void Server() 
        {
            Console.WriteLine("Server start");
            TcpListener server = new TcpListener(System.Net.IPAddress.Any, 25565);
            server.Start();
            await Task.Run(() => {
                while (true) 
                {
                   // error_ref:
                   TcpClient client =  server.AcceptTcpClient();
                    new Thread(ClientWork).Start(client);
                }
            
            });
        }
        static private string DirectWork(string command) 
        {
            string requst = "Unknown comand";
            if ("ls" == command)
            {
                requst = "ls al ==========================";
            }
            else if ("ls user" == command) 
            {
                requst = "";
                for (int i = 0; i < user_list.Count;i++) 
                {
                    requst+= " user = "+user_list[i];
                }
            }
            return requst;

        }
        static private void ClientWork(object obj) 
        {
            try
            {
                TcpClient client = obj as TcpClient;
                StreamReader read = new StreamReader(client.GetStream());
                StreamWriter write = new StreamWriter(client.GetStream());
                string user_name = read.ReadLine();
                user_list.Add(user_name);
                Console.WriteLine($"Client Connection {user_name}");
                //
                write.WriteLine("server read");
                write.Flush();
                while (true)
                {
                    if (client.GetStream().CanRead || client.GetStream().CanWrite)
                    {
                        string commamd = "";
                         commamd = read.ReadLine();
                        if (commamd == null)
                            commamd = "Close";
                        write.WriteLine(DirectWork(commamd));
                        write.Flush();
                        if (commamd.ToLower() == "close")
                        {
                            user_list.Remove(user_name);
                            break;
                        }
                    }

                }
                //

                read.Close();
                write.Close();
                client.Close();
                Console.WriteLine($"Client Close {user_name}");
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message);
               
            }
        }
    }
}
