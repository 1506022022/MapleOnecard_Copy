using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{
    private static Focus singleton;
    public static Focus Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<Focus>();
            return singleton;
        }
    }

    [SerializeField]
    private Animator getani;
    [SerializeField]
    private Spriter getMagicAni;
    [SerializeField]
    private GameObject Shield;

    private List<Card> cards;
    public Card info { get; private set; }
    public bool trigger { get; private set; }
    private SpriteRenderer card;

    private ObserverBot turnObserver;
    private ObserverBot restartObserver;

    private SubjectAgent reverseSubject;
    private SubjectAgent cardSubmitSubject;
    
    void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }

    private void reset()
    {
        singleton = null;
        cards = new List<Card>();
        info = null;
        trigger = false;
        card = GetComponent<SpriteRenderer>();
        card.sprite = null;
        turnObserver = new ObserverBot(() => trigger = false);
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);

        reverseSubject = new SubjectAgent();
        Broadcaster.ReverseTurnChannel.AddSuject(reverseSubject);

        cardSubmitSubject = new SubjectAgent();
        Broadcaster.CardSubmitChannel.AddSuject(cardSubmitSubject);
    }
    public void GiveTo(Card info)
    {
        trigger = true;
        if(info.number == ENumber.special && info.symbol == ESymbol.Violet)
        {
            HelpMessage.Singleton.HelpString("이카르트가 은신을 사용하여 턴을 넘깁니다.");
            Deck.SIngleton.AddCard(info);
            TurnSystem.Singleton.TurnEnd();
            CutScene.Singleton.IcartScene();
            return;
        }
        this.info = info;
        card.sprite = CardSprite.Singleton.GetSprite(info);
        cards.Add(info);

        cardSubmitSubject.Notify();

        Getani();
        Getani_Data();


    }
    public void GiveTo(Card info, bool deck)
    {
        this.info = info;
        card.sprite = CardSprite.Singleton.GetSprite(info);
        Getani();
        cards.Add(info);
    }
    public List<Card> TakeTo()
    {
        if (cards.Count < 2) return new List<Card>();
        var count = cards.Count - 1;
        var temp = cards.GetRange(0, count);
        cards.RemoveRange(0, count);
        StartCoroutine(TakeToAni(count));
        return temp;

    }
    private void Getani()
    {
        switch (info.number)
        {
            case ENumber.jump:
                getMagicAni.StartAnim();
                break;
            case ENumber.king:
                getMagicAni.StartAnim();
                break;
            case ENumber.reverse:
                getMagicAni.StartAnim();
                reverseSubject.OnNotify();
                break;
            case ENumber.attack_three:
                getMagicAni.StartAnim();
                break;
            case ENumber.attack_two:
                getMagicAni.StartAnim();
                break;
            case ENumber.change:
                getMagicAni.StartAnim();
                break;
            case ENumber.special:
                if (info.symbol == ESymbol.Violet) break;
                getani.Play("Anim_Me_Getani");
                break;
            default:
                getani.Play("Anim_Me_Getani");
                break;
        }
    }
    private void Getani_Data()
    {
        switch (info.number)
        {
            case ENumber.jump:
                TurnSystem.Singleton.Jump();
                break;
            case ENumber.king:
                TurnSystem.Singleton.King();
                break;
            case ENumber.reverse:
                TurnSystem.Singleton.Reverse();
                break;
            case ENumber.attack_three:

                AttackFlame.Singleton.Count += 3;
                break;
            case ENumber.attack_two:
                AttackFlame.Singleton.Count += 2;
                break;
            case ENumber.change:
                if (TurnSystem.Singleton.isMyTurn)
                {
                    ChangeColor.Singleton.Active(true);
                }
                else ChangeColor.Singleton.RandomChange();
                break;
            case ENumber.special:
                if (info.symbol == ESymbol.Red)
                {
                    AttackFlame.Singleton.Count += 5;
                }
                else if(info.symbol == ESymbol.Yellow)
                {
                    CrateShield();
                }
                else if (info.symbol == ESymbol.Blue)
                {
                    var targets = TurnSystem.Singleton.Turns;

                    foreach(var target in targets)
                    {
                        if (target == TurnSystem.Singleton.current) continue;
                        if(target==MyHand.Singleton.Me)
                        {
                            Deck.SIngleton.Draw();
                            Deck.SIngleton.Draw();
                        }
                        else
                        {
                            target.Draw();
                            target.Draw();
                        }
                    }
                }
                else if (info.symbol == ESymbol.Green)
                {
                    var players = TurnSystem.Singleton.Turns;
                    foreach (var player in players)
                    {

                        if (player == MyHand.Singleton.Me) MyHand.Singleton.DeleteGreenCard();
                        else
                        {
                            player.GetComponentInChildren<HandOther>().DeleteGreenCard();
                        }
                    }

                    if (TurnSystem.Singleton.isMyTurn)
                        ChangeColor.Singleton.Active(true);
                    else
                        ChangeColor.Singleton.RandomChange();

                }

                break;
            default:

                break;
        }

        var shields = FindObjectsOfType<Shield>();
        foreach(var shield in shields)
        {
            if (TurnSystem.Singleton.current == shield.player)
                shield.Def--;
        }
        
        if(!ChangeColor.Singleton.active)
        TurnSystem.Singleton.TurnEnd();
    }
    private IEnumerator TakeToAni(int count)
    {
        count = count>7?7 : count;
        for (int i = 0; i < count; i++)
        {
            getani.Play("Anim_Me_Getani");
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetColor(ESymbol color)
    {
        info = new Card(color, ENumber.change);
        card.sprite = CardSprite.Singleton.GetSprite(info);
    }

    private void CrateShield()
    {
        var obj = Instantiate(Shield);
        obj.GetComponent<Shield>().Init(TurnSystem.Singleton.current);

        
    }
}


