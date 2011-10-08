//
// Mono.Dns.ResolverTest
//
// Authors:
//	Gonzalo Paniagua Javier (gonzalo.mono@gmail.com)
//
// Copyright 2011 Gonzalo Paniagua Javier
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using Mono.Net.Dns;

namespace Mono.Net.Dns {
	static class ResolverTest {
		static int counter;
		static ManualResetEvent exit_event = new ManualResetEvent (false);

		static void Main (string [] args)
		{
			if (args.Length == 0)
				return;
			counter = args.Length;
			Stopwatch watch = new Stopwatch ();
			watch.Start ();
			SimpleResolver r = new SimpleResolver ();
			foreach (string s in args) {
				SimpleResolverEventArgs e = new SimpleResolverEventArgs ();
				e.Completed += OnCompleted;
				e.HostName = s;
				if (!r.GetHostEntryAsync (e))
					OnCompleted (e, e);
			}
			exit_event.WaitOne ();
			watch.Stop ();
			Console.WriteLine (watch.Elapsed);
		}

		static void OnCompleted (object sender, SimpleResolverEventArgs e)
		{
			IPHostEntry entry = e.HostEntry;
			if (entry == null) {
				Console.WriteLine ("HostName: {0} Error: {1} {2}", e.HostName, e.ResolverError, e.ErrorMessage);
				if (Interlocked.Decrement (ref counter) == 0)
					exit_event.Set ();
				return;
			}

			if (e.ResolverError != 0)
				Console.WriteLine ("HostName: {0} {1}", e.HostName, e.ResolverError);
			else
				Console.WriteLine ("HostName: {0}", e.HostName);

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
}

