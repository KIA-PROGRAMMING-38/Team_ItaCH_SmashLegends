using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemovePanelButton : MonoBehaviour
{
    private GameObject _panel;
    private Button _button;
    //�׽�Ʈ �ڵ�. ���� �ʿ��� �κп��� ȣ���� ����
    private void Start()
    {
        InitRemovePanelButtonSettings();
    }
    public void InitRemovePanelButtonSettings()
    {
        _panel = transform.parent.gameObject;
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(RemovePanel);
    }
    public void RemovePanel()
    {
        _panel.SetActive(false);
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
