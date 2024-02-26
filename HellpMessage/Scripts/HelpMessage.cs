using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HelpMessage : MonoBehaviour
{
    private static HelpMessage singleton;
    public static HelpMessage Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<HelpMessage>();
            return singleton;
        }
    }


    [SerializeField]
    private SoundControll bgm;
    [SerializeField]
    private SoundControll myTurnAudio;
    [SerializeField]
    private SoundControll turnAutio;
    [SerializeField]
    private SoundControll card_magicAudio;
    [SerializeField]
    private SoundControll card_goAudio;
    [SerializeField]
    private SoundControll reverseAudio;


    private Text help;
    private Animator anim;
    private bool play;


    private ObserverBot turnObserver;
    private ObserverBot submitObserver;
    private ObserverBot restartObserver;
    private ObserverBot gamestartObserver;
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void reset()
    {
        play = false;
        singleton = null;
        turnObserver = new ObserverBot(MyTurnString);
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);

        submitObserver = new ObserverBot(CardString);
        Broadcaster.CardSubmitChannel.AddObserver(submitObserver);

        gamestartObserver = new ObserverBot(async () => {
            await Task.Delay(100);
            play = true; });
        Broadcaster.GameStartChannel.AddObserver(gamestartObserver);

        bgm.Play();

        help = GetComponent<Text>();
        anim = GetComponent<Animator>();
        
        HelpString("잠시 후 게임이 시작됩니다...");
    }
    public void HelpString(string message)
    {
        anim.Play("Empty");
        help.text = message;
        anim.Play("Anim_Fadeout");
    }

    private void MyTurnString()
    {
        if(play)
        if (TurnSystem.Singleton.isMyTurn)
        {
            HelpString("당신의 턴입니다.");
            myTurnAudio.Play();
        }
        else
        {
            turnAutio.Play();
        }
    }
    private void CardString()
    {
        if (!play) return;
        var card = Focus.Singleton.info;

        switch (card.number)
        {
            case ENumber.attack_two:
                HelpString(TurnSystem.Singleton.current.NickName+ "님의 공격!");
                card_goAudio.Play();
                break;
            case ENumber.attack_three:
                HelpString(TurnSystem.Singleton.current.NickName + "님의 공격!");
                card_goAudio.Play();
                break;
            case ENumber.change:
                HelpString("마법 : 색 바꾸기!");
                card_magicAudio.Play();
                break;
            case ENumber.king:
                HelpString("한 번 더!");
                card_goAudio.Play();
                break;
            case ENumber.jump:
                HelpString("마법 : 점프!");
                card_magicAudio.Play();
                break;
            case ENumber.reverse:
                HelpString("마법 : 거꾸로!");
                card_magicAudio.Play();
                break;
            case ENumber.special:
                card_magicAudio.Play();
                if (card.symbol == ESymbol.Red)
                    HelpString("오즈가 강력한 공격마법을 시전하였습니다.");
                else if (card.symbol == ESymbol.Green)
                    HelpString("이리나가 모든 초록색 카드를 흡수하였습니다.");
                else if (card.symbol == ESymbol.Yellow)
                    HelpString("미하일이 방패를 소환하였습니다.");
                else if (card.symbol == ESymbol.Violet)
                    HelpString("이카르트가 은신을 사용하여 턴을 넘깁니다.");
                else if (card.symbol == ESymbol.Blue)
                    HelpString("호크아이가 모두에게 카드를 선물합니다.");
                break;
            default:
                card_goAudio.Play();
                break;
        }

            
        
    }
}
