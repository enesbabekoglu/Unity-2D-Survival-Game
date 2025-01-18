using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    public Texture2D defaultCursor;         // Varsayılan cursor
    public Texture2D handCursor;            // Özel cursor
    public Vector2 cursorHotspot = Vector2.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetDefaultCursor();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<CustomInteractable>() != null)
            {
                SetHandCursor();
                return;
            }
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                if (result.gameObject.GetComponent<CustomInteractable>() != null)
                {
                    SetHandCursor();
                    return;
                }
            }
        }

        SetDefaultCursor();
    }

    private void SetHandCursor()
    {
        Cursor.SetCursor(handCursor, cursorHotspot, CursorMode.Auto);
    }

    private void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }
}
