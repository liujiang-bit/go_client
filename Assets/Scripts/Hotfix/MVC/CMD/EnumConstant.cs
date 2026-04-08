
using System;
using System.Collections.Generic;


namespace Game
{
    //城市建筑类型可以由策划表导出
    public enum EnumCityBuildingType
    {
        /// <summary>
        /// 市政厅/City Hall
        /// </summary>
        TownCenter = 1,
        /// <summary>
        /// 农场/Farm
        /// </summary>
        Farm = 2,
        /// <summary>
        /// 木材厂/Lumber Mill
        /// </summary>
        Sawmill = 3,
        /// <summary>
        /// 采石场/Quarry
        /// </summary>
        Quarry = 4,
        /// <summary>
        /// 金矿/Goldmine
        /// </summary>
        SilverMine = 5,
        /// <summary>
        /// 城墙/Wall
        /// </summary>
        CityWall = 6,
        /// <summary>
        /// 工人小屋/Builder Hut
        /// </summary>
        BuilderHut = 7,
        /// <summary>
        /// 警戒塔/Watchtower
        /// </summary>
        GuardTower = 8,
        /// <summary>
        /// 兵营/Barracks
        /// </summary>
        Barracks = 9,
        /// <summary>
        /// 马厩/Stable
        /// </summary>
        Stable = 10,
        /// <summary>
        /// 靶场/Archery Range
        /// </summary>
        ArcheryRange = 11,
        /// <summary>
        /// 攻城武器厂/Siege Workshop 
        /// </summary>
        SiegeWorkshop = 12,
        /// <summary>
        /// 学院/Academy
        /// </summary>
        Academy = 13,
        /// <summary>
        /// 医院/Hospital
        /// </summary>
        Hospital = 14,
        /// <summary>
        /// 仓库/Storehouse
        /// </summary>
        Storage = 15,
        /// <summary>
        /// 联盟中心/Alliance Center
        /// </summary>
        AllianceCenter = 16,
        /// <summary>
        /// 城堡/Castel
        /// </summary>
        Castel = 17,
        /// <summary>
        /// 酒馆/Tavern
        /// </summary>
        Tavern = 18,
        /// <summary>
        /// 商栈/Trading Post
        /// </summary>
        TradingPost = 19,
        /// <summary>
        /// 商店/Shop
        /// </summary>
        Market = 20,
        /// <summary>
        /// 驿站/Courier Station
        /// </summary>
        CourierStation = 21,
        /// <summary>
        /// 斥候营地/Scout Camp
        /// </summary>
        ScoutCamp = 28,
        /// <summary>
        /// 公告板/Notice Board
        /// </summary>         
        BulletinBoard = 29,
        /// <summary>
        /// 纪念碑/Monument
        /// </summary>
        Monument = 30,
        /// <summary>
        /// 铁匠铺/Blacksmith
        /// </summary>
        Smithy = 31,
        /// <summary>
        /// 道路
        /// </summary>
        Road = 50,
        /// <summary>
        ///    装饰物
        /// </summary>
        tree = 51,
        tree2 = 52,
        tree3 = 53,
        tree4 = 54,
    }

    public enum EnumAgeType
    {
        NON = 0,
        /// <summary>
        /// 远古
        /// </summary>
        StoneAge = 1,
        /// <summary>
        /// 古典
        /// </summary>
        BronzeAge = 2,
        /// <summary>
        /// 黑暗
        /// </summary>
        IronAge = 3,
        /// <summary>
        /// 封建
        /// </summary>
        DarkAge = 4,
        /// <summary>
        /// 工业
        /// </summary>
        FeudalAge = 5,
        OTHER = 6
    }

    //战报角色类型
    public enum EnumFightRoleType
    {
        ROLE,           //角色
        ARMY,           //军队
        MONSTER,        //怪物
        CITY,           //城市
        STONE,          //石料
        FARMLAND,       //农田
        WOOD,           //木材
        GOLD,           //金矿
        DENAR,          //宝石
        SCOUTS          //斥候
    }

