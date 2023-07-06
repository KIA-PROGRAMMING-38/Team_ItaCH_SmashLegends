using System.Collections.Generic;
using UnityEngine;
using Util.CharacterData;

public class DataTable
{
    public Dictionary<int, CharacterDefaultData> CharacterTable = new();
    
    public void SetDataTable()
    {
        SetCharacterData();
    }
    private void SetCharacterData()
    {
        TextAsset characterCSV = Resources.Load<TextAsset>(ResourcesPath.CharacterDataTable);
        string[] characters = characterCSV.text.Split('\n');

        for (int i = 1; i < characters.Length; ++i)
        {
            string[] line = characters[i].Split(',');
            CharacterDefaultData data = new();

            data.legendId = int.Parse(line[0]);
            data.skillGauge = int.Parse(line[1]);
            data.skillRecovery = int.Parse(line[2]);
            data.moveSpeed = float.Parse(line[3]);
            data.jumpAcceleration = float.Parse(line[4]);
            data.gravitationalAcceleration = float.Parse(line[5]);
            data.maxFallingSpeed = int.Parse(line[6]);
            data.size = int.Parse(line[7]);
            data.hp = int.Parse(line[8]);
            data.defaultAttackDamage = int.Parse(line[9]);
            data.jumpAttackDamage = int.Parse(line[10]);
            data.heavyAttackDamage = int.Parse(line[11]);
            data.skillAttackDamage = int.Parse(line[12]);
            data.dashPower = float.Parse(line[13]);
            data.defaultKnockbackPower = float.Parse(line[14]);
            data.heavyKnockbackPower = float.Parse(line[15]);
            data.heavyCooltime = float.Parse(line[16]);
            
            CharacterTable.Add(data.legendId, data);
        }
    }
}
