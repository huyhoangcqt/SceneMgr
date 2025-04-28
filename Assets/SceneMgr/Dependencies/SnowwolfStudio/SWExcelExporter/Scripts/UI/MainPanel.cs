using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Snowwolf;

public class MainPanel : MonoBehaviour
{
    private ScrollRect m_FilesSV;
    private Transform m_FilesSVContent;
    private Transform m_FilesSVItem;

    private ScrollRect m_LogSV;
    private Transform m_LogSVContent;
    private Transform m_LogSVItem;

    private Button m_SelAllButton;
    private Button m_SelNoneButton;
    private Button m_ReverseSelButton;
    private Button m_StartButton;
    private Button m_ListButton;
    private Button m_SaveConfigButton;

    private Text m_SelectText;
    private InputField m_ExcelPathInputField;
    private InputField m_ClientPathInputField;
    private InputField m_ServerPathInputField;

    private Transform m_ClientExportersLayout;
    private Transform m_ServerExportersLayout;
    private GameObject[] m_ClientExporterItems;
    private GameObject[] m_ServerExporterItems;

    private GameObject[] m_FileViewItems = null;

    private bool m_NeedUpdateLogView = false;
    private int m_LogScrollWaitFrame = 0;

    private List<string> m_Logs = new List<string>();
    private List<string> m_Files = new List<string>();
    private List<string> m_SelFiles = new List<string>();

    private bool m_IsExporting = false;

    void Awake()
    {
        Transform contentTrans = transform.Find("Content");

        m_FilesSV = contentTrans.Find("FilesSV").GetComponent<ScrollRect>();
        m_FilesSVContent = m_FilesSV.content;
        m_FilesSVItem = m_FilesSVContent.Find("Item");

        m_LogSV = contentTrans.Find("LogSV").GetComponent<ScrollRect>();
        m_LogSVContent = m_LogSV.content;
        m_LogSVItem = m_LogSVContent.Find("Text");

        m_SelAllButton = contentTrans.Find("SelAllButton").GetComponent<Button>();
        m_SelNoneButton = contentTrans.Find("SelNoneButton").GetComponent<Button>();
        m_ReverseSelButton = contentTrans.Find("ReverseSelButton").GetComponent<Button>();
        m_StartButton = contentTrans.Find("StartButton").GetComponent<Button>();
        m_ListButton = contentTrans.Find("ListButton").GetComponent<Button>();
        m_SaveConfigButton = contentTrans.Find("SaveConfigButton").GetComponent<Button>();

        m_SelectText = contentTrans.Find("SelectText").GetComponent<Text>();
        m_ExcelPathInputField = contentTrans.Find("ExcelPathInputField").GetComponent<InputField>();
        m_ClientPathInputField = contentTrans.Find("ClientPathInputField").GetComponent<InputField>();
        m_ServerPathInputField = contentTrans.Find("ServerPathInputField").GetComponent<InputField>();

        m_ClientExportersLayout = contentTrans.Find("ClientExporters");
        m_ServerExportersLayout = contentTrans.Find("ServerExporters");

        AddEvents();
    }

    void Start()
    {
        UpdateAllViews();
    }

    void LateUpdate()
    {
        if (m_NeedUpdateLogView)
        {
            UpdateLogView();
        }
        if (m_LogScrollWaitFrame > 0)
        {
            --m_LogScrollWaitFrame;
            if (m_LogSV && m_LogScrollWaitFrame == 0)
            {
                m_LogSV.verticalNormalizedPosition = 0f;
            }
        }
    }

    void OnDestroy()
    {
        RemoveEvents();
    }

