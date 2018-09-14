using System.Collections;
using System.Collections.Generic;
using Framework.TurnBased;
public class GameEvent
{
}

[System.Serializable]
public class Events : GenericEvents<GameEvent>
{
}


[System.Serializable]
public class GenericEvents<J> : Singleton<GenericEvents<J>>
{
	public delegate void EventDelegate<T> (T e) where T : J;
	private delegate void EventDelegate (J e);
	
	private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
	private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
	
	public void AddListener<T> (EventDelegate<T> del) where T : J
	{	
		// Early-out if we've already registered this delegate
		if (delegateLookup.ContainsKey(del))
			return;
		
		// Create a new non-generic delegate which calls our generic one.
		// This is the delegate we actually invoke.
		EventDelegate internalDelegate = (e) => del((T)e);
		delegateLookup[del] = internalDelegate;
		
		EventDelegate tempDel;
		if (delegates.TryGetValue(typeof(T), out tempDel))
		{
			delegates[typeof(T)] = tempDel += internalDelegate; 
		}
		else
		{
			delegates[typeof(T)] = internalDelegate;
		}
	}
	
	public void RemoveListener<T> (EventDelegate<T> del) where T : J
	{
		EventDelegate internalDelegate;
		if (delegateLookup.TryGetValue(del, out internalDelegate))
		{
			EventDelegate tempDel;
			if (delegates.TryGetValue(typeof(T), out tempDel))
			{
				tempDel -= internalDelegate;
				if (tempDel == null)
				{
					delegates.Remove(typeof(T));
				}
				else
				{
					delegates[typeof(T)] = tempDel;
				}
			}
			
			delegateLookup.Remove(del);
		}
	}
	
	public void Raise (J e)
	{
		EventDelegate del;
		if (delegates.TryGetValue(e.GetType(), out del))
		{
			del.Invoke(e);
		}
	}
}
