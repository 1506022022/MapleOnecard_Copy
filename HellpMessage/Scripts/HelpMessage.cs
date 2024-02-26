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
        
        HelpString("��� �� ������ ���۵˴ϴ�...");
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
            HelpString("����� ���Դϴ�.");
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
                HelpString(TurnSystem.Singleton.current.NickName+ "���� ����!");
                card_goAudio.Play();
                break;
            case ENumber.attack_three:
                HelpString(TurnSystem.Singleton.current.NickName + "���� ����!");
                card_goAudio.Play();
                break;
            case ENumber.change:
                HelpString("���� : �� �ٲٱ�!");
                card_magicAudio.Play();
                break;
            case ENumber.king:
                HelpString("�� �� ��!");
                card_goAudio.Play();
                break;
            case ENumber.jump:
                HelpString("���� : ����!");
                card_magicAudio.Play();
                break;
            case ENumber.reverse:
                HelpString("���� : �Ųٷ�!");
                card_magicAudio.Play();
                break;
            case ENumber.special:
                card_magicAudio.Play();
                if (card.symbol == ESymbol.Red)
                    HelpString("��� ������ ���ݸ����� �����Ͽ����ϴ�.");
                else if (card.symbol == ESymbol.Green)
                    HelpString("�̸����� ��� �ʷϻ� ī�带 ����Ͽ����ϴ�.");
                else if (card.symbol == ESymbol.Yellow)
                    HelpString("�������� ���и� ��ȯ�Ͽ����ϴ�.");
                else if (card.symbol == ESymbol.Violet)
                    HelpString("��ī��Ʈ�� ������ ����Ͽ� ���� �ѱ�ϴ�.");
                else if (card.symbol == ESymbol.Blue)
                    HelpString("ȣũ���̰� ��ο��� ī�带 �����մϴ�.");
                break;
            default:
                card_goAudio.Play();
                break;
        }

            
        
    }
}
