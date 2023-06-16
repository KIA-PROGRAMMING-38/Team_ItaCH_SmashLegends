using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnablePanelButton : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private Button _button;

    //�׽�Ʈ �ڵ�. ���� �ʿ��� �κп��� ȣ���� ����
    private void Start()
    {
        InitEnablePanelButtonSettings();
    }
    public void InitEnablePanelButtonSettings()
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(EnablePanel);
        _button.onClick.AddListener(() => SoundManager._instance.Play("EnablePanel"));
    }
    public void EnablePanel()
    {
        _panel.SetActive(true);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
