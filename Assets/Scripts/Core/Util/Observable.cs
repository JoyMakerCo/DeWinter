using System;
using System.Collections;

public class Observable<T>
{
	protected T _value;

	protected event Action<T> _notifier;

	public Observable() {}
	public Observable(T t)
	{
		Set(t);
	}

	public static implicit operator T (Observable<T> t)
	{
		return t._value;
	}

	public T Set(T Value)
	{
		_value = Value;
		_notifier.Invoke(_value);
		return _value;
	}

	public void Observe(Action<T> listener)
	{
		_notifier += listener;
		listener(_value);
	}

	public void Remove(Action<T> listener)
	{
		_notifier -= listener;
	}
}