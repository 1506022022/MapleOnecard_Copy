using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : MonoBehaviour, ISubject
{
    private static StartEvent singleton;
    public static StartEvent Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<StartEvent>();
            }

            
            return singleton;
        }
    }

    [SerializeField]
    private GameObject EmotionUI;
    [SerializeField]
    private SpriteRenderer Board;
    private List<Observer> observers;
    private ObserverBot restartObserver;

    private void Start()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void reset()
    {
        var sds = FindObjectsOfType<Shield>();
        foreach(var sd in sds)
        {
            Destroy(sd.gameObject);
        }
        EmotionUI.SetActive(false);
        singleton = null;
        observers = null;
        StopAllCoroutines();
        // 턴 생성 용도, 여기서 안하면 오류남
        var create = TurnSystem.Singleton;
        StartCoroutine(GameStart());
        Broadcaster.GameStartChannel.AddSuject(this);
    }
    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        Deck.SIngleton.Create();
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().Play("Anim_Screeneff_Start");
        GetComponent<SoundControll>().Play();
        EmotionUI.SetActive(true);
        Board.sortingOrder = -32748;

        Notify();

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
