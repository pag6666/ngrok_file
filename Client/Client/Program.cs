using System.Net;
using System.Net.Sockets;
class Pro 
{
   static string connectionSource = "https://raw.githubusercontent.com/pag6666/ngrok_file/main/ngrok_server_connection";
    
    static string path_connection = "conection.init";

    static void Main()
    {
       
            connection:
            string host = "null";
            int port = 0;
            WebClient webckin = new WebClient();
            webckin.DownloadFile(connectionSource, path_connection);
            if (File.Exists(path_connection)) {
                StreamReader readfile = new StreamReader(File.Open(path_connection, FileMode.Open));
                string connectionLine = readfile.ReadLine();
                string[] hoat_and_port = connectionLine.Split(':');
                host = hoat_and_port[0];
                port = int.Parse((hoat_and_port[1]));
                readfile.Close();
            }
            //
            Console.WriteLine($"host = {host} port = {port}");
            TcpClient client = new TcpClient(host, port);
            Console.WriteLine("Client success connected");
        try
        {
            StreamWriter write = new StreamWriter(client.GetStream());
            StreamReader read = new StreamReader(client.GetStream());
            Console.WriteLine("Input Name");
            write.WriteLine(Console.ReadLine());
            write.Flush();
            //
            while (true)
            {
                string answer_server = read.ReadLine();

                Console.WriteLine($"server: {answer_server}");
                Console.WriteLine("Input command");
                string command_send = Console.ReadLine();
                write.WriteLine(command_send);
                write.Flush();
                if (command_send.ToLower() == "close")
                {
                    break;
                }
            }
            //
            read.Close();
            write.Close();
            client.Close();
            Console.WriteLine("Client Close");
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
            goto connection;
        }
    }
}