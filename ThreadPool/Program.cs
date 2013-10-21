using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
	class Program
	{
		static void Main(string[] args)
		{
			MyClass myClass = new MyClass();

			//MiniThreadPool.RunInQueue(() => myClass.Write());
			MiniThreadPool.RunInQueue(new Action(myClass.Write), 2);

			Console.WriteLine("...");

			Console.Read();
		}
	}

	public class MyClass
	{
		private static readonly object syncLock = new object();
		private List<string> dict = new List<string>();

		public MyClass()
		{
			for(int i = 0; i < 10; i++)
			{
				this.dict.Add("Hallo");
				this.dict.Add("Welt");
				this.dict.Add("!");
				this.dict.Add("Ich");
				this.dict.Add("bin");
				this.dict.Add("eine");
				this.dict.Add("Testklasse");
				this.dict.Add("!");
			}
		}

		public void Write()
		{
			string entry = "";

			// hole ein eintrag
			lock(syncLock)
			{
				entry = this.dict.Grap(0);

				// tue etwas
				if(!string.IsNullOrWhiteSpace(entry))
				{
					Console.WriteLine(entry);
				}
				else
				{
					Console.WriteLine();
				}

				// end thread if list is empty
				if(this.dict.Count == 0)
				{
					MiniThreadPool.StopQueue();
				}
			}
		}
	}
}
