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
}
