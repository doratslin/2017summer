using System;
using System.IO;

public class RSAClient
{
	// this class is for encryption and decryption use
	//including main function, encryptMsg and decryptMsg
	private long N, E;

	public RSAClient(byte[] B1, byte[] B2) //main function
	{
		Console.WriteLine("New RSA client");
		// From byte array to string
		string sN = System.Text.Encoding.UTF8.GetString(B1, 0, B1.Length);
		string sE = System.Text.Encoding.UTF8.GetString(B2, 0, B2.Length);
		N = Convert.ToInt32(sN);
		E = Convert.ToInt32(sE);
		Console.WriteLine("E is {0}; N is {1}.", E, N);
	}// end RSAClient()


	public string encryptMsg(string msg)
	{
		long pt, k;
		int len, i = 0;
		int[] en = new int[msg.Length+1];
		string str;
		len = msg.Length;
		while (i < len)
		{
			pt = Convert.ToInt32(msg[i]);
			k = 1;
			for (int j = 0; j < E; j++)
			{
				k = k * pt;
				k = k % N;
			}
			en[i] = (int)k;
			i++;
		}

		str = String.Join(" ", en);
		return str;
	}// end encryptMsg


	public string decryptMsg(string S)
	{
		char[] s = new char[S.Length + 1];
		long ct, k, i = 0;

		string[] SA = S.Split(' ');
		foreach (string str in SA)
		{
			if (!long.TryParse(str, out ct))
				Console.WriteLine("Wrong Format of string!");
			k = 1;
			for (int j = 0; j < E; j++)
			{
				k = k * ct;
				k = k % N;
			}
			if (k > 0) s[i] = Convert.ToChar(k);
			//i++;
			i++;
		}
		string sstr = new string(s);
		return sstr;
	}// end decryptMsg

}// end class RSA