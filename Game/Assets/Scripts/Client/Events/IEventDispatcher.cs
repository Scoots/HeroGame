using System;

public interface IEventDispatcher
{
    void AddEventHandler<T>(Action<T> eventHandler);

    void RemoveEventHandler<T>(Action<T> eventHandler);

    void QueueEvent(object eventObject);

    void HandleEvent(object eventObject); 
}
