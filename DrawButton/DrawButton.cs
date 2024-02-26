using ObserverPattern;
using UnityEngine;
using UnityEngine.UI;

public class DrawButton : MonoBehaviour
{
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private SoundControll drawAudio;

    ObserverBot turnObserver;
    ObserverBot restartObserver;

    bool trigger;


    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }

    private void reset()
    {
        trigger = false;
        button.transform.GetChild(0).gameObject.SetActive(false);
        turnObserver = new ObserverBot(myTurn);
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);
    }

    private void myTurn()
    {
        if (TurnSystem.Singleton.isMyTurn)
        {
            button.GetComponentInChildren<Button>().interactable = true;
            button.transform.GetChild(0).gameObject.SetActive(true);
            trigger = false;
        }
        else
        {
            button.GetComponentInChildren<Button>().interactable = false;
            button.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void OnClcik()
    {
        if (trigger) return;
        trigger = true;
        drawAudio.Play();
        TurnSystem.Singleton.TurnEnd();
    }



}
