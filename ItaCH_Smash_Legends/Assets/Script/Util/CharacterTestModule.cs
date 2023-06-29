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
    // �ٸ���Bomb Hit ���� �ӽ� �ڵ�
    //private void BombHit(Collider other, int AnimationHash)
    //{
    //    Rigidbody rigidbody = other.GetComponent<Rigidbody>();
    //    Animator animator = other.GetComponent<Animator>();
    //    rigidbody.AddForce(_knockBackDirection * _knockBackPower, ForceMode.Impulse);
    //    animator.SetTrigger(AnimationHash);
    //}
}
