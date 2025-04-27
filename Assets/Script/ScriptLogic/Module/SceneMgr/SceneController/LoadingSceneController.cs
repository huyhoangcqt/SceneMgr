using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using YellowCat.SceneMgr;

public class LoadingSceneController : MonoBehaviour
{
	[SerializeField] private Slider progressBar;
	[SerializeField] private TMPro.TextMeshProUGUI taskName;

	private void Start()
	{
		StartCoroutine(TrackLoadingProgress());
	}

	private IEnumerator TrackLoadingProgress()
	{
		var loadingOp = SceneMgr.Instance.CurrentLoadingOperation;
		if (loadingOp == null)
		{
			Debug.LogError("No Loading Operation found!");
			yield break;
		}

		updateProgress(0f, "Loading");

		// Phase 1: Load Scene File
		while (loadingOp.progress < 0.9f)
		{
			updateProgress(loadingOp.progress * 0.5f, "Loading");
			yield return null;
		}
		updateProgress(0.5f, "Loading");

		yield return new WaitForSeconds(0.2f);

		SceneMgr.Instance.ActivateLoadedScene();

		yield return new WaitUntil(() => { return loadingOp.isDone; });

		// Phase 2: Load Resource (Optional)

		Debug.Log("Find Loader");
		ISceneLoadable loadable = FindLoadable();
		if (loadable != null)
		{
			Debug.Log("Find Loader success!!");
			SmartCoroutineLoader loadCoroutine = new SmartCoroutineLoader();
			StartCoroutine(loadCoroutine.RunCoroutine(loadable.LoadAsync()));
			while (!loadCoroutine.IsDone)
			{
				//updateProgress((float)loadCoroutine.Progress, loadCoroutine.CurrentTaskName);
				var progress = Mathf.Clamp01((float)loadCoroutine.Progress);
				updateProgress(0.5f + progress * 0.5f, "Loading");
				yield return null;
			}
		}
		else
		{
			Debug.LogWarning("Loader is NOT FOUND!!");
			// Không có gì để load thêm
			progressBar.value = 1.0f;
			yield return new WaitForSeconds(0.5f);
		}

		// Finish Loading
		yield return new WaitForSeconds(0.5f);
		SceneMgr.Instance.UnloadScene("LoadingScene");
	}

	private ISceneLoadable FindLoadable()
	{
		MonoBehaviour[] behaviours = FindObjectsOfType<MonoBehaviour>(true);
		foreach (var b in behaviours)
		{
			Debug.Log("FindLoadable " + b.GetType().Name);
			if (b is ISceneLoadable loadable)
				return loadable;
		}
		return null;
	}

	private void updateProgress(float progress, string taskname)
	{
		float resourceProgress = Mathf.Clamp01(progress);
		progressBar.value = resourceProgress;
		var progressPercent = resourceProgress * 100;
		taskName.text = $"{taskname}... {progressPercent}%";
	}
}
