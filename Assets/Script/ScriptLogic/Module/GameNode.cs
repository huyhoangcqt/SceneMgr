using UnityEngine;
using System;

public class GameNode
{
    public class MainSceneNode
    {
        public const string UINode = "UI";
        public const string MapNode = "Map";
    }

    public class BattleSceneNode
    {
        public const string UIRoot = "UI/GRoot";
        public const string MapNode = "MapBattle";
        public const string Lineups = "MapLineup/Lineups";
        public const string EnemyLineup = "MapLineup/Lineups/EnemyLineup";
        public const string PlayerLineup = "MapLineup/Lineups/PlayerLineup";
    }
}