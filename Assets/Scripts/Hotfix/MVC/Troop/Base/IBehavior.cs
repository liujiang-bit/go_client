namespace Hotfix
{
    public interface IBehavior
    {
        int State { get; set; }
        int LastState { get; set; }
        void Init(int id);
        void OnMOVE();
        void OnIDLE();
        void OnFIGHT();
        void OnFIGHTMOVE();
        void OnFIGHTANDFOLLOWUP();
    }
}