    void AddEvents()
    {
        Application.logMessageReceived += OnLogRecieve;
        m_SelAllButton.onClick.AddListener(OnSelAllButtonClick);
        m_SelNoneButton.onClick.AddListener(OnSelNoneButtonClick);
        m_ReverseSelButton.onClick.AddListener(OnReverseSelButtonClick);
        m_StartButton.onClick.AddListener(OnStartButtonClick);
        m_ListButton.onClick.AddListener(OnListButtonClick);
        m_SaveConfigButton.onClick.AddListener(OnSaveConfigButtonClick);

        m_ExcelPathInputField.onEndEdit.AddListener(OnExcelPathEndEdit);
    }

    void RemoveEvents()
    {
        m_ExcelPathInputField.onEndEdit.RemoveAllListeners();

        m_SaveConfigButton.onClick.RemoveAllListeners();
        m_ListButton.onClick.RemoveAllListeners();
        m_StartButton.onClick.RemoveAllListeners();
        m_ReverseSelButton.onClick.RemoveAllListeners();
        m_SelNoneButton.onClick.RemoveAllListeners();
        m_SelAllButton.onClick.RemoveAllListeners();
        Application.logMessageReceived -= OnLogRecieve;
    }

    void OnExcelPathEndEdit(string text)
    {
        UpdateFiles();
    }

    void OnLogRecieve(string condition, string stackTrace, UnityEngine.LogType type)
    {
        string fullMessage;
        if (string.IsNullOrEmpty(stackTrace) || (type == UnityEngine.LogType.Log))
        {
            fullMessage = string.Format("[{0}]{1}", type, condition);
        }
        else
        {
            fullMessage = string.Format("[{0}]{1}\n{2}", type, condition, stackTrace);
        }
        m_Logs.Add(fullMessage);
        m_NeedUpdateLogView = true;
    }

    void OnStartButtonClick()
    {
        if (m_IsExporting){ return ;}

        GetSelectedFiles(m_SelFiles);
        if (m_SelFiles.Count == 0)
        {
            Debug.Log("Please select excel files to export.");
            return;
        }

        string clientPath = m_ClientPathInputField.text;
        string serverPath = m_ServerPathInputField.text;
        if (string.IsNullOrEmpty(clientPath) && string.IsNullOrEmpty(serverPath))
        {
            Debug.Log("Client and server path should be used at lease one.");
            return;
        }

        var usingClientExporters = GetSelectedExporters(false);
        var usingServerExporters = GetSelectedExporters(true);

        SavePreferences();
        StartCoroutine(StartExport(m_SelFiles, clientPath, serverPath, usingClientExporters, usingServerExporters));
    }

    void OnListButtonClick()
    {
        UpdateFiles();
    }

    void OnSaveConfigButtonClick()
    {
        SavePreferences();
    }


    void SetAllTogglesInBatch(bool isOn, bool isReverse = false)
    {
        if (m_FileViewItems == null) { return; }
        for(int i = 0, cnt = m_FileViewItems.Length; i < cnt; ++i)
        {
            Toggle toggle = m_FileViewItems[i].transform.Find("Toggle").GetComponent<Toggle>();
            toggle.isOn = isReverse ? !toggle.isOn : isOn;
        }
    }

    void OnReverseSelButtonClick()
    {
        SetAllTogglesInBatch(true, true);
    }

    void OnSelAllButtonClick()
    {
        SetAllTogglesInBatch(true, false);
    }

    void OnSelNoneButtonClick()
    {
        SetAllTogglesInBatch(false, false);
    }

    void ClearLogs()
    {
        m_Logs.Clear();
        UpdateLogView();
    }

    void SavePreferences()
    {
        SetSelFilesToPref(m_SelFiles);
        SetSelExportersToPref();
        var userPrefs = UserPreferences.GetOrLoad();
        userPrefs.excelPath = m_ExcelPathInputField.text;
        userPrefs.clientPath = m_ClientPathInputField.text;
        userPrefs.serverPath = m_ServerPathInputField.text;
        UserPreferences.Save();
        Debug.Log("Preferences saved.");
    }

    void UpdateAllViews()
    {
        UpdatePathTexts();
        UpdateLogView();
        UpdateFiles();
        UpdateExporters();
    }

