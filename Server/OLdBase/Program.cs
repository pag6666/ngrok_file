
namespace OLdBase {
   internal partial class Pro 
    {
        static void Main() 
        {

            Server();
        tcp:
            if (Console.ReadLine() == "Close")
            {
               Environment.Exit(0);
            }
            else
            {
                goto tcp;
            }
        }
    }

}

