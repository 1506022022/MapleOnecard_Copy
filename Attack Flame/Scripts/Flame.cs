using ObserverPattern;
using System.Collections;
using UnityEngine;

public enum EFlameStae
{
    create= 1,ball,bust
}


public class Flame : MonoBehaviour
{
    [SerializeField]
    private Animator change;
    [SerializeField]
    private SoundControll appearAudio;
    [SerializeField]
    private SoundControll changeAudio;
    [SerializeField]
    private SoundControll burstAudio;

    private Animator anim;
    private Vector3 origine;
    public bool Event { get; private set; }


    private ObserverBot restartObserver;


    private void reset()
    {
        anim = GetComponent<Animator>();
        origine = transform.position;
        Event = false;
    }
    private void Awake()
    {
        restartObserver = new ObserverBot(reset);
        Broadcaster.RestartChannel.AddObserver(restartObserver);

        reset();
    }
    private void OnEnable()
    {
        anim.Play("Empty");
        anim.Play("Anim_Flame_Appear");
        appearAudio.Play();
    }
    public void Attack(Vector3 target)
    {
        Event = true;
        change.Play("Empty");
        change.Play("Anim_Flame_Change");
        changeAudio.Play();
        anim.Play("Empty");
        anim.Play("Anim_Flame_Ball");
        StartCoroutine(Move(target));
        
    }
    private void Explosion()
    {
        Event = false;
        transform.position = origine;
        gameObject.SetActive(false);
    }
    
    private IEnumerator Move(Vector3 target)
    {
        float limit = 0.5f;
        float time = 0f;
        var dis = (target - origine) / limit;
        while (true)
        {
            transform.position += dis * Time.deltaTime; ;
            time += Time.deltaTime;
            if (time>limit)
            {
                anim.Play("Empty");
                anim.Play("Anim_Flame_Buster");
                burstAudio.Play();
                yield break;
            }
            yield return null;
        }
    }
}
