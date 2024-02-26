using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField]
    private int def;
    public int Def
    {
        get { return def; }
        set { 
                def = value;
                if (def <= 0) Destroy(gameObject);
            }
    }


    public SlotBoard player;

    private void Start()
    {
        if (AttackFlame.Singleton.Count > 0)
        {
            AttackFlame.Singleton.Count = 0;
            Destroy(gameObject);
        }

    }
    public void Init(SlotBoard player)
    {
        this.player = player;
        Def = 2;
    }
}
