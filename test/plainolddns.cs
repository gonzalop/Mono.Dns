using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

class Test {
	static int counter;
	static ManualResetEvent exit_event = new ManualResetEvent (false);

	static void Main (string [] args)
	{
		counter = args.Length;
		Stopwatch watch = new Stopwatch ();
		watch.Start ();
		foreach (string s in args) {
			Dns.BeginGetHostEntry (s, OnCompleted, s);
		}
		exit_event.WaitOne ();
		watch.Stop ();
		Console.WriteLine (watch.Elapsed);
	}

	static void OnCompleted (IAsyncResult ares)
	{
		IPHostEntry entry = null;
		try {
			entry = Dns.EndGetHostEntry (ares);
		} catch {
			Console.WriteLine ("HostName: {0} Error: NoAnswer", ares.AsyncState);
		}

		if (entry == null) {
			if (Interlocked.Decrement (ref counter) == 0)
				exit_event.Set ();
			return;
		}

		Console.WriteLine ("HostName: {0}", ares.AsyncState);
		if (entry.HostName != null)
			Console.WriteLine ("\tHostName: {0}", entry.HostName);
		foreach (string alias in entry.Aliases) {
			Console.WriteLine ("\tAlias: {0}", alias);
		}
		foreach (IPAddress addr in entry.AddressList) {
			Console.WriteLine ("\tAddress: {0}", addr);
		}
		if (Interlocked.Decrement (ref counter) == 0)
			exit_event.Set ();
	}
}

