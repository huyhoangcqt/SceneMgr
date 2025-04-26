using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private static GameLauncher _instance;
    public static GameLauncher Instance => _instance;
    private void Awake() {
        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        /**
         * 1. Tải bundle tài nguyên cơ bản cho MainUI.
         * => Quá trình được chạy ngay LoadingScene.
         * 2. Sau khi tải tài nguyên thành công.
         * Tiến hành khởi tạo MainScene.
         * 3. Sau khi khởi tạo thành công => Start GameManager.
        */ 
       GameManager.Instance.Start();
    }
}
