namespace Hotfix
{
    public class TroopLinesMgr :  ITroopLinesHandler
    {
        private ITroopLine iTroopLine;
        
        public void Init()
        {
            iTroopLine=new TroopsLine();
        }

        public void Clear()
        {
            if(iTroopLine != null)
            {
                iTroopLine.RemoveTroopLines();                
            }
        }

        public ITroopLine GetITroopLine()
        {
            return iTroopLine;
        }
    }
}