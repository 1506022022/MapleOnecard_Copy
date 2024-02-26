using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIMe : MonoBehaviour
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
        if (!TurnSystem.Singleton.isMyTurn) yield break;
        yield return new WaitForSeconds(Random.Range(0.5f, thinkTime));



        var hand = MyHand.Singleton.GetComponentsInChildren<CardButton>();

        var linq = hand.Where(x => RuleSystem.Rule(x.info));
        if (linq == null) yield break;
        var list = linq.ToList();
        if (list.Count == 0) { TurnSystem.Singleton.TurnEnd(); yield break; }

        var index = Random.Range(0, Mathf.Clamp(list.Count - 1, 0, 100));
        
        foreach(var cd in hand)
        {
            if (cd.info == list[index].info)
            {
                cd.OnMouseDown();
                MyHand.Singleton.Sort();
                break;
            }

        }


    }


}