    void UpdateSelectText()
    {
        int allFilesCount = m_Files.Count;
        int selFilesCount = GetSelectedFiles(m_SelFiles).Count;

        m_SelectText.text = string.Format("{0}/{1}", selFilesCount, allFilesCount);
    }

    void UpdatePathTexts()
    {
        var userPref = UserPreferences.GetOrLoad();
        m_ExcelPathInputField.text = userPref.excelPath;
        m_ClientPathInputField.text = userPref.clientPath;
        m_ServerPathInputField.text = userPref.serverPath;
    }

    void UpdateFiles()
    {
        if (!m_FilesSV) { return; }
        UserPreferences userPref = UserPreferences.GetOrLoad();
        m_Files.Clear();
        m_Files.AddRange(FileUtils.GetAllExcelFiles(m_ExcelPathInputField.text));
        List<string> settingSelFiles = userPref.lastSelectedExcelFiles;

        m_FileViewItems = UIUtils.GetChildren(m_FilesSVContent.gameObject, m_FilesSVItem.gameObject, m_Files.Count);
        for (int i = 0, cnt = m_FileViewItems.Length; i < cnt; ++i)
        {
            string fileName = Path.GetFileName(m_Files[i]);
            Transform fileViewItemTrans = m_FileViewItems[i].transform;
            Toggle toggle = fileViewItemTrans.Find("Toggle").GetComponent<Toggle>();
            Text label = fileViewItemTrans.Find("Toggle/Label").GetComponent<Text>();

            label.text = fileName;
            toggle.isOn = settingSelFiles.Contains(fileName);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(OnSelFileChanged);
        }
        UpdateSelectText();
    }

    void UpdateExporters()
    {
        m_ClientExporterItems = UpdateExporters(m_ClientExportersLayout, false);
        m_ServerExporterItems = UpdateExporters(m_ServerExportersLayout, true);
    }

    void UpdateLogView()
    {
        if (!m_LogSV) { return ;}
        GameObject[] logViewItems = UIUtils.GetChildren(m_LogSVContent.gameObject, m_LogSVItem.gameObject, m_Logs.Count);
        for (int i = 0, cnt = logViewItems.Length; i < cnt; ++i)
        {
            GameObject logViewItem = logViewItems[i];
            Text textComp = logViewItem.GetComponent<Text>();
            string log = m_Logs[i];
            if (log.StartsWith("[Error]") || log.StartsWith("Exception"))
            {
                textComp.color = Color.red;
            }
            else if (log.StartsWith("[Warning]"))
            {
                textComp.color = Color.yellow;
            }
            else
            {
                textComp.color = Color.white;
            }
            textComp.text = log;
        }
        m_NeedUpdateLogView = false;
        m_LogScrollWaitFrame = 2;
    }

    List<string> GetSelectedFiles(List<string> selFiles)
    {
        selFiles.Clear();
        if ((m_FileViewItems == null) || (m_FileViewItems.Length == 0)){ return selFiles; }

        for(int i = 0, cnt = m_FileViewItems.Length; i < cnt; ++i)
        {
            Toggle toggle = m_FileViewItems[i].transform.Find("Toggle").GetComponent<Toggle>();
            if (toggle.isOn)
            {
                selFiles.Add(m_Files[i]);
            }
        }
        return selFiles;
    }

    void SetSelFilesToPref(List<string> selFiles)
    {
        var lastSelList = UserPreferences.GetOrLoad().lastSelectedExcelFiles;
        lastSelList.Clear();
        for (int i = 0, cnt = selFiles.Count; i < cnt; i++)
        {
            lastSelList.Add(Path.GetFileName(selFiles[i]));
        }
    }

    void OnSelFileChanged(bool isOn)
    {
        UpdateSelectText();
    }

