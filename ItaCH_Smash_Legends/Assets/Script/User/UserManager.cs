using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public UserData UserData { get => _userData; set => _userData = value; }
    private UserData _userData;
    void Start()
    {
        _userData = new UserData();        
    }    
}
