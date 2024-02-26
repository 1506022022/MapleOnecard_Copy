using ObserverPattern;
using UnityEngine;

public class MyHand : MonoBehaviour
{
    private static MyHand singleton;
    public static MyHand Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<MyHand>();
            return singleton;
        }
    }

    [SerializeField]
    private SlotBoard me;
    public SlotBoard Me
    {
        get
        {
            return me;
        }
        private set
        {
            me = value;
        }
    }
    [SerializeField]
    private float space;
    [SerializeField]
    private GameObject cardPre;
    

    private Vector3 origin;
    private int observer;
    private bool play;


    private ObserverBot gameStartObserver;
    private ObserverBot restartObserver;

    private SubjectAgent dropoutSubject;
    private SubjectAgent lastcardSubject;


    private void Awake()
    {
    // 재시작 이벤트를 추가한다. 
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void Update()
    {
        // 소지한 카드에 변동이 생기는지 확인한다.
        if (observer != transform.childCount)
        {
            LastCard();
            Sort();

            if (play)
            {
            // 손패를 모두 털었으면 승리한다.
                if (observer > 0 && transform.childCount == 0)
                {
                    me.Win();
                }
                // 16장을 초과하면 패배한다.
                else if (transform.childCount > 16)
                {
                    dropoutSubject.Notify();
                    me.Lose();
                    DropDown();
                }
                // 원카드
                if (observer > 1 && transform.childCount == 1)
                {
                    lastcardSubject.Notify();
                }
                observer = transform.childCount;
            }
        }


    }
    // 재시작시 실행되는 이벤트
    private void reset()
    {
        play = false;
        singleton = null;
        origin = Vector3.zero;
        observer = 0;

        gameStartObserver = new ObserverBot(() => play = true);
        Broadcaster.GameStartChannel.AddObserver(gameStartObserver);

        dropoutSubject = new SubjectAgent();
        Broadcaster.DropoutChannel.AddSuject(dropoutSubject);

        lastcardSubject = new SubjectAgent();
        Broadcaster.LastcardChannel.AddSuject(lastcardSubject);


        var cards = FindObjectsOfType<CardButton>();
        foreach (var card in cards)
            Destroy(card.gameObject);



        Sort();
    }
    // 손패의 레이어를 정렬시킨다. 
    public void Sort()
    {

        for(int i =0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(origin.x+(space*i),origin.y,origin.z);
            transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = -32747 + i*2;
            transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -32747 +( i * 2)+1;
        }
    }

    // 주어진 카드를 획득하고, 손패를 정렬시킨다.
    public void AddCard(Card info)
    {
        var inst = Instantiate(cardPre,transform).GetComponent<CardButton>();
        inst.Init(info);
        Sort();
    }
    
    private void LastCard()
    {
        if (transform.childCount == 0) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            var last = transform.GetChild(transform.childCount - 1).GetComponent<CardButton>();
            var card = transform.GetChild(i).GetComponent<CardButton>();
            if (last.Equals(card))
                card.button.enabled = true;
            else
                card.button.enabled = false;
        }
    }

    // 특수 카드의 효과로 소지한 녹색 카드를 제거한다.
    public void DeleteGreenCard()
    {
        var cds = GetComponentsInChildren<CardButton>();

        foreach(var cd in cds)
        {
            if (cd.info.symbol == ESymbol.Green)
            {
                Deck.SIngleton.AddCard(cd.info);
                Destroy(cd.gameObject);
            }
        }
    }

    // 게임에서 패배한 경우로 패를 몰수한다.
    public void DropDown()
    {
        play = false;
        var cds = GetComponentsInChildren<CardButton>();

        foreach (var cd in cds)
        {
                Deck.SIngleton.AddCard(cd.info);
                Destroy(cd.gameObject);
            
        }
    }
}
