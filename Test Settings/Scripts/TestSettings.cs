using UnityEngine;


public class TestSettings : MonoBehaviour
{
    [SerializeField]
    private float gameSpeed;
    private float gamespeed;

    private void Update()
    {
        if (gamespeed != gameSpeed)
        {
            Time.timeScale = gameSpeed;
            gamespeed = gameSpeed;
        }
    }
    private void Awake()
    {
        Cursor.visible = false;
    }

}
