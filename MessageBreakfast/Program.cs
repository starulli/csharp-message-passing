using System;
using System.Threading;
using MPL;
using Breakfast;

namespace MessageBreakfast
{
	class BreakfastItem<T> : Message
	{
		public T Item { get; }
		public BreakfastItem(T item) => Item = item;
	}

    class ToastMaker : Runnable
    {
    	private int count;
    	private ProcessId to;
    	private Toast toast;

    	public ToastMaker(ProcessId cook, int number)
    	{
    		count = number;
    		to = cook;
    	}

    	public class ToastPiece : Message
    	{
    		public Toast Piece { get; }
    		public ToastPiece(Toast t) => Piece = t;
    	}

		private static ToastPiece Toaster(int slices)
		{
			for (int slice = 0; slice < slices; slice++)
            {
		        Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Thread.Sleep(3000);
            Console.WriteLine("Remove toast from toaster");

            return new ToastPiece(new Toast());
		}

    	public override void Invoke(MessagingRuntime rt, Mailbox mbox)
    	{
    		rt.Spawn(Self, () => Toaster(count));
    		mbox.Receive();
            Console.WriteLine("Putting butter on the toast");
            Console.WriteLine("Putting jam on the toast");

            rt.Send(to, new BreakfastItem<Toast>(toast));
        }

        public void Receive(ToastPiece msg) => toast = msg.Piece;
    }

	class Cook : Runnable
	{
		private bool doneEggs = false,
					 doneBacon = false,
					 doneToast = false;

		public override void Invoke(MessagingRuntime rt, Mailbox mbox)
		{
			Console.WriteLine("Pouring coffee");
			Coffee cup = new Coffee();
			Console.WriteLine("Pouring Coffee");
			
			while (!doneEggs || !doneBacon || !doneToast)
			{
				mbox.Receive();
			}

			Console.WriteLine("Pouring orange juice");
            Juice oj = new Juice();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
		}

		public void Receive(BreakfastItem<Egg> eggs)
		{
			Console.WriteLine("eggs are ready");
			doneEggs = true;
		}

		public void Receive(BreakfastItem<Bacon> bacon)
		{
			Console.WriteLine("bacon is ready");
			doneBacon = true;
		}

		public void Receive(BreakfastItem<Toast> toast)
		{
			Console.WriteLine("toast is ready");
			doneToast = true;
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
        	var rt = new MessagingRuntime();
        	var cook = rt.Spawn(new Cook());

        	rt.Spawn(cook, () => EggFryer(2));
        	rt.Spawn(cook, () => BaconFryer(3));
        	rt.Spawn(new ToastMaker(cook, 2));

        	rt.Wait();
        }

    	private static BreakfastItem<Egg> EggFryer(int howMany)
    	{
            Console.WriteLine("Warming the egg pan...");
            Thread.Sleep(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Thread.Sleep(3000);
            Console.WriteLine("Put eggs on plate");
         
         	return new BreakfastItem<Egg>(new Egg());
        }

		private static BreakfastItem<Bacon> BaconFryer(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Thread.Sleep(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Thread.Sleep(3000);
            Console.WriteLine("Put bacon on plate");

            return new BreakfastItem<Bacon>(new Bacon());
        }
    }
}