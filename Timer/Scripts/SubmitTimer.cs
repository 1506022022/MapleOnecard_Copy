using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitTimer : MonoBehaviour, ISubject
{
    private static SubmitTimer singleton;
    public static SubmitTimer Singleton
    {
        get
        {
            if (singleton == null)
            {

                singleton = FindObjectOfType<SubmitTimer>();
            }
            return singleton;
        }
    }

    [SerializeField]
    private Timer other;
    [SerializeField]
    private Timer my;

    private List<Observer> observers;
    private ObserverBot turnObserver;
    private ObserverBot timeOutObserver;
    private ObserverBot cardSubmitObserver;
    private ObserverBot gameOverObserver;
    private ObserverBot victoryObserver;
    private ObserverBot restartObserver;

    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void reset()
    {
        singleton = null;
        observers = null;
        other.gameObject.SetActive(false);
        my.gameObject.SetActive(false);

        turnObserver = new ObserverBot(TimerSet);
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);

        timeOutObserver = new ObserverBot(() => { Notify(); });
        other.AddObserver(timeOutObserver);
        my.AddObserver(timeOutObserver);

        gameOverObserver = new ObserverBot(Pause);
        Broadcaster.GameOverChannel.AddObserver(gameOverObserver);

        victoryObserver = new ObserverBot(Pause);
        Broadcaster.VictoryChannel.AddObserver(victoryObserver);

        cardSubmitObserver = new ObserverBot(Pause);
        Broadcaster.CardSubmitChannel.AddObserver(cardSubmitObserver);



       Broadcaster.SubmitTimeOutChannel.AddSuject(this);
    }
    private void TimerSet()
    {
        if (TurnSystem.Singleton.isMyTurn)
        {
            other.gameObject.SetActive(false);
            my.gameObject.SetActive(true);
            my.reset();
        }
        else
        {
            my.gameObject.SetActive(false);
            other.gameObject.SetActive(true);
            other.reset();
        }

    }
    public void Pause()
    {
        if(other.gameObject.activeSelf)
            other.stop = true;
        if (my.gameObject.activeSelf)
            my.stop = true;
    }
    public void Restart()
    {
        if (other.gameObject.activeSelf)
            other.stop = false;
        if (my.gameObject.activeSelf)
            my.stop = false;
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
            o.OnNotify();
    }

}