    //战斗类型
    public enum EnumBattleType
    {
        MONSTER = 1,             // NPC怪物战斗
        CITY_DEFENSE = 2,              // 城市防守战
        ALLY_CITY_DEFENSE = 3,              // 盟友城市防守战
        GUILD_BUILD_DEFENSE = 4,              // 联盟建筑防守战
        FIELD = 5,              // 野外战斗
        RESOURCE = 6,              // 资源点战斗
        SANCTUARY = 7,              // 圣所战斗
        ALTAR = 8,              // 圣坛战斗
        CHECKPOINT_1 = 9,              // 等级1关卡战斗
        SHRINE = 10,             // 圣祠战斗
        CHECKPOINT_2 = 11,             // 等级2关卡战斗
        LOST_TEMPLE = 12,             // 失落神庙
        CHECKPOINT_3 = 13,             // 等级3关卡战斗
        ATTACK_CITY = 14,             // 攻城战斗
        MONSTER_CITY = 15,             // 野蛮人城寨
    }

    public enum EnumBuildingGroupType
    {
        NON = 0,
        Economic = 1,//经济类
        Military = 2,//军事类
        Decorative = 3,//装饰类
    }
    public enum EnumTaskPageType
    {
        /// <summary>
        /// 章节任务
        /// </summary>
        TaskChapter = 1,
        /// <summary>
        /// 主线任务
        /// </summary>
        TaskMain = 2,
        /// <summary>
        /// 日常任务
        /// </summary>
        TaskDaily = 3,
        /// <summary>
        /// 支线任务
        /// </summary>
        TaskSide = 4,
    }

    public enum MenuButtonType
    {
        /// <summary>
        /// 通用建筑信息按钮
        /// </summary>
        openBuildingInfo = 101,

        /// <summary>
        /// 通用建造升级按钮
        /// </summary> 
        openBuildingUpdata = 102,
        /// <summary>
        /// 通用加速按钮
        /// </summary>
        openBuildingSpeedUp = 103,
        /// <summary>
        /// 打开界面
        /// </summary>
        openUI = 104,      //打开界面
        /// <summary>
        /// 训练按钮
        /// </summary>
        drill = 105,
        /// <summary>
        /// 粮食收获按钮
        /// </summary>
        harvestFood = 106,
        /// <summary>
        /// 木材收获按钮
        /// </summary>
        harvestWood = 107,
        /// <summary>
        /// 石料收获按钮
        /// </summary>
        harvestStone = 108,
        /// <summary>
        /// 金矿收获按钮
        /// </summary>
        harvestGold = 109,
        /// <summary>
        /// 研究按钮
        /// </summary>
        study = 110,
        /// <summary>
        /// 治疗按钮
        /// </summary>
        cure = 111,
        /// <summary>
        /// 主城1-2级显示升级按钮
        /// </summary>
        openBuildingUpdata_1 = 120,
        /// <summary>
        /// 主城3级显示升级按钮
        /// </summary>
        openBuildingUpdata_2 = 121,
        /// <summary>
        /// 主城4-8级显示升级按钮
        /// </summary>
        openBuildingUpdata_3 = 122,
        /// <summary>
        /// 主城9级显示升级按钮
        /// </summary>
        openBuildingUpdata_4 = 123,
        /// <summary>
        /// 主城10-14级显示升级按钮
        /// </summary>
        openBuildingUpdata_5 = 124,
        /// <summary>
        /// 主城15级显示升级按钮
        /// </summary>
        openBuildingUpdata_6 = 125,
        /// <summary>
        /// 主城16-19级显示升级按钮
        /// </summary>
        openBuildingUpdata_7 = 126,
        /// <summary>
        /// 主城20级显示升级按钮
        /// </summary>
        openBuildingUpdata_8 = 127,
        /// <summary>
        /// 主城21-24级显示升级按钮
        /// </summary>
        openBuildingUpdata_9 = 128,
    }
    /// <summary>
    /// 属性来源
    /// </summary>
    public enum EnumSourceAttr
    {
        /// <summary>
        /// 建筑
        /// </summary>
        Build = 1,
        /// <summary>
        /// 科技
        /// </summary>
        Study = 2,
        /// <summary>
        /// 联盟科技
        /// </summary>
        AliStudy = 3,
        /// <summary>
        /// 城市buff
        /// </summary>
        CityBuff = 4,

