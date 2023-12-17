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
        static int limit_cone = 0;
        static List<string> user_list = new List<string>();
        static async void Server() 
        {
            int server_max_limit = 100;
            Console.WriteLine("Server start");
            TcpListener server = new TcpListener(System.Net.IPAddress.Any, 25565);
            server.Start();
            if (limit_cone <= 3)
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        if (limit_cone < server_max_limit)
                        {
                            // error_ref:
                            
                            new Thread(ClientWork).Start(client);
                        }
                        else 
                        {
                            new Thread(WaitConnect).Start(client);
                        }
                    }

                });
            }
            
        }
        static private void WaitConnect(object obj)
        {
            TcpClient client = obj as TcpClient;
            StreamReader read = new StreamReader(client.GetStream());
            StreamWriter write = new StreamWriter(client.GetStream());
            string user_name = read.ReadLine();
            write.WriteLine("hello "+user_name+" server limit");
            write.Flush();
            read.Close();
            write.Close();
            client.Close();
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
                for (int i = 0; i < user_list.Count; i++)
                {
                    requst += " user = " + user_list[i];
                }
            }
            else if ("help" == command)
            {
                requst = "ls | ls user | clear | count user | help";
            }
            else if ("clear" == command)
            {
                requst = "Clear";
            }
            else if ("count user" == command) 
            {
               requst = (limit_cone - 1).ToString();
            }

            return requst;

        }
        static private void ClientWork(object obj) 
        {
            try
            {
                limit_cone++;
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
                        write.WriteLine(DirectWork(commamd.ToLower()));
                        write.Flush();
                        if (commamd.ToLower() == "close")
                        {
                            user_list.Remove(user_name);
                            break;
                        }
                    }

                }
                //
                limit_cone--;
                read.Close();
                write.Close();
                client.Close();
                Console.WriteLine($"Client Close {user_name}");
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message);
                user_list.Remove("");
               
            }
        }
    }
}
