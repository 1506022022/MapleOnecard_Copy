using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private static Deck singleton;
    public static Deck SIngleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<Deck>();
            return singleton;
        }
    }


    [SerializeField]
    public List<SlotBoard> otherPlayers;
    [SerializeField]
    public Animator anim;
    [SerializeField]
    private SoundControll suffleAudio;

    private List<Card> cards;
    private int dealCount=30;
    [SerializeField]
    private int attackCard;
    [SerializeField]
    private int michael;
    [SerializeField]
    private int oz;

    private ObserverBot gameStartObserver;
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
        
        gameStartObserver = new ObserverBot(FirstDeal);
        Broadcaster.GameStartChannel.AddObserver(gameStartObserver);

        anim = GetComponent<Animator>();
    }

    public Card Deal()
    {
        if (cards != null && cards.Count > 0)
        {
            var dealCard = cards.Last();
            cards.Remove(dealCard);
            return dealCard;
        }
        dealCount--;
        if (dealCount <= 0)
        {
            Shuffle();
            dealCount = 30;
        }
        if (cards.Count < 10) { cards.InsertRange(0, Focus.Singleton.TakeTo());  }

        return null;
    }
    public void Draw()
    {
        
        if (cards != null && cards.Count > 0)
        {
            var dealCard = cards.Last();

            MyHand.Singleton.AddCard(dealCard);
            cards.Remove(dealCard);
        }
        dealCount--;
        if (dealCount <= 0)
        {
            Shuffle();
            dealCount = 30;
        }
        if (cards.Count < 10) { cards.InsertRange(0, Focus.Singleton.TakeTo()); }
    }
    private void Shuffle()
    {
        Shuffle_Event();
        if (cards == null || cards.Count < 2) return;
        Shuffle_Data();

    }
    private void Shuffle_Data()
    {
        for (int i = 0; i < 4; i++)
        {
            // 탁탁탁
            var count = cards.Count - 1;
            var under = Random.Range(0, count / 2);
            var upper = Random.Range(under, count) - under;
            var temp = cards.GetRange(under, upper);
            cards.RemoveRange(under, upper);
            cards.AddRange(temp);

            // 촤라락
            count++;
            var left = cards.GetRange(0, count/2);
            var right = cards.GetRange((count/2), count - (count / 2));
            List<Card> dummy = new List<Card>();
            while (left.Count != 0 || right.Count != 0)
            {
                if (left.Count > 0)
                {
                    dummy.Add(left.First());
                    left.RemoveAt(0);
                }
                if (right.Count > 0)
                {
                    dummy.Add(right.First());
                    right.RemoveAt(0);
                }

            }
            cards = dummy;
        }
    }
    private void Shuffle_Event()
    {
        anim.Play("Anim_Shuffle");
        suffleAudio.Play();
    }
    public void Create()
    {
        cards = new List<Card>();
        for (int symbol = 0;symbol<4;symbol++)
        for(int number=0; number < 13; number++)
        {
                // 공격카드
                if (number == (int)ENumber.attack_three ||
                    number == (int)ENumber.attack_two)
                {
                    for (int i = 0; i < attackCard; i++)
                        cards.Add(new Card((ESymbol)symbol, (ENumber)number));
                }
                // 미하일
                else if (number == (int)ENumber.special && symbol == (int)ESymbol.Yellow)
                {
                    for (int i = 0; i < michael; i++)
                        cards.Add(new Card((ESymbol)symbol, (ENumber)number));
                }
                else if(number==(int)ENumber.special && symbol == (int)ESymbol.Red)
                {
                    for (int i = 0; i < oz; i++)
                        cards.Add(new Card((ESymbol)symbol, (ENumber)number));
                }
                else
                {
                    cards.Add(new Card((ESymbol)symbol, (ENumber)number));
                }

        }

        // 블랙 이카르트 카드 추가
        cards.Add(new Card(ESymbol.Violet, ENumber.special));
        Shuffle();

    }
    private void FirstDeal()
    {
        if (cards == null) Create();
        var card = cards.Last();
        Focus.Singleton.GiveTo(card,true);
        cards.Remove(card);

        for(int i=0;i<7;i++)
        Draw();

        foreach(var p in otherPlayers)
        {
            for (int i = 0; i < 7; i++)
            {
                p.Draw();
                p.GetComponentInChildren<HandOther>().AddCard(Deal());

            }
        }
    }
    public void AddCard(Card card)
    {
        cards.Insert(0, card);
    }
}
