using ObserverPattern;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndRestart : MonoBehaviour
{
    private ObserverBot gameoverObserver;
    private ObserverBot victoryObserver;
    private SubjectAgent gameendSubject;
    private SubjectAgent restartSubject;

    [SerializeField]
    private string thisScene;

    private void Awake()
    {
        restartSubject = new SubjectAgent();
        Broadcaster.RestartChannel.AddSuject(restartSubject);
        gameendSubject = new SubjectAgent();
        Broadcaster.GameendChannel.AddSuject(gameendSubject);

        reset();
    }
    private void reset()
    {
        gameoverObserver = new ObserverBot(Restart);
        Broadcaster.GameOverChannel.AddObserver(gameoverObserver);

        victoryObserver = new ObserverBot(Restart);
        Broadcaster.VictoryChannel.AddObserver(victoryObserver);


    }
    private async void Restart()
    {
        gameendSubject.Notify();
        await Task.Delay(9900);
        AttackFlame.Singleton.Count = 0;
        await Task.Delay(100);
        Broadcaster.Reset();
        restartSubject.Notify();
        reset();
    }
}
