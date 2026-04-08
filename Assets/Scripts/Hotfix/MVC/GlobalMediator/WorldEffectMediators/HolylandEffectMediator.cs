using Data;
using Skyunion;

namespace Game
{
    public class HolylandEffectMediator : BaseWorldEffectMediator
    {
        private StrongHoldDataDefine m_strongHoldDataDefine;
        private StrongHoldTypeDefine m_strongHoldTypeDefine;
        
        
        public override void UpdateEffects(MapObjectInfoEntity objectInfoEntity)
        {
            CheckCreateProtectEffect(objectInfoEntity);
        }

        private void CheckCreateProtectEffect(MapObjectInfoEntity objectInfoEntity)
        {
            StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)objectInfoEntity.strongHoldId);
            StrongHoldTypeDefine strongHoldTypeDefine =
                CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);

            UI_Item_IconAndTime_SubView.BuildingState state =
                (UI_Item_IconAndTime_SubView.BuildingState) objectInfoEntity.holyLandStatus;
            
            bool isShowEffect = false;
            if (state == UI_Item_IconAndTime_SubView.BuildingState.InitProtecting || state == UI_Item_IconAndTime_SubView.BuildingState.Protecting || state == UI_Item_IconAndTime_SubView.BuildingState.NotUnlock)
            {
                isShowEffect = true;
            }
            SetEffectActive(objectInfoEntity, strongHoldTypeDefine.protectEffect, isShowEffect);

            if (objectInfoEntity.rssType == RssType.HolyLand)
            {
                SetEffectActive(objectInfoEntity, RS.HolyLandCircle, true);
            }
        }

    }
}