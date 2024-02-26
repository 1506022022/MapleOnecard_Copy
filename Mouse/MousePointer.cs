using UnityEngine;

public class MousePointer : MonoBehaviour
{
    [SerializeField]
    private Sprite nomalMouse;
    [SerializeField]
    private Sprite nomalMouseDown;
    [SerializeField]
    private Sprite clickMouse;

    private Rect window;
    private SpriteRenderer mouse;
    
    
    void Awake()
    {
        mouse = GetComponent<SpriteRenderer>();
        window.height = 2 * Camera.main.orthographicSize;
        window.width = window.height * Camera.main.aspect;
    }
    private void Update()
    {
        LockMouseWindow();

        if (Input.GetMouseButtonDown(0))
            mouse.sprite = nomalMouseDown;
        
        else if (Input.GetMouseButtonUp(0))
            mouse.sprite = nomalMouse;
        
    }
    private void LockMouseWindow()
    {
        var mouse = Input.mousePosition / 100f;
        if (mouse.x > window.width) mouse.x = window.width;
        else if (mouse.x < 0) mouse.x = 0;
        if (mouse.y > window.height) mouse.y = window.height;
        else if (mouse.y < 0) mouse.y = 0;

        transform.position = mouse;
    }
}
