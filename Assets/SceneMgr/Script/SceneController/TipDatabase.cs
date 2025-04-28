using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TipsData", menuName = "Data/TipDatabase", order = 1)]
public class TipDatabase : ScriptableObject
{
	public string[] tips;
}

