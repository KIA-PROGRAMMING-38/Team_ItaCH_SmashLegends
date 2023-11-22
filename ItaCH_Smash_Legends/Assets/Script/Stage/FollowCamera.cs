using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBrain _brainCamera;
    private Camera _mainCamera;

    private List<GameObject> _legends = new List<GameObject>();

    private void Awake()
    {
        _brainCamera = GetComponent<CinemachineBrain>();
        _mainCamera = GetComponent<Camera>();
    }

    //private void Update()
    //{
    //CheckLegendInCamera();
    //}

    // TODO : 쿼터뷰 캠 구현시 적용
    //private void CheckLegendInCamera()
    //{
    //    for (int index = 0; index < _legends.Count; ++index)
    //    {
    //        Vector3 screenPoint = _mainCamera.WorldToViewportPoint(_legends[index].transform.position);

    //        bool isViewOnLegend = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    //        if (isViewOnLegend)
    //        {
    //              // TODO : 기능내용
    //        }
    //    }
    //}

    public void Init()
    {
        SetCamera();
        SetTargetLegend();

        void SetCamera()
        {
            _virtualCamera = _brainCamera.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            _virtualCamera.Follow = Managers.LobbyManager.UserLocalData.OwnedLegend.gameObject.transform;
        }
    }

    private void SetTargetLegend()
    {
        for (int i = 0; i < Managers.StageManager.CurrentGameMode.Teams[GetTeamType()]
                    .Members.Count; ++i)
        {
            if (Managers.StageManager.CurrentGameMode.Teams[GetTeamType()]
                    .Members[i].OwnedLegend.gameObject != _virtualCamera.Follow.gameObject)
            {
                _legends.Add(Managers.StageManager.CurrentGameMode.Teams[GetTeamType()]
                    .Members[i].OwnedLegend.gameObject);
            }
        }

    }
   
    private int GetTeamType()
    {
        if (Managers.LobbyManager.UserLocalData.TeamType == TeamType.Blue)
        {
            return (int)TeamType.Red;
        }
        else
        {
            return (int)TeamType.Blue;
        }
    }
}
