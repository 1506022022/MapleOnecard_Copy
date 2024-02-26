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
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void Update()
    {
        if (observer != transform.childCount)
        {
            LastCard();
            Sort();

            if (play)
            {
                if (observer > 0 && transform.childCount == 0)
                {
                    me.Win();
                }
                else if (transform.childCount > 16)
                {
                    dropoutSubject.Notify();
                    me.Lose();
                    DropDown();
                }
                if (observer > 1 && transform.childCount == 1)
                {
                    lastcardSubject.Notify();
                }
                observer = transform.childCount;
            }
        }


    }
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
    public void Sort()
    {

        for(int i =0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(origin.x+(space*i),origin.y,origin.z);
            transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = -32747 + i*2;
            transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -32747 +( i * 2)+1;
        }
    }
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
