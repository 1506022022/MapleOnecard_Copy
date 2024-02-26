using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    [SerializeField]
    private GameObject disableSprite;
    [SerializeField]
    public BoxCollider2D button;
    [SerializeField]
    private Animator getani;

    private ObserverBot turnObserver;
    private ObserverBot gameoverObserver;


    private MyHand hand;
    public Card info { get; private set; }
    private bool active = true;

    private void Start()
    {

        hand = FindObjectOfType<MyHand>();
        Rule();

        turnObserver = new ObserverBot(Rule);
        Broadcaster.TurnChangedChannel.AddObserver(turnObserver);

        gameoverObserver = new ObserverBot(() => Active(false));
        Broadcaster.GameOverChannel.AddObserver(gameoverObserver);
    }
    public void OnMouseDown()
    {
        if (!active) return;
        
            Focus.Singleton.GiveTo(info);
            Broadcaster.TurnChangedChannel.RemoveObserver(turnObserver);
            Destroy(gameObject);
        
    }
    private void OnMouseEnter()
    {

        hand.Sort();
        GetComponent<SpriteRenderer>().sortingOrder = -32001;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -32000;
    }

    public void Active(bool _is)
    {
        active = _is;
        if(disableSprite!=null)
      disableSprite.SetActive(!_is);
    }
    public void Init(Card info)
    {
        getani.Play("Anim_Me_Getani");
        this.info = info;
        var render = GetComponent<SpriteRenderer>();
        render.sprite = CardSprite.Singleton.GetSprite(info);
        

    }

    public void Rule()
    { 
        if (!TurnSystem.Singleton.isMyTurn)
        {
            Active(false);
            return;
        }
        
        Active(RuleSystem.Rule(info));
    }

}
