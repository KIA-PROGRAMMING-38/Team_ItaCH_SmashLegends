using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataTableManager : MonoBehaviour
{
    private DataTable dataTable;

    private void Awake()
    {
        dataTable = new DataTable();
        dataTable.SetDataTable();
    }
}
