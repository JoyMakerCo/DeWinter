using System;
using System.Collections;

public struct Observable<T>
{
    private Action<T> _notifyHandler;
	private event Action<T> _notifier
    {
        add => _notifyHandler += value;
        remove => _notifyHandler -= value;
    }

    public Observable(T t)
    {
        _notifyHandler = delegate { };
        _value = t;
    }

	public static implicit operator T (Observable<T> t) => t._value;

    private T _value;
    public T Value
	{
        get => _value;
        set
        {
            _value = value;
            _notifyHandler?.Invoke(_value);
        }
    }

	public void Observe(Action<T> listener)
	{
        if (listener != null)
        {
            _notifier += listener;
            listener(_value);
        }
	}

	public void Remove(Action<T> listener)
	{
        if (listener != null)
    		_notifier -= listener;
	}
}
