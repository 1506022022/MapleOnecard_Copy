using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    private List<Card> cards = new List<Card>();
    public List<Card> Cards
    {
        get
        {
            if (cards == null) cards = new List<Card>();
            return cards.ToList();
        }
    }
    public int HandCount
    {
        get
        {
            if (cards == null) cards = new List<Card>();
            return cards.Count;
        }
    }

    public void AddCard(Card card)
    {
        if (cards == null) cards = new List<Card>();
        cards.Add(card);
    }
    public void RemoveCard(Card card)
    {
        if (cards == null) return;
        cards.Remove(card);
    }
    
}
