using ObserverPattern;
using System.Threading.Tasks;
using UnityEngine;

public class EventMessage : MonoBehaviour
{
    [SerializeField]
    private Animator reverse;
    [SerializeField]
    private Animator lastcard;
    [SerializeField]
    private Animator dropout;
    [SerializeField]
    private Animator gameover;
    [SerializeField]
    private Spriter victory;
    [SerializeField]
    private SoundControll victoryAudio;
    [SerializeField]
    private SoundControll gameoverAudio;
    [SerializeField]
    private SoundControll dropoutAudio;
    [SerializeField]
    private SoundControll lastcardAudio;
    [SerializeField]
    private SoundControll reverseAudio;
    [SerializeField]
    private SoundControll card_dropoutAudio;


    private ObserverBot reverseObserver;
    private ObserverBot victoryObserver;
    private ObserverBot dropoutObserver;
    private ObserverBot lastcardObserver;
    private ObserverBot gameoverObserver;
    private ObserverBot gameStartObserver;
    private ObserverBot restartObserver;

    private bool play;


    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }

    private void reset()
    {
        play = false;

        reverseObserver = new ObserverBot(Reverse);
        Broadcaster.ReverseTurnChannel.AddObserver(reverseObserver);

        victoryObserver = new ObserverBot(Victory);
        Broadcaster.VictoryChannel.AddObserver(victoryObserver);

        dropoutObserver = new ObserverBot(Dropout);
        Broadcaster.DropoutChannel.AddObserver(dropoutObserver);

        lastcardObserver = new ObserverBot(LastCard);
        Broadcaster.LastcardChannel.AddObserver(lastcardObserver);

        gameoverObserver = new ObserverBot(Gameover);
        Broadcaster.GameOverChannel.AddObserver(gameoverObserver);

        gameStartObserver = new ObserverBot(GameStart);
        Broadcaster.GameStartChannel.AddObserver(gameStartObserver);
    }
    public void Gameover() 
    {
        if (!play) return;
        gameover.Play("Empty");
        gameover.Play("Anim_Gameover");
        gameoverAudio.Play();
    }
    public void Victory() 
    {
        if (!play) return;
        victory.StartAnim();
        victoryAudio.Play();
    }
    public void Dropout()
    {
        if (!play) return;
        dropout.Play("Empty");
        dropout.Play("Anim_Dropout");
        dropoutAudio.Play();
        card_dropoutAudio.Play();
        }
    public void LastCard() 
    {
        if (!play) return;
        lastcard.Play("Empty");
        lastcard.Play("Anim_Lastcard");
        lastcardAudio.Play();
    }
    public void Reverse() 
    {
        if (!play) return;
        reverse.Play("Empty");
        reverse.Play("Anim_Reverse");
        reverseAudio.Play();
    }

    private async void GameStart()
    {
        await Task.Delay(100);
        play = true;
    }


}
