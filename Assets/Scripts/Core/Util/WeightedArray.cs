using System;
using System.Collections.Generic;

namespace Util
{
	public class WeightedArray<T>
	{
		public WeightedArray () : base()
		{
			Clear();
		}

		private List<int> _weights;
		private List<T> _elements;
		private int _totalWeight;

		public void Add(T element, int weight)
		{
			if (weight > 0)
			{
				_elements.Add(element);
				_weights.Add(weight);
				_totalWeight += weight;
			}
		}

		public void Remove(T element)
		{
			int index = _elements.FindIndex(x => x==element);
			if (index >= 0)
			{
				_elements.RemoveAt(index);
				_totalWeight -= _weights[index];
				_weights.RemoveAt(index);
			}
		}

		public T Choose()
		{
			if (_elements.Count == 0) return null;
			int value = (new Random()).Next(_totalWeight);
			int total = _totalWeight;
			for (int i=_elements.Count-1; i>=0; i--)
			{
				total -= _weights[i];
				if (value > total)
				{
					return _elements[i];
				}
			}
			return _elements[0];
		}

		public int Count
		{
			get { return _elements.Count; }
		}

		public void Clear()
		{
			_weights.Clear();
			_elements.Clear();
			_totalWeight = 0;
		}
	}
}