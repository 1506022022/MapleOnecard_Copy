using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandOther : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cards;
    [SerializeField]
    private SoundControll dropoutAudio;
    [SerializeField]
    private SoundControll card_dropoutAudio;

    private List<Card> hand;
    public List<Card> Hand
    {
        get
        {
            if (hand == null) hand = new List<Card>();
            return hand.ToList();
        }
    }
    private int cardCount;

    public int CardCount
    {
        get { return cardCount; }
        set {
            var v = Mathf.Clamp(value, 0, 100);
            if (play)
            {
                if (cardCount > 0 && v == 0) GetComponentInParent<SlotBoard>().Win();
                if (cardCount > 1 && v == 1) lastCardSubject.Notify();
                if (v > 16) { GetComponentInParent<SlotBoard>().Lose(); DropDown(); }
            }
            cardCount = v;
        }
    }
    private int temp;
    private bool play;

    private ObserverBot restartObserver;
    private SubjectAgent lastCardSubject;
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void Update()
    {
        if (!play) return;
        if (temp != cardCount)
        {
            if (temp > cardCount)
            {
                for (int i = temp; i > cardCount; i--)
                    cards[Mathf.Clamp(i - 1,0,cards.Length-1)].SetActive(false);
            }
            else if (temp < cardCount)
            {
                for (int i = temp - 1; i < cardCount; i++)
                {
                    var card = cards[Mathf.Clamp(i, 0, cards.Length-1)];
                    card.SetActive(true);
                    StartCoroutine( GetaniDelay(((i-temp+1)*0.2f),()=>card.GetComponent<Animator>().Play("Anim_Getani")));
                }
                
            }
            temp = cardCount;
        }
    }

    private void reset()
    {
        foreach(var card in cards)
        {
            card.SetActive(false);
        }
        play = false;
        hand = new List<Card>();
        temp = 0;
        CardCount = 0;
        lastCardSubject = new SubjectAgent();
        Broadcaster.LastcardChannel.AddSuject(lastCardSubject);
        play = true;
    }
    private IEnumerator GetaniDelay(float delay,Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    public void AddCard(Card card)
    {
        if (card != null)
        {
            hand?.Add(card);
        }
    }
    public void Submit(Card card)
    {
        hand.Remove(card);
        CardCount--;
        Focus.Singleton.GiveTo(card);
    }
    public void DeleteGreenCard()
    {

        foreach (var cd in Hand)
            if (cd.symbol == ESymbol.Green)
            {
                hand.Remove(cd);
                Deck.SIngleton.AddCard(cd);
                CardCount--;
            }
            
        
    }
    public void GiveUp()
    {
        TurnSystem.Singleton.TurnEnd();
    }
    public void DropDown()
    {
        play = false;
        foreach (var card in hand)
            Deck.SIngleton.AddCard(card);
        hand = null;
        cardCount = 0;

        for (int i = 0; i < cards.Length; i++)
            cards[i].SetActive(false);

        dropoutAudio.Play();
        card_dropoutAudio.Play();
    }

}
