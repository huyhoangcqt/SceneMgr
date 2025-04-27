name: UnityUI
namespace: YellowCat.UnityUI

=> Logic độc lập.
=> Package nhằm quản lý Window > Component
=> Chỉ phụ thuộc một chỗ: 
	"GameObject prefab = MgrMgr.Resource.LoadWindow(windowName) as GameObject;"
=> Lệnh này cần ResourceMgr để load lên Window cần thiết.
=> Có thể thay thế bằng Resource.Load để giảm phụ thuộc.
