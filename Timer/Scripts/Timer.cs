using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour,ISubject
{
    [SerializeField]
    private RectTransform leftGage;
    [SerializeField]
    private RectTransform rightGage;
    [SerializeField]
    private RectTransform middleGage;
    [SerializeField]
    private float time;
    [SerializeField]
    private SoundControll timerAudio;

    private float value;

    private List<Observer> observers;
    private ObserverBot gameendObserver;
    public bool stop;
  
    void Update()
    {
        if (stop) return;
        if (value < 0) 
        { 
            TimerOut();
            return;
        }

        value -= Time.deltaTime;
        TickDelete();
    }
    private void Awake()
    {
        gameendObserver = new ObserverBot(() => observers = null);
        Broadcaster.GameendChannel.AddObserver(gameendObserver);

        value = time;
    }

    public void reset()
    {
       // timerAudio.Play();
        leftGage.localPosition = new Vector3(-60f, 1f, 0f);
        leftGage.localScale = new Vector3(1, 1, 1);

        middleGage.localPosition = new Vector3(-58f, 1, 0);
        middleGage.localScale = new Vector3(116, 1, 1);

        rightGage.localPosition = new Vector3(60f, 1f, 0f);
        rightGage.localScale = new Vector3(1, 1, 1);

        value = time;
        stop = false;
    }
    private void TimerOut()
    {

        stop = true;
        Notify();

    }
    private void TickDelete()
    {
        
        int tick = (int)middleGage.localScale.x;
        if (tick <= 0) return;

        var tmp = Mathf.CeilToInt((value / time) * 116f);

        while (tick > tmp)
        {
            var rightPos = rightGage.transform.localPosition;
            middleGage.localScale = new Vector3(tick - 1, 1, 1);
            rightGage.transform.localPosition = new Vector3(rightPos.x-1,rightPos.y,rightPos.z);
            tick = (int)middleGage.localScale.x;
        }

    }

    public void AddObserver(Observer o)
    {
        if (observers == null) observers = new List<Observer>();
        observers.Add(o);
    }

    public void RemoveObserver(Observer o)
    {
        if (observers == null) return;
        observers.Remove(o);
    }

    public void Notify()
    {
        if (observers == null) return;
        foreach (var o in observers)
        {
            o.OnNotify();
        }
    }

    private void OnDisable()
    {
        timerAudio.Stop();
    }
}
