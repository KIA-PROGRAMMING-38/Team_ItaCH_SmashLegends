using UnityEngine;
public class UI_GameResultSubItem : UIBase
{
    enum Texts
    {
        UserNameText
    }

    private GameObject _selectedLegendModel;
    private TeamType _team;
    public override void Init()
    {
        BindText(typeof(Texts));
    }

    public void SetInfo(UserData user, Transform legendModelSpawnPoint)
    {
        this._team = user.TeamType;
        this.GetComponent<RectTransform>().anchoredPosition = Define.GAME_RESULT_USER_PROFILE_POSITIONS[(int)user.TeamType];
        GetText((int)Texts.UserNameText).text = user.Name;
        SetGameResultLegendModel(user.SelectedLegend, legendModelSpawnPoint);
    }

    private void SetGameResultLegendModel(LegendType legend, Transform spawnPoint)
    {
        GameObject legendModel = Managers.ResourceManager.GetLobbyLegendPrefab(legend);
        _selectedLegendModel = Managers.ResourceManager.Instantiate(legendModel, spawnPoint);
        _selectedLegendModel.transform.localScale = new Vector3(100, 100, 100);
        _selectedLegendModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public void RefreshSubItem(TeamType winnerTeam)
    {
        Animator legendModelAnimator = _selectedLegendModel.GetComponent<Animator>();
        if (this._team == winnerTeam)
        {
            legendModelAnimator.SetTrigger(Animator.StringToHash("WinGame"));
        }
        else
        {
            legendModelAnimator.SetTrigger(Animator.StringToHash("LoseGame"));
        }
    }
}