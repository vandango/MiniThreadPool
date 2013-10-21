using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
	public static class MiniThreadPool
	{
		private static readonly object syncLock = new object();

		private static int numberOfThreads;
		private static bool done = false;
		private static Action internalAction;

		// Statischer Konstruktor
		static MiniThreadPool()
		{
			int workerThreads = 0;
			int completionPortThreads = 0;

			System.Threading.ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);

			numberOfThreads = workerThreads;
		}

		// Startet die Aufgabe in der Queue
		public static void RunInQueue(Action action, int threads)
		{
			RunInQueue(action, false, threads);
		}

		public static void RunInQueue(Action action)
		{
			RunInQueue(action, false, numberOfThreads);
		}

		public static void RunInQueue(Action action, bool waitToEnd)
		{
			RunInQueue(action, waitToEnd, numberOfThreads);
		}

		public static void RunInQueue(Action action, bool waitToEnd, int threads)
		{
			internalAction = action;

			List<Thread> threadList = new List<Thread>();

			// Erstellt pro gewünschter Threadanzahl einen Thread in der internen Queue
			for(int i = 0; i < threads; i++)
			{
				Thread thread = new Thread(new ParameterizedThreadStart(RunThread));
				threadList.Add(thread);

				if(!done)
				{
					thread.Start(thread);

					Thread.Sleep(12);
				}
				else
				{
					i = threads;
				}
			}

			// Optional kann man auf die Fertigstellung aller Threads warten
			if(waitToEnd)
			{
				while(ThreadsRunning(threadList))
				{
					Thread.Sleep(TimeSpan.FromMilliseconds(1.0));
				}
			}
		}

		// Queue vorzeitig beenden
		public static void StopQueue()
		{
			done = true;
		}

		// Stellt fest, ob noch Threads laufen und aktiv sind
		private static bool ThreadsRunning(List<Thread> threadList)
		{
			bool running = false;

			foreach(Thread thread in threadList)
			{
				running = running || thread.IsAlive;
			}

			return running;
		}

		// Jeder einzelne Thread führt diese Funktion aus und hält den eigenen Thread am leben
		private static void RunThread(object threadContext)
		{
			while(!done)
			{
				try
				{
					Thread thread = (Thread)threadContext;
					RunCheck(thread);
				}
				catch
				{
				}
			}
		}

		// Führe nun die Action aus und halte den Thread am leben
		private static void RunCheck(Thread thread)
		{
			try
			{
				internalAction.Invoke();

				// Kurz warten, nachdem die Funktion ausgeführt wurde
				Thread.Sleep(12);
			}
			catch
			{
			}
		}
	}
}
