using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;


public class client
{
	public static void Main()
	{

		try
		{
			TcpClient tcpclnt = new TcpClient();
			Console.WriteLine("Connecting.....");

			tcpclnt.Connect("127.0.0.1", 8888);
			// use the ipaddress as in the server program

			Console.WriteLine("Connected");

			Stream stm = tcpclnt.GetStream();
			String str;

			UTF8Encoding asen = new UTF8Encoding();
			byte[] bb = new byte[1000], bb2 = new byte[1000];
			int lenOfMsg = stm.Read(bb, 0, 1000);
			int lenOfbb2 = stm.Read(bb2, 0, 1000);

			RSAClient rsa = new RSAClient(bb, bb2);

			Console.WriteLine("\n* * * * *");

			while(true)
			{
				Console.Write("Enter the string to be transmitted : ");	
				str = Console.ReadLine();
				//stm = tcpclnt.GetStream();
				//UnicodeEncoding asen = new UnicodeEncoding();
				byte[] ba = asen.GetBytes(rsa.encryptMsg(str));
				//Console.WriteLine("Transmitting.....");

				stm.Write(ba, 0, ba.Length);

				//this part is for getting msg from server.

				//bb = new byte[100];
				//lenOfMsg = stm.Read(bb, 0, 100);

				//for (int i = 0; i < lenOfMsg; i++)
				//	Console.Write(Convert.ToChar(bb[i]));

				if (string.Compare(str, "exit") == 0)
					break;
			}

			tcpclnt.Close();
		}

		catch (Exception e)
		{
			Console.WriteLine("Error..... " + e.StackTrace);
		}
	}



}