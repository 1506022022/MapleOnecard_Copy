using ObserverPattern;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static Board singleton;
    public static Board Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<Board>();
            return singleton;
        }
    }


    [SerializeField]
    GameObject clock;
    [SerializeField]
    GameObject reverseClock;
    [SerializeField]
    bool isReverse;


    bool temp = false;

    private ObserverBot reverseObserver;
    private ObserverBot restartObserver;


    private void reset()
    {
        singleton = null;
        isReverse = false;
        temp = false;

        var board = GetComponent<SpriteRenderer>();
        board.sortingOrder = -32000;
        reverseObserver = new ObserverBot(Reverse);
        Broadcaster.ReverseTurnChannel.AddObserver(reverseObserver);
        clock.SetActive(true);
        reverseClock.SetActive(false);
    }
    void Update()
    {
        if (isReverse != temp)
        {
            if (isReverse)
            {
                clock.SetActive(false);
                reverseClock.SetActive(true);
            }else
            {
                reverseClock.SetActive(false);
                clock.SetActive(true);
            }

            temp = isReverse;
        }
    }
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    public void Reverse()
    {
        if (clock.activeSelf)
        {
            clock.SetActive(false);
            reverseClock.SetActive(true);
        }
        else
        {
            reverseClock.SetActive(false);
            clock.SetActive(true);
        }
    }
}