    void SetSelExportersToPref()
    {
        var clientUsingExporters = UserPreferences.GetOrLoad().clientUsingExporters;
        clientUsingExporters.Clear();
        clientUsingExporters.AddRange(GetSelectedExporters(false));

        var serverUsingExporters = UserPreferences.GetOrLoad().serverUsingExporters;
        serverUsingExporters.Clear();
        serverUsingExporters.AddRange(GetSelectedExporters(true));
    }

    List<string> GetSelectedExporters(bool forServer)
    {
        List<string> selectedExporters = new List<string>();
        var Items = forServer ? m_ServerExporterItems : m_ClientExporterItems;
        if ((Items == null) || (Items.Length == 0)){ return selectedExporters; }

        var supportedExporters = Exporters.GetSupportedExporters();

        for(int i = 0, cnt = Items.Length; i < cnt; ++i)
        {
            Toggle toggle = Items[i].transform.GetComponent<Toggle>();
            if (toggle.isOn)
            {
                selectedExporters.Add(toggle.gameObject.name);
            }
        }
        return selectedExporters;
    }

    GameObject[] UpdateExporters(Transform layoutTrans, bool forServer)
    {
        if (layoutTrans == null) { return null; }

        var supportedExporters = Exporters.GetSupportedExporters();
        var usingExporters = forServer ? UserPreferences.GetOrLoad().serverUsingExporters : UserPreferences.GetOrLoad().clientUsingExporters;

        GameObject[] exporterItems = UIUtils.GetChildren(layoutTrans.gameObject, layoutTrans.Find("Toggle").gameObject, supportedExporters.Length);
        for (int i = 0, cnt = exporterItems.Length; i < cnt; ++i)
        {
            var exporter = supportedExporters[i];
            Transform fileViewItemTrans = exporterItems[i].transform;
            Toggle toggle = fileViewItemTrans.GetComponent<Toggle>();
            Text label = fileViewItemTrans.Find("Label").GetComponent<Text>();

            label.text = exporter.displayName;
            toggle.gameObject.name = exporter.name;
            toggle.isOn = (usingExporters != null) && (usingExporters.Contains(exporter.name));
        }
        return exporterItems;
    }

    IEnumerator StartExport(List<string> selFiles, string clientPath, string serverPath, List<string> clientExporters, List<string> serverExporters)
    {
        m_IsExporting = true;
        ClearLogs();
        yield return null;
        Debug.LogFormat("->Start to export {0} files.", selFiles.Count);
        yield return null;

        System.Exception exception = null;
        IEnumerator subCoroutine;

        if (!string.IsNullOrEmpty(clientPath))
        {
            Debug.Log("-->Start export files for client.");
            subCoroutine = Workflow.ExportFiles(selFiles.ToArray(), clientPath, clientExporters.ToArray(), false);
            while(true)
            {
                try
                {
                    if (!subCoroutine.MoveNext()) { break; }
                }
                catch(System.Exception e)
                {
                    exception = e;
                    break;
                }
                yield return null;
            }
            if (exception == null)
            {
                Debug.Log("<--Finish exporting for client.");
            }
        }
        if ((exception == null) && !string.IsNullOrEmpty(serverPath))
        {
            Debug.Log("-->Start export files for server.");
            subCoroutine = Workflow.ExportFiles(selFiles.ToArray(), serverPath, serverExporters.ToArray(), true);
            while(true)
            {
                try
                {
                    if (!subCoroutine.MoveNext()) { break; }
                }
                catch(System.Exception e)
                {
                    exception = e;
                    break;
                }
                yield return null;
            }
            if (exception == null)
            {
                Debug.Log("<--Finish exporting for server.");
            }
        }

        if (exception != null)
        {
            Debug.LogError(exception);
            Debug.LogError("<-Export Aborted!");
            m_IsExporting = false;
            yield break;
        }

        Debug.LogFormat("<-Export Finished.");
        m_IsExporting = false;
    }
}
