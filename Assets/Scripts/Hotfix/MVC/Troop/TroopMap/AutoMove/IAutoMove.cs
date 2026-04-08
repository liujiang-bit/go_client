namespace Hotfix
{
    public interface IAutoMove
    {
        void Init(object pamr);
        void Update(int id);
        void Remove(int id);
        object GetData(int id);
    }
}