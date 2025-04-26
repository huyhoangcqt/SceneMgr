using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicListView<T> where T : ListViewItem, new()
{
    public GameObject mContainer;
    public GameObject mCopy;
    public GameObject mCopyItem;
    private List<GameObject> itemPool;
    private List<T> itemList;
    private T crrItem;
    private int mCount;
    private int mPoolCount;

    public DynamicListView(DynamicListViewMono listViewMono)
    {
        Transfer(listViewMono);
    }

    void Transfer(DynamicListViewMono listViewMono)
    {
        Debuger.Log("DynamicListView awake");
        mContainer = listViewMono.mContainer;
        mCopy = listViewMono.mCopy;
        mCopyItem = listViewMono.mCopyItem;

        mCopy.SetActive(false);

        //Get Current Pool Items
        itemPool = new List<GameObject>();
        for (int i = 0; i < mContainer.transform.childCount; i++){
            itemPool.Add(mContainer.transform.GetChild(i).gameObject);
            itemPool[i].SetActive(false);
        }
        mPoolCount = itemPool.Count;
    }

    public void InitView(List<ItemListData> mDatas)
    {
        mCount = mDatas.Count;
        //Init Pool
        if (mPoolCount < mCount){
            for (int i = mPoolCount; i  < mCount; i++){
                Debuger.Log("mCopyItem is nulL: " + (mCopyItem == null) + " or mContainer == null: " + (mContainer == null));
                var newItemObj = GameObject.Instantiate(mCopyItem, mContainer.transform);
                Debuger.Log("newItemObj is null: " + (newItemObj == null));
                itemPool.Add(newItemObj);
            }
            mPoolCount = mCount;
        }

        //Init items
        itemList = new List<T>();
        T item;
        for (int i = 0; i < mCount; i++){
            item = new T();
            item.InitView(itemPool[i], mDatas[i], (item) => {
                ItemSeletectedCallback(item as T);
            });
            itemList.Add(item);
        }
    }

    public void ItemSeletectedCallback(T clickedItem)
    {
        if (crrItem != null){
            crrItem.OnUnselected();
        }
        crrItem = clickedItem;
    }
}