        /// <summary>
        /// VIP等级
        /// </summary>
        Vip = 5,
        /// <summary>
        /// 文明加成
        /// </summary>
        CivilizationBuff = 6,

        /// <summary>
        /// 圣地
        /// </summary>
        HolylandBuff = 7,
        /// <summary>
        /// 联盟任命
        /// </summary>
        GuildOfficerInfo = 8,

        /// <summary>
        /// 英雄技能
        /// </summary>
        HeroSkill = 9,
        /// <summary>
        /// 英雄天赋
        /// </summary>
        HeroTalent = 10,

    }

    public enum EnumTaskType
    {
        None = 0,
        /// <summary>
        /// 野蛮人克星
        /// </summary>
        RebelsNemesis = 1,
        /// <summary>
        /// 加入或创建联盟
        /// </summary>
        SwornAllies = 2,
        /// <summary>
        /// 占领一座圣地
        /// </summary>
        AbandonedFortMaster = 3,
        /// <summary>
        /// 占领一座关卡
        /// </summary>
        Stranglehold = 4,
        /// <summary>
        /// 设置过昵称
        /// </summary>
        InMyName = 5,
        /// <summary>
        /// 科技研发
        /// </summary>
        TechnologyResearch = 6,
        /// <summary>
        /// 拥有部队
        /// </summary>
        RisingStar = 7,
        /// <summary>
        /// 升级建筑
        /// </summary>
        UpgradeBuilding = 8,
        /// <summary>
        /// 收集资源
        /// </summary>
        CityCollect = 9,
        /// <summary>
        /// 招募各类部队
        /// </summary>
        Recruit = 10,
        /// <summary>
        /// 采集各类资源
        /// </summary>
        MapGather = 11,
        /// <summary>
        /// 拥有建筑
        /// </summary>
        BuildSomething = 13,
        /// <summary>
        /// 探索迷雾
        /// </summary>
        ExploreFog = 14,
        /// <summary>
        /// 战斗力达到
        /// </summary>
        PowerReached = 15,
        /// <summary>
        /// 开启类型宝箱
        /// </summary>
        StrangeEncounter = 16,
        /// <summary>
        /// 升级统帅等级
        /// </summary>
        ImposingAura = 17,
        /// <summary>
        /// 升级统帅技能
        /// </summary>
        MasterTactician = 18,
        /// <summary>
        /// 升级统帅星级
        /// </summary>
        ALegendaryPerson = 19,
        /// <summary>
        /// 为统帅点选天赋
        /// </summary>
        TheTalentedOne = 20,
        /// <summary>
        /// 摧毁野蛮人城寨
        /// </summary>
        DestroytheStronghold = 21,
        /// <summary>
        /// 进行侦查
        /// </summary>
        ScoutingMission = 22,
        /// <summary>
        /// 设置头像
        /// </summary>
        OneinaMillion = 23,
        /// <summary>
        /// 探索村庄
        /// </summary>
        AFriend = 24,
        /// <summary>
        /// 调查山洞
        /// </summary>
        CaveInvestigation = 25,
        /// <summary>
        /// 发现关卡
        /// </summary>
        AgainstAllOdds = 26,
        /// <summary>
        /// 发现圣地
        /// </summary>
        TheToweringCity = 27,
        /// <summary>
        /// 资源总产量
        /// </summary>
        Production = 28,
        /// <summary>
        /// 派遣部队
        /// </summary>
        SendRroops = 29,
        /// <summary>
        /// 医院中治疗
        /// </summary>
        Healandcollect = 30,
        /// <summary>
        /// 帮助盟友
        /// </summary>
        AllianceHelp = 31,
        /// <summary>
        /// 使用道具
        /// </summary>
        UseItem = 32,
        /// <summary>
        ///  训练部队
        /// </summary>
        Train = 33,
        /// <summary>
        /// 科技研究
        /// </summary>
        Research = 34,
        /// <summary>
        /// 商店购买
        /// </summary>
        shangdiangoumai = 37,
        /// <summary>
        /// 驿站购买
        /// </summary>
        KingdomTrade = 38,
        /// <summary>
        /// 远征通关
        /// </summary>
        LongJourney = 40,
        /// <summary>
        /// 锻造装备
        /// </summary>
        ForgeEquipment = 42,
        /// <summary>
        /// 合成图纸
        /// </summary>
        FuseEquipmentBlueprint = 43,
        /// <summary>
        /// 合成装备材料
        /// </summary>
        FuseEquipmentMaterial = 44,
        /// <summary>
        /// 生产装备材料
        /// </summary>
        ProduceEquipmentMaterials = 45,
        /// <summary>
        /// 分解装备
        /// </summary>
        DismantleEquipment = 46,
        /// <summary>
        /// 分解装备材料
        /// </summary>
        DismantleEquipmentMaterial = 47,
    }

