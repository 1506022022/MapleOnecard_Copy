using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Emotion
{
    Think,Sad, Angry, kk,Thank
}
public enum turnState
{
    my,next,wait
}
public class SlotBoard : MonoBehaviour, ISubject
{
    [SerializeField]
    private GameObject myTurnLight;
    [SerializeField]
    private GameObject nextTurnLight;
    [SerializeField]
    private Animator animTurn;
    [SerializeField]
    private Animator animEmo;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private string nickName;
    public string NickName
    {
        get
        {
            return nickName;
        }

    }
    [SerializeField]
    private Text nameUI;
    

    List<Observer> observers;
    private ObserverBot turnObserver;
    private ObserverBot restartObserver;
    private SubjectAgent victorySubject;
    private SubjectAgent lastcardSubject;
    private SubjectAgent gameoverSubject;
    

    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void reset()
    {
        nameUI.text = NickName;
        animTurn.SetBool("isEnd", false);

        turnObserver = new ObserverBot(Stat);
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);

        victorySubject = new SubjectAgent();
        Broadcaster.VictoryChannel.AddSuject(victorySubject);

        lastcardSubject = new SubjectAgent();
        Broadcaster.LastcardChannel.AddSuject(lastcardSubject);

        gameoverSubject = new SubjectAgent();
        Broadcaster.GameOverChannel.AddSuject(gameoverSubject);
    }
    private void MyTurn()
    {
        if(nextTurnLight.activeSelf)
            nextTurnLight.SetActive(false);
        myTurnLight.SetActive(true);

        animTurn.Play("Anim_myTurn_Appear");
    }
    private void NextTurn()
    {
        if (myTurnLight.activeSelf)
            myTurnLight.SetActive(false);
        nextTurnLight.SetActive(true);

        animTurn.Play("Anim_Next_Appear");
    }
    private void WaitingTurn()
    {
        if (nextTurnLight.activeSelf)
            nextTurnLight.SetActive(false);
        if (myTurnLight.activeSelf)
            myTurnLight.SetActive(false);

        animTurn.Play("Anim_Waiting_Appear");
    }
    private void End()
    {
        TurnSystem.Singleton.Out(this);
        animTurn.SetBool("isEnd", true);
        myTurnLight.SetActive(false);
        nextTurnLight.SetActive(false);
    }
    public void Win()
    {
        HelpMessage.Singleton.HelpString(NickName + "님의 승리! 게임이 종료됩니다.");
        if (MyHand.Singleton.GetComponentInParent<SlotBoard>() == this)
        {
            victorySubject.Notify();

        }
        else
        {
            
            gameoverSubject.Notify();

        }
        End();
    }
    public void Lose()
    {
        End();
    }
    public void LastCard()
    {
        lastcardSubject.Notify();
    }
    private void Stat()
    {
        var turn = TurnSystem.Singleton.Turns;


        if (turn.Count > 1 && turn.First().Equals(this))
            MyTurn();
        else if (turn.Count > 2 && turn[1].Equals(this))
            NextTurn();
        else if (turn.Count > 1 && !turn.Any(x => x.Equals(this)))
            End();
        else WaitingTurn();

    }
    private IEnumerator Emotion(Emotion type)
    {
        animEmo.SetBool("End",false);
        animEmo.Play(type.ToString());
        animEmo.speed = 0;
        yield return new WaitForSeconds(0.6f);
        animEmo.speed = 1;
        yield return new WaitForSeconds(2.4f);
        animEmo.SetBool("End", true);

    }
    public void StartEmotion(int type)
    {
        StartCoroutine(Emotion((Emotion) type));
    }
    public void Draw()
    {
        var temp = GetComponentInChildren<HandOther>();
        if (temp != null)
        {
            temp.CardCount++;
            temp.AddCard(Deck.SIngleton.Deal());
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
            o.OnNotify();
    }
}
