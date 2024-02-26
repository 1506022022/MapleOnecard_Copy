using System.Collections;
using UnityEngine;

public class Spriter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sprites;
    [SerializeField]
    private float wait = 0.3f;

    public void StartAnim()
    {
        StartCoroutine(Anim());
    }

    private IEnumerator Anim()
    {
        for (int index = 0; index < sprites.Length; index++)
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
        }
        sprites[sprites.Length - 1].SetActive(false);
        
    }


}
