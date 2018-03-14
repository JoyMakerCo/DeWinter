using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
	public class WeightedArray<T> : IList<T>
	{
		private List<T> _elements;
		private List<int> _weights;
		private int _totalWeight;

		public WeightedArray () : base()
		{
			_elements = new List<T>();
			_weights = new List<int>();
			_totalWeight = 0;
		}

		public WeightedArray(WeightedArray<T> collection)
		{
			_elements = new List<T>(collection._elements);
			_weights = new List<int>(collection._weights);
			_totalWeight = collection._totalWeight;
		}

		public int Count
		{
			get { return _elements.Count; }
		}

		public bool IsFixedSize { get { return false; } }
		public bool IsReadOnly { get { return false; } }
		public T this [int index] {
			get { return _elements[index]; }
			set { _elements[index] = value; }
		}

		public void Add (T value)
		{
			this.Add(value, 0);
		}

		public void Add (T value, int weight)
		{
			if (weight < 0) weight = 0;
			_elements.Add(value);
			_totalWeight += weight;
			_weights.Add(weight);
		}

		public void Clear ()
		{
			_elements.Clear();
			_weights.Clear();
			_totalWeight = 0;
		}

		public void CopyTo(T[] list, int index)
		{
			for(int i=Math.Min(index + Count, list.Length)-1; i>=0; i--)
			{
				list[i+index] = _elements[i];
			}
		}

		public bool Contains (T value)
		{
			return _elements.Contains(value);
		}

		public int IndexOf (T value)
		{
			return _elements.IndexOf(value);
		}

		public void Insert (int index, T value)
		{
			this.Insert(index, value, 0);
		}

		public void Insert (int index, T value, int weight)
		{
			_elements.Insert(index, value);
			if (weight < 0) weight = 0;
			_weights.Insert(index, weight);
			_totalWeight += weight;
		}

		public bool Remove(T item)
		{
			int i = this.IndexOf(item);
			if (i < 0) return false;
			_totalWeight -= _weights[i];
			_weights.RemoveAt(i);
			this.RemoveAt(i);
			return true;
		}

		public void RemoveAt (int index)
		{
			_elements.RemoveAt(index);
			_totalWeight -= _weights[index];
			_weights.RemoveAt(index);
		}

		public T Choose()
		{
			if (_elements.Count > 0)
			{
				int query = UnityEngine.Random.Range(0,_totalWeight);
				int value = _totalWeight;
				for (int i=_weights.Count-1; i>0; i--)
				{
					value -= _weights[i];
					if (value < query)
						return _elements[i];
				}
			}
			return _elements.FirstOrDefault();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)((IEnumerable<T>)this).GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}
}