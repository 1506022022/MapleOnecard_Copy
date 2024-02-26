using ObserverPattern;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackFlame : MonoBehaviour
{
    private static AttackFlame singleton;
    public static AttackFlame Singleton
    {
        get {
            if (singleton == null) singleton = FindObjectOfType<AttackFlame>();
            return singleton;
        }
    }


    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = Mathf.Clamp(value, 0, 12);
        }
    }
    [SerializeField]
    private int count;
    [SerializeField]
    private GameObject[] flames;


    private ObserverBot timeoutObserver;
    private ObserverBot turnEndObserver;
    private ObserverBot restartObserver;

    private int observer;
    private bool Event;


    private void reset()
    {
        singleton = null;
        observer = 0;
        Event = false;
        Count = 0;
        timeoutObserver = new ObserverBot(() => StartCoroutine(Singleton.Attack()));
        Broadcaster.SubmitTimeOutChannel.AddObserver(timeoutObserver);

        turnEndObserver = new ObserverBot(() => { if (!Focus.Singleton.trigger) StartCoroutine(Singleton.Attack()); });
        Broadcaster.TurnEndChannel.AddObserver(turnEndObserver);
    }
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }

    private void Update()
    {
        
        if(observer != Count)
        {
            // 공격 이벤트가 진행 중이면 스킵
            if (flames.Any(x => x.GetComponent<Flame>().Event)) return;

            // 공격 연출 실행
            StartCoroutine(Fire());
            observer = Count;
        }
    }
    private IEnumerator Fire()
    {
        Event = true;
        for (int i = 0; i < flames.Length; i++)
        {
            if (i < Count)
            {
                if (flames[i].activeSelf) continue;
                flames[i].SetActive(true);
            }
            else
            {
                if (!flames[i].activeSelf) continue;
                if (flames[i].GetComponent<Flame>().Event) continue;
                flames[i].SetActive(false);
                continue;
            }
            yield return new WaitForSeconds(0.5f);
        }
        Event = false;
    }
    private IEnumerator Attack()
    {
        if (Count == 0) yield break;

        // 미하일 방패 처리
        var shields=FindObjectsOfType<Shield>();
        foreach (var shield in shields)
        {
            if (shield.player == TurnSystem.Singleton.current)
            {
                Count = 0;
                Destroy(shield.gameObject);
                yield break;
            }
        }

        // 어택 데이터 처리
        HelpMessage.Singleton.HelpString(TurnSystem.Singleton.current.NickName+"님이 "+Count+"의 피해를 입었습니다.");
        Vector3 target = TurnSystem.Singleton.current.transform.position;
        if(TurnSystem.Singleton.isMyTurn)
        {
            for(int i =0; i<Count;i++)
            Deck.SIngleton.Draw();
        }
        else
        {
            for (int i = 0; i < Count; i++)
            TurnSystem.Singleton.current.Draw();
        }

        // 불꽃 발사 연출
        while (Event)
        {
            yield return new WaitForSeconds(0.1f);
        }
        for(int i =0; i < Count; i++)
        {
            transform.GetChild(i).GetComponent<Flame>().Attack(target);
        }
        Count = 0;

        
    }

}
