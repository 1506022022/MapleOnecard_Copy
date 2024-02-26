using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bot : MonoBehaviour
{
    private float thinkTime = 5.0f;        // 봇이 결정을 내릴 때까지의 시간이다. (0.5 ~ thinkTime)
    private ObserverBot turnObserver;
    private ObserverBot restartObserver;

    private void Awake()
    {
        // 재시작시 이벤트 지정
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
        // 내 턴일 때만 동작한다.
        if (TurnSystem.Singleton.isMyTurn) yield break;

        // 생각하는 시간을 가진다.
        yield return new WaitForSeconds(Random.Range(0.5f, thinkTime));

        // 자신의 패를 확인한다. 
        var player = TurnSystem.Singleton.current.GetComponentInChildren<HandOther>();
        if (player == null) yield break;
        var hand = player.Hand;

        // 제출 가능한 카드를 찾는다.
        var linq = hand.Where(x => RuleSystem.Rule(x));
        if (linq == null) yield break;
        var list = linq.ToList();

        // 제출 가능한 카드가 없으면 패를 1장 먹는다.
        if (list.Count == 0) { player.GiveUp(); yield break; }

        // 랜덤으로 한 장 제출한다. 
        var index = Random.Range(0, Mathf.Clamp(list.Count-1,0,100));
        player.Submit(list[index]);


    }



}
