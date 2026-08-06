public class StageItemData : ItemListData
{    
    public int id;
    public StageItemData(int id)
    {
        this.id =id;
    }
    
    public string Name
    {
        get { return $"Stage {id}";}
    }

    public string Key
    {
        get { return $"{id}"; }
    }
}