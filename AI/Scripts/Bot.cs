using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bot : MonoBehaviour
{
    private float thinkTime = 5.0f;

    private ObserverBot turnObserver;
    private ObserverBot restartObserver;

    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }

    private void reset()
    {
        turnObserver = new ObserverBot(() => StartCoroutine(Think()));
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);
    }
    private IEnumerator Think()
    {
        if (TurnSystem.Singleton.isMyTurn) yield break;

        yield return new WaitForSeconds(Random.Range(0.5f, thinkTime));

        var player = TurnSystem.Singleton.current.GetComponentInChildren<HandOther>();
        if (player == null) yield break;
        var hand = player.Hand;
        
        var linq = hand.Where(x => RuleSystem.Rule(x));
        if (linq == null) yield break;
        var list = linq.ToList();
        if (list.Count == 0) { player.GiveUp(); yield break; }

        var index = Random.Range(0, Mathf.Clamp(list.Count-1,0,100));
        player.Submit(list[index]);


    }



}
