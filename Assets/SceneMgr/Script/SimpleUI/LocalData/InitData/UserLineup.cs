using System.Collections.Generic;
using ExcelData;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UserLineup
{
    public List<string> userLineup;

    public void Initialization()
    {
        userLineup = new List<string>(){"c01","c02","c03","c04","c05","c06"};
    }
}