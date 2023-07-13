using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Util.Enum;

public class CharacterStatus : MonoBehaviour
{
    public int CurrentHP { get => _currentHealthPoint; }
    public int CurrentHPRatio { get => (_currentHealthPoint * 100) / _maxHealthPoint; }
    private int _currentHealthPoint;
    private int _maxHealthPoint;
    private const int DEAD_TRIGGER_HP = 0;
    internal bool _isDead = false;
    // TO DO : health 관련 Legend Controller에서 stat 관리

    // TO DO : StageManager와 LegendController가 서로 관여
    private float _currentRespawnTime;
        
    public string Name { get; set; } // TO DO : UI 로직 수정 이후 없어질 프로퍼티
    
    public event Action<int, int> OnPlayerHealthPointChange;
    public void Init(UserData userData) // TO DO : UI 로직 수정 이후 전부 삭제
    {
        Name = userData.Name;
        //Character = userData.SelectedCharacter; // TO DO : 체력 감소 로직 수정 필요
        //_maxHealthPoint = Stat.HP;
        SetDefaultHP();
    }

    public void SetDefaultHP()
    {
        _currentHealthPoint = _maxHealthPoint * 10;
        OnPlayerHealthPointChange?.Invoke(_currentHealthPoint, CurrentHPRatio);
        _currentHealthPoint = _maxHealthPoint;
    }
    // TO DO : stageManager로 이식, 죽었을 때 호출.Forget();

    //private async UniTaskVoid RespawnAsync(float respawnTime)
    //{
    //    await UniTask.Delay((int)respawnTime * 1000);
    //    Respawn();
    //}

    //public void Respawn()
    //{
    //    this.transform.position = SpawnPoint;
    //    this.gameObject.SetActive(true);
    //    this.GetComponent<Collider>().isTrigger = false;
    //    this._isDead = false;
    //    OnPlayerRespawn?.Invoke(this);
    //    OnRespawnSetting?.Invoke(this, PlayerID);
    //    SetDefaultHP();
    //} 
}
