using System;
using UnityEngine;

public class CharacterTestModule : MonoBehaviour
{
    
    private void Awake()
    {
        CharacterStatus characterStatus = transform.root.GetComponent<CharacterStatus>();

        characterStatus.dataTable = new DataTable();
        characterStatus.dataTable.SetDataTable();
        characterStatus.InitCharacterType(characterStatus.CharacterType);
        characterStatus.InitStatus();
    }
    // 앨리스Bomb Hit 관련 임시 코드
    //private void BombHit(Collider other, int AnimationHash)
    //{
    //    Rigidbody rigidbody = other.GetComponent<Rigidbody>();
    //    Animator animator = other.GetComponent<Animator>();
    //    rigidbody.AddForce(_knockBackDirection * _knockBackPower, ForceMode.Impulse);
    //    animator.SetTrigger(AnimationHash);
    //}
}
