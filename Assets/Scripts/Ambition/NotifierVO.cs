using System;
using Core;

public class NotifierVO<T>
{
	public string Message=null;

	protected T _value;

	public NotifierVO() {}

	public NotifierVO (T value)
	{
		this.Set(value);
	}

	public NotifierVO (string message, T value)
	{
		Message = message;
		Set(value);
	}

	public T Set(T value)
	{
		_value = value;
		if (string.IsNullOrEmpty(Message))
		{
			App.Service<MessageSvc>().Send(_value);
		}
		else
		{
			App.Service<MessageSvc>().Send(Message, _value);
		}
		return _value;
	}

	public static implicit operator T (NotifierVO<T> n)
	{
		return n._value;
	}
}