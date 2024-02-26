using ObserverPattern;
using System.Collections;
using UnityEngine;

public class ClickSpritesAnim : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sprites;
    [SerializeField]
    private float wait = 0.3f;


    private int index;



    private void OnEnable()
    {
        index = 0;

        StartCoroutine(Anim());
    }
    private IEnumerator Anim()
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            if (index >= sprites.Length) index = 0;
            for (int i = 0; i < sprites.Length; i++)
            {
                if (i == index)
                    sprites[i].SetActive(true);
                else
                    sprites[i].SetActive(false);
            }
            index++;
        }
    }
}
