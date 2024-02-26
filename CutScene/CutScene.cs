using ObserverPattern;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    private static CutScene singleton;
    public static CutScene Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<CutScene>();
            return singleton;
        }
    }


    private Animator anim;


    ObserverBot cardsubmitObserver;
    ObserverBot restartObserver;

    private void reset()
    {
        singleton = null;

        cardsubmitObserver = new ObserverBot(cutScene);
        Broadcaster.CardSubmitChannel.AddObserver(cardsubmitObserver);

        anim = GetComponent<Animator>();
    }
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }

    private void cutScene()
    {
        
        if(Focus.Singleton.trigger)
        {

            var card = Focus.Singleton.info;
            if (card.number == ENumber.special)
            {
                anim.Play("Empty");
                switch (card.symbol)
                {
                    case ESymbol.Blue:
                        anim.Play("Anim_CutScene_Hawkeye");
                        break;
                    case ESymbol.Green:
                        anim.Play("Anim_CutScene_Irina");
                        break;
                    case ESymbol.Red:
                        anim.Play("Anim_CutScene_Oz");
                        break;
                    case ESymbol.Yellow:
                        anim.Play("Anim_CutScene_Michael");
                        break;
                    case ESymbol.Violet:
                        anim.Play("Anim_CutScene_Icart");
                        break;
                }
            }
        }    
    }
    public void IcartScene()
    {
        anim.Play("Empty");
        anim.Play("Anim_CutScene_Icart");
    }
}
