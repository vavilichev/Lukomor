using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.Domain.Signals;

namespace Lukomor.Application.Signals
{
	public class SignalTower : ISignalTower
	{
		private readonly Dictionary<Type, List<ISignalObserver>> _signalObservers = new Dictionary<Type, List<ISignalObserver>>();
		
		public void Register<TSignal>(ISignalObserver<TSignal> observer) where TSignal : ISignal
		{
			var type = typeof(TSignal);

			_signalObservers.TryGetValue(type, out var list);

			if (list == null)
			{
				list = new List<ISignalObserver>();
				list.Add(observer);

				_signalObservers[type] = list;
			}
			else if (!list.Contains(observer))
			{
				list.Add(observer);
			}
		}

		public void Unregister<TSignal>(ISignalObserver<TSignal> observer) where TSignal : ISignal
		{
			var type = typeof(TSignal);

			_signalObservers.TryGetValue(type, out var list);

			list?.Remove(observer);
		}

		public void FireSignal<TSignal>(TSignal signal) where TSignal : ISignal
		{
			var type = typeof(TSignal);

			if (_signalObservers.ContainsKey(type))
			{
				List<ISignalObserver<TSignal>> list = _signalObservers[type].Cast<ISignalObserver<TSignal>>().ToList();
				
				var length = list.Count;

				for (int i = 0; i < length; i++)
				{
					list[i].ReceiveSignal(signal);
				}
			}
		}

		public void Clear()
		{
			_signalObservers.Clear();
		}
	}
}