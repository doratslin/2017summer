using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class server
{
	public static void Main()
	{
		try
		{
			IPAddress ipAd = IPAddress.Parse("127.0.0.1");
			// use local m/c IP address, and 
			// use the same in the client

			/* Initializes the Listener */
			TcpListener myList = new TcpListener(ipAd, 8888);

			/* Start Listeneting at the specified port */
			myList.Start();

			Console.WriteLine("The server is running at port 8888...");
			Console.WriteLine("The local End point is  :" +
							  myList.LocalEndpoint);
			Console.WriteLine("Waiting for a connection.....");

			Socket s = myList.AcceptSocket();
			Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

			RSAServer rsa = new RSAServer();
			UTF8Encoding Asen = new UTF8Encoding();
			//sending N and D these two public key to client
			s.Send(Asen.GetBytes(Convert.ToString(rsa.getN())));
			s.Send(Asen.GetBytes(Convert.ToString(rsa.getD())));
			byte[] b;

			while (true)
			{
				b = new byte[80000];
				int k = s.Receive(b);
				char[] c = new char[k];
				//Console.WriteLine("Recieved... {0}", k);
				for (int i = 0; i < k; i++)
					c[i] = Convert.ToChar(b[i]);

				string S = new string(c);
				S = rsa.decryptMsg(S);
				Console.WriteLine("client send msg: '{0}'", S);

				if (string.Compare(S, "exit") == 0) break;

				UnicodeEncoding asen = new UnicodeEncoding();
				//s.Send(asen.GetBytes("The string was recieved by the server."));
				//Console.WriteLine("\nSent Acknowledgement");
			}

			/* clean up */
			s.Close();
			myList.Stop();

		}
		catch (Exception e)
		{
			Console.WriteLine("Error.....\n" + e.ToString());
		}
	}

}