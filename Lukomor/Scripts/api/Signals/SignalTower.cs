using System;
using System.Collections.Generic;
using System.Linq;

namespace Lukomor.Signals
{
	public class SignalTower
	{
		private readonly Dictionary<Type, List<ISignalObserver>> observers = new Dictionary<Type, List<ISignalObserver>>();
		
		public void Register<TSignal>(ISignalObserver<TSignal> observer) where TSignal : ISignal
		{
			var type = typeof(TSignal);

			observers.TryGetValue(type, out var list);

			if (list == null)
			{
				list = new List<ISignalObserver>
				{
					observer
				};

				observers[type] = list;
			}
			else if (!list.Contains(observer))
			{
				list.Add(observer);
			}
		}

		public void Unregister<TSignal>(ISignalObserver<TSignal> observer) where TSignal : ISignal
		{
			var type = typeof(TSignal);

			observers.TryGetValue(type, out var list);

			list?.Remove(observer);
		}

		public void FireSignal<TSignal>(TSignal signal) where TSignal : ISignal
		{
			var type = typeof(TSignal);

			if (observers.ContainsKey(type))
			{
				List<ISignalObserver<TSignal>> list = observers[type].Cast<ISignalObserver<TSignal>>().ToList();
				
				var length = list.Count;

				for (int i = 0; i < length; i++)
				{
					list[i].ReceiveSignal(signal);
				}
			}
		}

		public void Clear()
		{
			observers.Clear();
		}
	}
}