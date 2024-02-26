using ObserverPattern;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private static ChangeColor singleton;
    public static ChangeColor Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<ChangeColor>();
            return singleton;
        }
    }


    [SerializeField]
    private Timer timer;


    private bool trigger;
    public bool active { get; private set; }


    private ObserverBot timeoutObserver;
    private ObserverBot gameoverObserver;
    private ObserverBot restartObserver;


    private void reset()
    {
        singleton = null;
        trigger = false;
        active = false;

        timeoutObserver = new ObserverBot(() => { TurnSystem.Singleton.TurnEnd(); Active(false); });
        timer.AddObserver(timeoutObserver);

        gameoverObserver = new ObserverBot(() => Active(false));
        Broadcaster.GameOverChannel.AddObserver(gameoverObserver);
    }
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    public void Active(bool active)
    {
        this.active = active;
        trigger = false;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(active);

        var hand = MyHand.Singleton.transform;
        for(int i = 0; i < hand.childCount; i++)
        {
            hand.GetChild(i).GetComponent<CardButton>().Active(false);
        }
        

        if (active)
            timer.reset();
    }
    public void OnClick(int color)
    {
        if (trigger) return;

        switch (color)
        {
            case 0:
                Focus.Singleton.SetColor(ESymbol.Red);
                break;
            case 1:
                Focus.Singleton.SetColor(ESymbol.Yellow);
                break;
            case 2:
                Focus.Singleton.SetColor(ESymbol.Blue);
                break;
            case 3:
                Focus.Singleton.SetColor(ESymbol.Green);
                break;
        }
        TurnSystem.Singleton.TurnEnd();
        Active(false);

    }
    public void RandomChange()
    {
        var color =Random.Range(0, 3);

        if (trigger) return;

        switch (color)
        {
            case 0:
                Focus.Singleton.SetColor(ESymbol.Red);
                break;
            case 1:
                Focus.Singleton.SetColor(ESymbol.Yellow);
                break;
            case 2:
                Focus.Singleton.SetColor(ESymbol.Blue);
                break;
            case 3:
                Focus.Singleton.SetColor(ESymbol.Green);
                break;
        }

    }
}