    //资源类型
    public enum EnumResType
    {
        /// <summary>
        /// 粮食
        /// </summary>
        Food = 1,
        /// <summary>
        /// 木材
        /// </summary>
        Wood = 2,
        /// <summary>
        /// 石料
        /// </summary>
        Stone = 3,
        /// <summary>
        /// 金币
        /// </summary>
        Gold = 4,
        /// <summary>
        /// 宝石
        /// </summary>
        Diamond = 5,
    }
    public enum EnumCurrencyType
    {
        food = 100,//粮食
        wood = 101,//木材
        stone = 102,//石料
        gold = 103,//金币
        denar = 104,//宝石
        enery = 105,//行动力
        individualPoints = 106,//个人积分
        leaguePoints = 107,//联盟积分
        allianceFood = 108,//联盟食物
        allianceWood = 109,//联盟木材
        allianceStone = 110,//联盟石料
        allianceGold = 111,//联盟金币
        vipPoint = 112,//VIP点数
        allianceGiftPoint = 113,//礼物点数
        allianceKeyPoint = 114,//钥匙点数
        activePoint = 115,//活跃度
        conquerorMedal = 116,//征服者勋章
    }

    //士兵类型
    public enum EnumSoldierType
    {
        /// <summary>
        /// 步兵
        /// </summary>
        Infantry = 1,
        /// <summary>
        /// 骑兵
        /// </summary>
        Cavalry = 2,
        /// <summary>
        /// 弓兵
        /// </summary>
        Bowmen = 3,
        /// <summary>
        /// 车兵
        /// </summary>
        SiegeEngines = 4,
    }

    /// <summary>
    /// 科技子类型
    /// </summary>
    public enum EnumStudyType
    {
        /// <summary>
        /// 采石
        /// </summary>
        Quarrying = 101,
        /// <summary>
        /// 灌溉
        /// </summary>
        Irrigation = 102,
        Handsaw = 103,
        /// <summary>
        /// 手锯
        /// </summary>
        Sickle = 104,
        /// <summary>
        /// 镰刀
        /// </summary>
        Masonry = 105,
        /// <summary>
        /// 手斧
        /// </summary>
        Handaxe = 106,
        /// <summary>
        /// 冶金术
        /// </summary>
        Metallurgy = 107,
        /// <summary>
        /// 凿子
        /// </summary>
        Chisel = 108,
        /// <summary>
        /// 文字
        /// </summary>
        Writing = 109,
        /// <summary>
        /// 金属加工
        /// </summary>
        Metalworking = 110,
        /// <summary>
        /// 多层建筑结构
        /// </summary>
        MultilayerStructure = 111,
        /// <summary>
        /// 手推车
        /// </summary>
        Handcart = 112,
        /// <summary>
        /// 砂矿开采法
        /// </summary>
        PlacerMining = 113,
        /// <summary>
        /// 车轮
        /// </summary>
        Wheel = 114,
        /// <summary>
        /// 珠宝
        /// </summary>
        Jewelry = 115,
        /// <summary>
        /// 耕犁
        /// </summary>
        Plow = 116,
        /// <summary>
        /// 锯木厂
        /// </summary>
        Sawmill = 117,
        /// <summary>
        /// 长柄大镰刀
        /// </summary>
        Scythe = 118,
        /// <summary>
        /// 双人粗木锯
        /// </summary>
        Whipsaw = 119,
        /// <summary>
        /// 工程学
        /// </summary>
        Engineering = 120,
        /// <summary>
        /// 数学
        /// </summary>
        Mathematics = 121,
        /// <summary>
        /// 露天采石场
        /// </summary>
        OpenpitQuarry = 122,
        /// <summary>
        /// 铸币
        /// </summary>
        Coinage = 123,
        /// <summary>
        /// 石锯
        /// </summary>
        StoneSaw = 124,
        /// <summary>
        /// 竖井开采法
        /// </summary>
        ShaftMining = 125,
        /// <summary>
        /// 机械
        /// </summary>
        Machinery = 126,
        /// <summary>
        /// 辎重马车
        /// </summary>
        Carriage = 127,
        /// <summary>
        /// 切割抛光工艺
        /// </summary>
        CuttingPolishing = 128,

