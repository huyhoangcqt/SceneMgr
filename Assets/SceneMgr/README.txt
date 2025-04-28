SceneMgr

Features:

Setup:
1. Kéo các scene vào build setting.
2. Mở GameLauncherScene => Play

How to Custom your Own:
1. Add New Scene:
step 1. Add new Enum to "Scene" enum in file: "SceneMgr.cs"
	with the name of enum is same as your new SceneName
step 2. Create new scene state like: "MainModule/StateMachine/BattleSceneState.cs"
	with your own naming, extend  : BaseState class
	=> Or you can Duplicate "BattleSceneState.cs" and rename.
step 3. Register State with SceneMgr in registerStates() function.

2. Custom your own PreLoadAsset flow.
<Some taskAsync, i still didn't test it>
But current:
Step 1. you can override void MakeSequenceAsync() method and do your own.
Step 2. custom your Corouine.
- Some NOTE: yield return float. like:
	+ yield return 0.2f;
	+ yield return 0.3f;
	+ yield return 0.5f;
=> give me a note for current coroutine progress.
=> I will estimate progress if you using other. like:
	+ yield return Other_Coroutine.
	+ yield return AsyncOperation
=> with Coroutine => I wrap and process it with SmartCoroutine.
=> You can custom your own at: SmartCoroutine.RunCoroutine()

3. Where can I run other script?
- When scene has been active => Monobehaviours will active too => Awave, OnEnable, Start
- If you want to call other script type which is not Monobehaviours
	=> override: "OnLoadComplete(string sceneName)" trong interface "IStateController".

Dependencies:
- TextMeshPro
- SnowwolfStudio (SnowwolfStudio) 
=> href: https://github.com/JasonAtGitHub
=> asset: https://assetstore.unity.com/packages/tools/game-toolkits/excelconfigexporter-export-excel-data-to-c-json-lua-etc-in-windo-203754

Chưa test tính năng: (Vietnamese)
- 1.Lồng Operation Sequence bên trong một Operation Sequence khác.
- 2.Bên trong coroutine: chỉ mới test với yield return float, yield return new WaitForSeconds(), chưa test với các tính năng khác.