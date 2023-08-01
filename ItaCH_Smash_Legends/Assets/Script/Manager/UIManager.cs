using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager
{
    private static readonly Vector3 DEFAULT_SCALE = Vector3.one;
    private int _canvasOrder = -20;

    private Stack<UIPopup> _popupStack = new Stack<UIPopup>();

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public void Init()
    {

    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _canvasOrder;
            _canvasOrder += 1;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject prefab = Managers.ResourceManager.Load<GameObject>($"Prefab/UI/SubItem/{name}");
        GameObject go = Managers.ResourceManager.Instantiate(prefab);

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        go.transform.localScale = DEFAULT_SCALE;
        go.transform.localPosition = prefab.transform.position;

        return Utils.GetOrAddComponent<T>(go);
    }

    public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject prefab = Managers.ResourceManager.Load<GameObject>($"Prefab/UI/Popup/{name}");
        GameObject go = Managers.ResourceManager.Instantiate($"UI/Popup/{name}");

        T popup = Utils.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }
        else
        {
            go.transform.SetParent(Root.transform);
        }

        go.transform.localScale = Vector3.one;
        go.transform.localPosition = prefab.transform.position;

        return popup;
    }

    public T FindPopup<T>() where T : UIPopup
    {
        return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }

    public T PeekPopupUI<T>() where T : UIPopup
    {
        if (_popupStack.Count == 0)
        {
            return null;
        }

        return _popupStack.Peek() as T;
    }

    public void ClosePopupUI(UIPopup popup)
    {
        if (_popupStack.Count == 0)
        {
            Debug.Assert(false, $"Failed to close {popup}. Popup Stack is empty.");

            return;
        }

        if (_popupStack.Peek() != popup)
        {
            Debug.Assert(false, $"Failed to close {popup}. Top of popup stack is not {popup}.");

            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
        {
            Debug.Assert(false, $"Failed to close PopupUI. Popup Stack is empty.");

            return;
        }

        UIPopup popup = _popupStack.Pop();
        Managers.ResourceManager.Destroy(popup.gameObject);
        _canvasOrder -= 1;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
    }
}
