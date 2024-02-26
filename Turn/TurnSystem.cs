using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TurnSystem
{
    private static TurnSystem singleton;
    public static TurnSystem Singleton
    {
        get
        {
            if (TurnSystem.singleton == null)

                singleton = new TurnSystem();

            
                return singleton;
        }
    }
    private TurnSystem() 
    {
        restartObserver = new ObserverBot(Start);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        Start();
    }

    private List<SlotBoard> players;
    private List<SlotBoard> turns;            // 플레이어들의 턴 순서를 저장한다. 
    public List<SlotBoard> Turns
    {
        get
        {
            if (turns == null) turns = new List<SlotBoard>();
            return turns.ToList();
        }
    }
    public SlotBoard current
    {
        get
        {
            return Turns.First();
        }
    }
    public bool isMyTurn
    {
        get
        {
            if (GameObject.Find("Me CharacterSlot")?.GetComponent<SlotBoard>() == null) return false;
            return GameObject.Find("Me CharacterSlot").GetComponent<SlotBoard>() == current;
        }
    }
    public List<SlotBoard> otherPlayers
    {
        get
        {
            if (Turns.Count > 0)
            {
                var temp = Turns;
                temp.RemoveAt(0);
                return temp;
            }
            return new List<SlotBoard>();
        }
    }
    private List<SlotBoard> outPlayers;
    private bool play;
    private bool king;

    // UnityEvent
    private ObserverBot timoutObserver;
    private ObserverBot gameStartObserver;
    private ObserverBot gameOverObserver;
    private ObserverBot victoryObserver;
    private ObserverBot restartObserver;
    public SubjectAgent turnChanged;
    public SubjectAgent turnEnd;


    private void Start()
    {
        singleton = this;
        outPlayers = new List<SlotBoard>();
        turns = new List<SlotBoard>();

        // 게임에 참여한 플레이어 전부를 가져온다.
        players = new List<SlotBoard>();
        players.Add(MyHand.Singleton.GetComponentInParent<SlotBoard>());
        players.AddRange(Deck.SIngleton.otherPlayers);

        foreach (var player in players)
        {
            turns.Add(player);
           
        }

        // 순서를 섞는다.
        RandomTurnSort();


        // 시간 초과 이벤트를 지정한다.
        timoutObserver = new ObserverBot(TurnEnd);
        Broadcaster.SubmitTimeOutChannel.AddObserver(timoutObserver);

        // 게임 시작 이벤트를 지정한다.
        gameStartObserver = new ObserverBot(() => {
            outPlayers = new List<SlotBoard>();
            turnChanged.OnNotify();
            GamePlay(true);
        });
        Broadcaster.GameStartChannel.AddObserver(gameStartObserver);

        // 게임 오버 이벤트를 지정한다.
        gameOverObserver = new ObserverBot(() => GamePlay(false));
        Broadcaster.GameOverChannel.AddObserver(gameOverObserver);

        // 승리했을 때의 이벤트를 지정한다.
        victoryObserver = new ObserverBot(() => GamePlay(false));
        Broadcaster.VictoryChannel.AddObserver(victoryObserver);

        // 턴에 관련된 이벤트들을 지정한다.
        turnChanged = new SubjectAgent();
        Broadcaster.TurnChangedChannel.AddSuject(turnChanged);

        turnEnd = new SubjectAgent();
        Broadcaster.TurnEndChannel.AddSuject(turnEnd);

    }

    private void next()
    {
        // 카드가 'K' 일 때는 자신의 턴을 유지한다.
        if (king)
        {
            king = false;
        }
        else
        
        if (turns.Count > 1) // 게임이 계속 진행되어야 한다. 
        {
            // 턴이 돌아간다.
            var temp = turns.First();
            turns.Remove(temp);
            if (!outPlayers.Contains(temp))
                turns.Add(temp);
        }
        if (turns.Count == 1) // 생존이 1명이므로 게임이 종료된다.
            current.Win();
        else 
        {
            turnChanged.OnNotify(); 
        }
    }

    public async void TurnEnd()
    {
        if (!play) return;
        if(!Focus.Singleton.trigger)
        if (AttackFlame.Singleton.Count == 0)
        {
            if (isMyTurn)
            {
                Deck.SIngleton.Draw();
            }
            else
            {
                current.Draw();
            }
        }
        SubmitTimer.Singleton.Pause();
        await Task.Delay(50);
        turnEnd.OnNotify();
        await Task.Delay(50);
        next();
    }
    // 패배했을때 해당 플레이어를 턴에서 제회한다.
    public void Out(SlotBoard player)
    {
        outPlayers.Add(player);
        
    }
    private void GamePlay(bool play) =>this.play = play;

    // 카드가 'J'일때 한 턴을 뛰어넘는다.
    public void Jump()
    {
        if (turns.Count < 2) return;
        var temp = turns.First();
        turns.RemoveAt(0);
        turns.Add(temp);
    }
    // 카드가 'K'일때 현재 턴을 유지한다.
    public void King()
    {
        if (turns.Count < 2) return;
        king = true;
        
    }
    // 카드가 'Q'일때 순서를 역전한다.
    public void Reverse() 
    {
        if (turns.Count < 2) return;
        turns.Reverse();
        king = true;
    }
    private void RandomTurnSort()
    {
        
        // 턴 순서를 섞는다
        for (int i = 0; i < UnityEngine.Random.Range(0, 3); i++)
        {
            var temp = turns.First();
            turns.Remove(temp);
            turns.Add(temp);
        }

    }

}

namespace ObserverPattern
{
    public class ObserverBot : Observer
    {
        Action action;

        public ObserverBot(Action action)
        {
            this.action = action;
        }

        public override void OnNotify()
        {
            action.Invoke();
        }
    }
    public abstract class Observer
    {
        public abstract void OnNotify();
    }
    public interface ISubject
    {
        public void AddObserver(Observer o);
        public void RemoveObserver(Observer o);
        public void Notify();
    }

    public class SubjectAgent : Observer, ISubject
    {
        private List<Observer> observers;
        public void AddObserver(Observer o)
        {
            if (observers == null) observers = new List<Observer>();
            if (observers.Contains(o)) observers.Remove(o);
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

        public override void OnNotify()
        {
            Notify();
        }
    }
}
