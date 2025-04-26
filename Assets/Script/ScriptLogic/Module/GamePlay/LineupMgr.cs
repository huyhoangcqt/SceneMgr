using JetBrains.Annotations;
using UnityEngine;

public class LineUpMgr : Singleton<LineUpMgr>
{
    public LineUp playerLineup;
    public LineUp enemyLineup;

    public LineUpMgr()
    {
        Init();
    }

    private void Init()
    {
        // GameObject lineupsNode = GameObject.Find(GameNode.BattleSceneNode.Lineups);
        GameObject playerLineupNode = GameObject.Find(GameNode.BattleSceneNode.PlayerLineup);
        GameObject enemyLineupNode = GameObject.Find(GameNode.BattleSceneNode.EnemyLineup);
        playerLineup = new LineUp(playerLineupNode);
        enemyLineup = new LineUp(enemyLineupNode);
        playerLineup.HideShowObjects();
        enemyLineup.HideShowObjects();
    }
}