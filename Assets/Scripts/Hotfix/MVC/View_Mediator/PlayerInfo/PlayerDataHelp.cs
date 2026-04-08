using Data;
using Skyunion;

namespace Game
{
    public static class PlayerDataHelp
    {
        public static void ShowActionUI(int costAp=0)
        {
            var m_bagProxy= AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            int count = 0;
            var itemCfgs = CoreUtils.dataService.QueryRecords<ItemDefine>();
            foreach (var itemCfg in itemCfgs)
            {
                if (itemCfg.subType == 50208 &&
                    m_bagProxy?.GetItemNum(itemCfg.ID) > 0)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                CoreUtils.uiManager.ShowUI(UI.s_exchageActionPoint,null);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_useItem, null, new UseItemViewData()
                {
                    ItemType = UseItemType.ActionPoint,
                    costAp= costAp
                });
            }
        }
    }
}