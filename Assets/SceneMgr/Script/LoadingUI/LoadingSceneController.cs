using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using YellowCat.SceneMgr;

public class LoadingSceneController : MonoBehaviour
{
	[SerializeField] private Slider _totalProgressBar;
	[SerializeField] private TMPro.TextMeshProUGUI _totalProgress;

	[SerializeField] private Slider _taskProgressBar;
	[SerializeField] private TMPro.TextMeshProUGUI _taskProgress;
	[SerializeField] private TMPro.TextMeshProUGUI _taskName;

	//private void Start()
	//{
	//	StartCoroutine(TrackLoadingProgress());
	//}

	public void UpdateProgress(float progress, string taskName, float taskProgress)
	{
		Debug.Log($"[LoadingSceneController] UpdateProgress > progress:{progress} > taskName: {taskName} > taskProgress: {taskProgress}");
		//Total Progress Bar
		progress = Mathf.Clamp01(progress);
		int progressPercent = (int)(progress * 100);
		_totalProgressBar.value = progress;
		_totalProgress.text = $"{progressPercent}%";

		//Task Progress Bar
		taskProgress = Mathf.Clamp01(taskProgress);
		int taskProgressPercent = (int)(taskProgress * 100);
		_taskProgressBar.value = taskProgress;
		_taskProgress.text = $"{taskProgressPercent}%";
		_taskName.text = $"{taskName}...";
	}

	//	private IEnumerator TrackLoadingProgress()
	//	{
	//		var loadingOp = SceneMgr.Instance.CurrentLoadingOperation;
	//		if (loadingOp == null)
	//		{
	//			Debug.LogError("No Loading Operation found!");
	//			yield break;
	//		}

	//		updateProgress(0f, "Loading");

	//		// Phase 1: Load Scene File
	//		while (loadingOp.progress < 0.9f)
	//		{
	//			updateProgress(loadingOp.progress * 0.5f, "Loading");
	//			yield return null;
	//		}
	//		updateProgress(0.5f, "Loading");

	//		yield return new WaitForSeconds(0.2f);

	//		SceneMgr.Instance.ActivateLoadedScene();

	//		yield return new WaitUntil(() => { return loadingOp.isDone; });

	//		// Phase 2: Load Resource (Optional)

	//		Debug.Log("Find Loader");
	//		ISceneLoadable loadable = FindLoadable();
	//		if (loadable != null)
	//		{
	//			Debug.Log("Find Loader success!!");
	//			SmartCoroutineLoader loadCoroutine = new SmartCoroutineLoader(loadable.LoadAsync());
	//			StartCoroutine(loadCoroutine.RunCoroutine());
	//			while (!loadCoroutine.IsDone)
	//			{
	//				//updateProgress((float)loadCoroutine.Progress, loadCoroutine.CurrentTaskName);
	//				var progress = Mathf.Clamp01((float)loadCoroutine.Progress);
	//				updateProgress(0.5f + progress * 0.5f, "Loading");
	//				yield return null;
	//			}
	//		}
	//		else
	//		{
	//			Debug.LogWarning("Loader is NOT FOUND!!");
	//			// Không có gì để load thêm
	//			progressBar.value = 1.0f;
	//			yield return new WaitForSeconds(0.5f);
	//		}

	//		// Finish Loading
	//		yield return new WaitForSeconds(0.5f);
	//		SceneMgr.Instance.UnloadScene("LoadingScene");
	//	}

	//	private ISceneLoadable FindLoadable()
	//	{
	//		MonoBehaviour[] behaviours = FindObjectsOfType<MonoBehaviour>(true);
	//		foreach (var b in behaviours)
	//		{
	//			Debug.Log("FindLoadable " + b.GetType().Name);
	//			if (b is ISceneLoadable loadable)
	//				return loadable;
	//		}
	//		return null;
	//	}

	//	private void updateProgress(float progress, string taskname)
	//	{
	//		float resourceProgress = Mathf.Clamp01(progress);
	//		progressBar.value = resourceProgress;
	//		var progressPercent = resourceProgress * 100;
	//		taskName.text = $"{taskname}... {progressPercent}%";
	//	}
}
