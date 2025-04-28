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

	public void UpdateProgress(float progress, string taskName, float taskProgress)
	{
		Debuger.Log($"[LoadingSceneController] UpdateProgress > progress:{progress} > taskName: {taskName} > taskProgress: {taskProgress}");
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
}
