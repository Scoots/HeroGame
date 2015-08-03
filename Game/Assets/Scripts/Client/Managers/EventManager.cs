using System; 
using System.Collections.Generic;
using EventHandlerList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object, System.Action<object>>>; 


public class EventManager : IEventDispatcher
{
    private readonly ListDictionary<Type, EventHandlerList> _eventHandlers = new ListDictionary<Type, EventHandlerList>();

    private readonly ListDictionary<Type, HashSet<object>> _removeHandlerList = new ListDictionary<Type, HashSet<object>>();

    private Queue<object> _forwardEventQueue = new Queue<object>();

    private Queue<object> _currentEventQueue = new Queue<object>();

    private readonly List<Action> _workingList = new List<Action>(); 

    private readonly Queue<Action> _dispatchQueue = new Queue<Action>(); 

    public void AddEventHandler<T>(Action<T> eventHandler)
    {
        if (eventHandler == null)
        {
            return;
        }

        Type eventType = typeof(T);
        EventHandlerList eventHandlerList;
        if (!_eventHandlers.TryGetValue(eventType, out eventHandlerList))
        {
            eventHandlerList = new EventHandlerList();
            _eventHandlers.Add(eventType, eventHandlerList); 
        }

        eventHandlerList.Add(new KeyValuePair<object, Action<object>>(eventHandler, eventObject => 
            {
                if(eventHandler != null)
                {
                    eventHandler((T)eventObject); 
                }

            }));

        HashSet<object> removeHandlerList; 
        if (_removeHandlerList.TryGetValue(eventType, out removeHandlerList))
        {
            removeHandlerList.Remove(eventHandler); 
        }
    }

    public void RemoveEventHandler<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);
        HashSet<object> removeHandlerList; 

        if(!_removeHandlerList.TryGetValue(eventType, out removeHandlerList))
        {
            removeHandlerList = new HashSet<object>();
            _removeHandlerList.Add(eventType, removeHandlerList); 
        }

        removeHandlerList.Add(eventHandler); 

    }

    public void QueueEvent(object eventObject)
    {
        _forwardEventQueue.Enqueue(eventObject); 
    }

    public void HandleEvent(object eventObject)
    {
        Type eventType = eventObject.GetType();
        EventHandlerList eventHandlerList;
        if (_eventHandlers.TryGetValue(eventType, out eventHandlerList))
        {
            int count = eventHandlerList.Count;
            for (int i = 0; i < count; i++)
            {
                KeyValuePair<object, Action<object>> eventHandlerPair = eventHandlerList[i];

                eventHandlerPair.Value(eventObject);
            }
        }
    }

    public void Dispatch(Action callback)
    {
        lock (_dispatchQueue)
        {
            _dispatchQueue.Enqueue(callback); 
        }
    }

    public void Update()
    {
        for (int i = 0, count = _removeHandlerList.Count; i< count; i++)
        {
            var key = _removeHandlerList.Keys[i];
            var value = _removeHandlerList.Values[i];

            EventHandlerList eventHandlerList; 
            if (_eventHandlers.TryGetValue(key, out eventHandlerList))
            {
                for (int j = eventHandlerList.Count -1; j>= 0 ; j--)
                {
                    if(value.Contains(eventHandlerList[j].Key))
                    {
                        eventHandlerList.RemoveAt(j); 
                    }
                }
            }
        }
        _removeHandlerList.Clear(); 

        lock (_dispatchQueue)
        {
            _workingList.Clear(); 
            while (_dispatchQueue.Count > 0 )
            {
                _workingList.Add(_dispatchQueue.Dequeue()); 
            }
        }
        for (int i = 0, count = _workingList.Count; i < count; i++)
        {
            var dispatchAction = _workingList[i];
            if(dispatchAction != null)
            {
                dispatchAction(); 
            }
        }

        Queue<object> temp = _currentEventQueue;
        _currentEventQueue = _forwardEventQueue;
        _forwardEventQueue = temp;
        _forwardEventQueue.Clear(); 

        while (_currentEventQueue.Count > 0)
        {
            HandleEvent(_currentEventQueue.Dequeue()); 
        }
    }
}
