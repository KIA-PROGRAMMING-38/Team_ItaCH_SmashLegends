using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class UIBase : MonoBehaviour
{
    protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    public virtual void Init()
    {

    }

    private void Start() => Init();
    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected void BindObject(Type type) => Bind<GameObject>(type);
    protected void BindImage(Type type) => Bind<Image>(type);
    protected void BindText(Type type) => Bind<TextMeshProUGUI>(type);
    protected void BindButton(Type type) => Bind<Button>(type);

    protected T Get<T>(int index) where T : Object
    {

        if (_objects.TryGetValue(typeof(T), out Object[] objects))
            return null;

        return objects[index] as T;
    }

    protected GameObject GetObject(int index) => Get<GameObject>(index);
    protected TextMeshProUGUI GetText(int index) => Get<TextMeshProUGUI>(index);
    protected Button GetButton(int index) => Get<Button>(index);
    protected Image GetImage(int index) => Get<Image>(index);

    public static void BindEvent(GameObject go, Action action, UIEventType uiEventType = UIEventType.Click)
    {
        UIEventHandler uiEvent = Utils.GetOrAddComponent<UIEventHandler>(go);

        switch (uiEventType)
        {
            case UIEventType.Click:
                uiEvent.OnClickHandler -= action;
                uiEvent.OnClickHandler += action;

                break;

            case UIEventType.Pressed:
                uiEvent.OnPressedHandler -= action;
                uiEvent.OnPressedHandler += action;

                break;
        }
    }
}