        /// <summary>
        /// 军事纪律
        /// </summary>
        MilitaryDiscipline = 201,
        /// <summary>
        /// 炼铁术
        /// </summary>
        IronWorking = 202,
        /// <summary>
        /// 箭羽改良
        /// </summary>
        ImprovedFletching = 203,
        /// <summary>
        /// 骑术
        /// </summary>
        Horsemanship = 204,
        /// <summary>
        /// 燃烧弹
        /// </summary>
        FlamingProjectile = 205,
        /// <summary>
        /// 剑士
        /// </summary>
        Swordsman = 206,

        /// <summary>
        /// 弓箭手
        /// </summary>
        Bowman = 207,
        /// <summary>
        /// 轻骑兵
        /// </summary>
        LightCavalry = 208,
        /// <summary>
        /// 床弩
        /// </summary>
        Arcuballista = 209,
        /// <summary>
        /// 追踪术
        /// </summary>
        Tracking = 210,
        /// <summary>
        /// 寻路术
        /// </summary>
        Pathfinding = 211,
        /// <summary>
        /// 小圆盾
        /// </summary>
        Buckler = 212,
        /// <summary>
        /// 皮甲
        /// </summary>
        LeatherArmor = 213,
        /// <summary>
        /// 鳞甲
        /// </summary>
        ScaleArmor = 214,
        /// <summary>
        /// 轮轴强化
        /// </summary>
        EnhancedAxle = 215,
        /// <summary>
        /// 枪兵
        /// </summary>
        Spearman = 216,
        /// <summary>
        /// 复合弓手
        /// </summary>
        CompositeBowman = 217,
        /// <summary>
        /// 重骑兵
        /// </summary>
        HeavyCavalry = 218,
        /// <summary>
        /// 投石车
        /// </summary>
        Mangonel = 219,
        /// <summary>
        /// 伪装术
        /// </summary>
        Camouflage = 220,
        /// <summary>
        /// 战斗策略
        /// </summary>
        CombatTactics = 221,
        /// <summary>
        /// 防御阵型
        /// </summary>
        DefensiveFormation = 222,
        /// <summary>
        /// 草药
        /// </summary>
        HerbalMedicine = 223,

        /// <summary>
        /// 制图学
        /// </summary>
        Cartography = 224,
        /// <summary>
        /// 长剑士
        /// </summary>
        LongSwordsman = 225,
        /// <summary>
        /// 弩手
        /// </summary>
        Crossbowman = 226,

        /// <summary>
        /// 骑士
        /// </summary>
        Knight = 227,

        /// <summary>
        /// 弩炮
        /// </summary>
        Ballista = 228,

        /// <summary>
        /// 乌兹钢
        /// </summary>
        WootzSteel = 229,


        /// <summary>
        /// 锥形箭
        /// </summary>
        BodkinArrows = 230,

        /// <summary>
        /// 马镫
        /// </summary>
        Stirrups = 231,

        /// <summary>
        /// 弹道学
        /// </summary>
        Ballistics = 232,

