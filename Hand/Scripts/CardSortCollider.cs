using UnityEngine;

public class CardSortCollider : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer card;

    private void OnMouseEnter()
    {
        card.sortingOrder = -32730;
    }
    private void OnMouseExit()
    {
        card.sortingOrder = -32731;
    }
}
