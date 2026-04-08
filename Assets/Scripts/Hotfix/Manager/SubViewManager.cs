using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PureMVC;
using PureMVC.Interfaces;

public class SubViewObserver
{
    public GameObject go;
    public Action<INotification> callback;

    public SubViewObserver(GameObject go,Action<INotification> callback)
    {
        this.go = go;
        this.callback = callback;
    }
}

public class SubViewManager : Hotfix.TSingleton<SubViewManager>
{
    private Dictionary<string, IList<SubViewObserver>> observers_map = new Dictionary<string, IList<SubViewObserver>>();
    public void AddListener(string[] notification,GameObject go,Action<INotification> callback)
    {
        if(notification!=null)
        {
            for(int i = 0;i<notification.Length;i++)
            {
                AddObserver(notification[i],go, callback);
            }
        }
    }

    private void AddObserver(string notification, GameObject go,Action<INotification> callback)
    {
        IList<SubViewObserver> observers;
        if (observers_map.TryGetValue(notification, out observers))
        {
            observers.Add(new SubViewObserver(go,callback));
        }
        else
        {
            observers_map.Add(notification, new List<SubViewObserver> { new SubViewObserver(go, callback) });
        }
    }

    public void NotifyObervers(INotification notification)
    {
        IList<SubViewObserver> observers_ref;
        if (observers_map.TryGetValue(notification.Name,out observers_ref))
        {
            var observers = new List<SubViewObserver>(observers_ref);
            if(observers.Count==0)
            {
                observers_map.Remove(notification.Name);
            }
            for(int i = observers.Count-1;i>=0;i--)
            {
                if(observers[i].go==null)
                {
                    observers.RemoveAt(i);
                    continue;
                }
                else
                {
                    observers[i].callback?.Invoke(notification);
                }
            }
        }
    }

}