        /// <summary>
        /// 长鳞盾
        /// </summary>
        Scutum = 233,
        /// <summary>
        /// 巨盾
        /// </summary>
        GiantShield = 234,

        /// <summary>
        /// 板甲
        /// </summary>
        PlateArmor = 235,

        /// <summary>
        /// 重型车架
        /// </summary>
        HeavyFrame = 236,

        /// <summary>
        /// 医疗部队
        /// </summary>
        MedicalCorps = 237,

        /// <summary>
        /// 联合作战
        /// </summary>
        CombinedArms = 238,

        /// <summary>
        /// 扎营防守
        /// </summary>
        Encampment = 239,


        /// <summary>
        /// 禁卫军
        /// </summary>
        RoyalGuard = 240,

        /// <summary>
        /// 皇家弩手
        /// </summary>
        RoyalCrossbowman = 241,

        /// <summary>
        /// 皇家骑士
        /// </summary>
        RoyalKnight = 242,

        /// <summary>
        /// 抛石机
        /// </summary>
        Trebuchet = 243

    }

    //队列类型
    public enum EnumQueueType
    {
        /// <summary>
        /// 训练中
        /// </summary>
        Training = 1,
        /// <summary>
        /// 研究中
        /// </summary>
        Studying = 2,
        /// <summary>
        /// 治疗中 
        /// </summary>
        Treatmenting = 3,
        /// <summary>
        /// 建筑升级中
        /// </summary>
        Upgradeing = 4,
    }

    //功能开启类型
    public enum EnumOpenType
    {
        /// <summary>
        /// 默认开启
        /// </summary>
        Default = 0,
        /// <summary>
        /// 建筑
        /// </summary>
        BuildType = 1,
        /// <summary>
        /// 系统
        /// </summary>
        SystemOpen = 2,
        /// <summary>
        /// 活动
        /// </summary>
        Activity = 3,
        /// <summary>
        /// 礼包
        /// </summary>
        Package = 4,
        /// <summary>
        /// 充值
        /// </summary>
        Recharge = 5,
        /// <summary>
        /// 隐藏前往按钮
        /// </summary>
        Hide = 6,
    }

    //品质类型
    public enum EnumRareType
    {
        /// <summary>
        /// 普通
        /// </summary>
        White = 1,
        /// <summary>
        /// 优秀
        /// </summary>
        Green = 2,
        /// <summary>
        /// 精英
        /// </summary>
        Blue = 3,
        /// <summary>
        /// 史诗
        /// </summary>
        Purple = 4,
        /// <summary>
        /// 传说
        /// </summary>
        Orange = 5,
    }

    //充值分页类型
    public enum EnumRechargeListPageType
    {
        ChargeFirst = 101,
        ChargeRiseRoad = 102,
        ChargeSuperGift = 103,
        ChargeDayCheap = 104,
        ChargeCitySupply = 105,
        ChargeGrowing = 106,
        ChargeGemShop = 107,
    }
    //个性化设置
    public enum EnumSetttingPersonType
    {
        DiamondUsageConfirmationa = 2, //钻石使用确认
        RecruitPurchaseConfirmation = 3,//招募购买确认
        AllianceShopPurchaseConfirmation = 4,//联盟商店购买确认
            AllianceShopReplenishConfirmation = 5,//联盟商店补货确认
    }

    //充值分页类型 View 对应 s_openui 中的ID
    public enum EnumRechargeListPageViewType
    {
        ChargeFirst = 7000,
        ChargeRiseRoad = 7001,
        ChargeSuperGift = 7002,
        ChargeDayCheap = 7003,
        ChargeCitySupply = 7005,
        ChargeGrowing = 7006,
        ChargeGemShop = 7004,
    }

    public enum MaintainType
    {
        Normal, // 维护公告
        NormalSingle, // 单服维护公告
        ForceUpdate, //强制整包更新
        OptionalUpdate, // 提示整包更新
        HotfixUpdate, // 提示热更新
    }

    public enum EnumAllianceStorePageType
    {
        /// <summary>
        /// 购物
        /// </summary>
        Shoping,
        /// <summary>
        /// 库存
        /// </summary>
        Stock
    }
}
