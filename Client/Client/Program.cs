using System.Net;
using System.Net.Sockets;
class Pro 
{
    
  

    static void Main()
    {

        connection:
        const string connectionSource = @"https://raw.githubusercontent.com/pag6666/ngrok_file/main/ngrok_server_connection";
        string host = "null";
            int port = 0;
        using (WebClient webclin = new WebClient())
        {

            
            Stream stream = webclin.OpenRead(connectionSource);
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
           
            Console.WriteLine("address: "+content);
            string connectionLine = content;
            string[] hoat_and_port = connectionLine.Split(':');
            host = hoat_and_port[0];
            port = int.Parse((hoat_and_port[1]));
                 
        }
            //
            Console.WriteLine($"host = {host} port = {port}");
            TcpClient client = new TcpClient(host, port);
            Console.WriteLine("Client success connected");
        try
        {
            StreamWriter write = new StreamWriter(client.GetStream());
            StreamReader read = new StreamReader(client.GetStream());
            if (client.Connected && client.GetStream().CanRead || client.GetStream().CanWrite) {
                Console.WriteLine("Input Name");
                string user_name = Console.ReadLine();
                write.WriteLine(user_name);
                write.Flush();
                //
                while (client.Connected)
                {
                    string answer_server = read.ReadLine();

                    Console.WriteLine($"server: {answer_server}");
                    if (answer_server != "server: hello " + user_name + " server limit") {
                        Console.WriteLine("Input command");
                        string command_send = Console.ReadLine();
                        write.WriteLine(command_send);
                        write.Flush();
                        if (command_send.ToLower() == "close")
                        {
                            break;
                        }
                        else if (command_send.ToLower() == "clear")
                        {
                            Console.Clear();
                        }
                    }
                }
                //
                read.Close();
                write.Close();
                client.Close();
                Console.WriteLine("Client Close");
            }
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
            goto connection;
        }
    }
}