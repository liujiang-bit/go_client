namespace Hotfix
{
    public enum IDataType
    {
        None=0,
        HeroSave,
    }

      
    public interface IData
    {
        void Init();
        void Update(int id,object data);
        void Clear();
        object GetData(int id);
        object GetDataByIndex(int index);
        int GetDataCount();
    }
}