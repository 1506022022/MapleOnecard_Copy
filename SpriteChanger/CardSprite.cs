using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSprite : MonoBehaviour
{
    private static CardSprite singleton;
    public static CardSprite Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<CardSprite>();
            return singleton;
        }
    }

    [SerializeField]
    private Sprite[] redCards;
    [SerializeField]
    private Sprite[] yellowCards;
    [SerializeField]
    private Sprite[] greenCards;
    [SerializeField]
    private Sprite[] blueCards;
    [SerializeField]
    private Sprite violetCard;


    public Sprite GetSprite(Card card)
    {
        switch (card.symbol)
        {
            case ESymbol.Red:
                return redCards[(int)card.number];

            case ESymbol.Green:
                return greenCards[(int)card.number];

            case ESymbol.Yellow:
                return yellowCards[(int)card.number];

            case ESymbol.Blue:
                return blueCards[(int)card.number];

            case ESymbol.Violet:
                return violetCard;

            default:
                return null;
        }
    }
    public Sprite GetSprite(ESymbol symbol, ENumber number)
    {
        switch (symbol)
        {
            case ESymbol.Red:
                return redCards[(int)number];

            case ESymbol.Green:
                return greenCards[(int)number];

            case ESymbol.Yellow:
                return yellowCards[(int)number];

            case ESymbol.Blue:
                return blueCards[(int)number];

            case ESymbol.Violet:
                return violetCard;

            default:
                return null;
        }
    }
}
