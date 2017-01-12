using System;
using System.Collections.Generic;

public struct WeightedElement<T>
{
	public T Value;
	public int Weight;
	public WeightedElement(T value, int weight)
	{
		Value = value;
		Weight = weight;
	}
}

public class WeightedArray<T> : List<WeightedElement<T>>
{
	public WeightedArray () : base() {}
	public WeightedArray(int capacity) : base(capacity) {}
	public WeightedArray(IEnumerable<WeightedElement<T>> collection) : base(collection) {}

	public T Choose()
	{
		int value = 0;
		foreach (WeightedElement<T> elt in this)
		{
			value += elt.Weight;
		}
		value = (new Random()).Next(value);
		foreach (WeightedElement<T> elt in this)
		{
			if (value < elt.Weight)
			{
				return elt.Value;
			}
			value -= elt.Weight;
		}
		return this[Count-1].Value;
	}
}