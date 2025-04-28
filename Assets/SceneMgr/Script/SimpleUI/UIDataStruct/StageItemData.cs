using ExcelData;

public class StageItemData : ItemListData
{    
    private Stage.Item config;
    public StageItemData(Stage.Item cfg)
    {
        this.config = cfg;
    }
    
    public string Name{
        get { 
            return this.config.name;
        }
    }

    public string Key{
        get {
            return this.config.key;
        }
    }
}