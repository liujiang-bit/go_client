using Game;
using SprotoType;

namespace Hotfix.Utils
{
    public class ErrorCodeHelper
    {
        public static void ShowErrorCodeTip(ErrorMessage error)
        {
            
            switch ((ErrorCode)@error.errorCode)
            {
                case ErrorCode.GUILD_BUILD_NUM_LIMIT:
                    
                    Tip.CreateTip(732010).Show();
                    break;
                
                case ErrorCode.GUILD_BUILD_NOT_OPEN_SPACE:
                    Tip.CreateTip(732011).Show();
                    break;
                case ErrorCode.GUILD_BUILD_CREATE_IN_DENSEFOG:
                    Tip.CreateTip(732016).Show();
                    break;
                case ErrorCode.GUILD_BUILD_CANT_OTHER_GUILD:
                                
                    Tip.CreateTip(732012).Show();
                    break;
                case ErrorCode.GUILD_TERRITORY_ALREADY_BUILD:
                    Tip.CreateTip(732013).Show();
                    break;
                case ErrorCode.GUILD_POINT_NOT_ENOUGH:
                    Tip.CreateTip(732015).Show();
                    break;
                case ErrorCode.GUILD_CREATE_BUILD_CANT_ARRIVE:
                    Tip.CreateTip(200032).Show();
                    break;
                case ErrorCode.GUILD_RESOURCE_NOT_TERRITORY:
                    Tip.CreateTip(732024).Show(); //执政官，您只能在与您的联盟领土相连接才能修建联盟旗帜 （732018）
                    break;
                case ErrorCode.GUILD_CREATE_FLAG_NOT_LINK:
                    Tip.CreateTip(732018).Show();//前往目的地的道路受到了阻碍，请先占领沿途的关卡！（ID：200032）
                    break;
                case  ErrorCode.GUILD_CURRENCY_NOT_ENOUGH:
                    Tip.CreateTip(732014).Show();//资源不足
                    break;
                case ErrorCode.GUILD_NOT_EXIST:
                    Tip.CreateTip(570101).SetStyle(Tip.TipStyle.Middle).Show();
                    break;
                case ErrorCode.GUILD_INVITE_LIMIT:
                    Tip.CreateTip(730360).Show();
                    break;
                case ErrorCode.GUILD_ALREADY_DISBAND:
                    Tip.CreateTip(730362).Show();
                    break;
                case ErrorCode.GUILD_ALREADY_IN_OTHER_GUILD:
                    Tip.CreateTip(730363).Show();
                    break;
                case ErrorCode.GUILD_MEMBER_FULL:
                    Tip.CreateTip(730072).SetStyle(Tip.TipStyle.Middle).Show();
                    break;
                case ErrorCode.GUILD_IN_THIS_GUILD:
                    Tip.CreateTip(730364).Show();
                    break;
                case ErrorCode.GUILD_BUILD_CREATE_IN_HOLYLAND:
                    Tip.CreateTip(732017).Show();
                    break;
                case ErrorCode.ITEM_NOT_JOIN_GUILD:
                    Tip.CreateTip(570091).Show();
                    break;
                case ErrorCode.ITEM_SOMMON_MONSTER_FAILED:
                    Tip.CreateTip(500103).Show();
                    break;
                case ErrorCode.CHAT_TOO_OFTEN:
                    Tip.CreateTip(750004).SetStyle(Tip.TipStyle.Middle).Show();
                    break;
                case ErrorCode.SCOUTS_SCOUTSING_BUSY:
                    Tip.CreateTip(181277).SetStyle(Tip.TipStyle.Middle).Show();
                    break;
                case ErrorCode.GUILD_NO_JURISDICTION://角色没有权限
                    Tip.CreateTip(730156).Show();
                    break;
                case ErrorCode.GUILD_MEMBER_NOT_IN_GUILD: // 该成员不在联盟中
                    Tip.CreateTip(730324).Show();
                    break;
                    //执政官，关卡和圣地周围的土地无法修建联盟建筑。（732017）

                    //当前联盟资源不足，请在联盟获取足够的资源后再次尝试。（可前往联盟仓库查看当前资源情况） （732014）

            }
        }
    }
}