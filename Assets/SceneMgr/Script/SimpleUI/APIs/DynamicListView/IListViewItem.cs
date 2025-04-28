using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.UI;

public interface IItemListData
{
}

public interface IListViewItem
{
}

public class ItemListData : IItemListData
{
}

public class ListViewItem : IListViewItem
{
    protected GameObject mObjView;
    private Button mButton;
    protected ItemListData mData;
    protected Action<ListViewItem> mItemSeletectedCallback;
    private bool mIsInitialzied;

    public ListViewItem()
    {
        mIsInitialzied = false;
    }

    public void InitView(GameObject objView, ItemListData data, Action<ListViewItem> itemSeletectedCallback)
    {        
        if (mIsInitialzied){ return; }
        mIsInitialzied = true;
        this.mObjView = objView;
        this.mData = data;
        this.mButton = mObjView.transform.GetComponent<Button>();
        this.mItemSeletectedCallback = itemSeletectedCallback;
        mObjView.SetActive(true);

        mButton.onClick.AddListener(
            () => {
                ItemClicked();
            }
        );

        OnInitView();
        
    }

    // protected virtual void RefreshView()
    // {

    // }

    protected virtual void OnInitView()
    {
        
    }

    private void ItemClicked()
    {
        if (mItemSeletectedCallback != null){
            mItemSeletectedCallback?.Invoke(this);
        }
        OnItemClicked();
    }

    protected virtual void OnItemClicked()
    {
    }

    public virtual void OnUnselected()
    {
        
    }
}
