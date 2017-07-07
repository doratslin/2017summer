using System;
using System.IO;

public class RSAServer
{
	// this class is for encryption and decryption use
	//including main function, encryptMsg and decryptMsg

	private string path = @"first-3000-primes.txt";
	private static Random random = new Random();
	private static object syncLock = new object();
	private int pkey, qkey;
	private long N, totientN, E, D;

	public static int RandomNumber(int min, int max) // random number by time
	{
		lock (syncLock)
		{ // synchronize
			return random.Next(min, max);
		}
	}

	public static void swap<T>(ref T x, ref T y) // swap funciton
	{ 
		T t = y;
		y = x;
		x = t;
	}

	public static long gcd(long a, long b) // greatest common 
	{
		if (a < b) swap<long>(ref a, ref b);
		while ((a %= b)!= 0) swap<long>(ref a, ref b);
		return b;
	}

	public RSAServer() //main function
	{
		int[] primeNumI = new int[15003];
		int pcnt = 0;
		if (File.Exists(path))
		{
			StreamReader sr = File.OpenText(path);
			string s;
			while ((s = sr.ReadLine()) != null)
			{
				primeNumI[pcnt] = Convert.ToInt32(s);
				pcnt++;
			}// end sr readline
		}// end file exist path

		do
		{
			pkey = primeNumI[RandomNumber(0, pcnt - 1)];
			qkey = primeNumI[RandomNumber(0, pcnt - 1)];

			N = pkey * qkey;

			totientN = (pkey - 1) * (qkey - 1);

			Console.WriteLine("totientN is {0}", totientN);
		} while (totientN < 0);

		do {
			E = RandomNumber(1, (int)totientN);
		}
		while (gcd(E, totientN) != 1);

		long k = 1;
		while (true)
		{
			k = k + totientN;
			if (k > E && k % E == 0) break;
		}
		D = k / E;

		Console.WriteLine("D is {0}, E is {1}; pkey is {2}, qkey is {3}; N is {4}.", D, E, pkey, qkey, N);
	}// end RSAServer()

	public string decryptMsg(string S)
	{
		char[] s = new char[S.Length+1];
		long ct, k, i = 0;

		string[] SA = S.Split(' ');
		foreach(string str in SA)
		{
			if(!long.TryParse(str, out ct)) 
				Console.WriteLine("Wrong Format of string!");
			k = 1;
			for (int j = 0; j < E; j++)
			{
				k = k * ct;
				k = k % N;
			}
			if(k>0) s[i] = Convert.ToChar(k);
			//i++;
			i++;
		}
		string sstr = new string(s);
		return sstr;
	}// end decryptMsg

	public long getN()
	{
		return N;
	}

	public long getD()
	{
		return D;
	}
}// end class RSA
