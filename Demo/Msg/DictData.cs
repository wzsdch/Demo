using System;
using System.Collections.Generic;

namespace Demo
{
	/// <summary>
	/// 全局数据字典
	/// </summary>
    public static class DictDataModel
    {
        //汽车品牌
        public static Dictionary<int, string> Logo;
        //车牌颜色
        public static Dictionary<int, string> PlateColor;
        //车牌类型
        public static Dictionary<int, string> PlateType;
        //车型
        public static Dictionary<int, string> ClassIndex;
        //车身颜色
        public static Dictionary<int, string> CarColor;

        static DictDataModel()   
        {
            //初始化
            LogoInit();
            PlateColorInit();
            PlateTypeInit();
            CarColorInit();
            ClassIndexInit();
        }
        public static string Logos(int key)
        {
            try
            {
                return Logo[key];
            }
            catch (Exception)
            {
                return "其他";
            }
        }
        public static string PlateColors(int key)
        {
            try
            {
                return PlateColor[key];
            }
            catch (Exception)
            {
                return "其他";
            }
        }
        public static string PlateTypes(int key)
        {
            try
            {
                return PlateType[key];
            }
            catch (Exception)
            {
                return "其他";
            }
        }
        public static string CarColorInit(int key)
        {
            try
            {
                return CarColor[key];
            }
            catch (Exception)
            {
                return "其他";
            }
        }
        public static string ClassIndexs(int key)
        {
            try
            {
                return ClassIndex[key];
            }
            catch (Exception)
            {
                return "其他";
            }
        }


        private static void LogoInit()
        {
	        Logo = new Dictionary<int, string>
	        {
		        {0, "其他"},
		        {1, "大众"},
		        {2, "别克"},
		        {3, "宝马"},
		        {4, "本田"},
		        {5, "标致"},
		        {6, "丰田"},
		        {7, "福特"},
		        {8, "日产"},
		        {9, "奥迪"},
		        {10, "马自达"},
		        {11, "雪佛兰"},
		        {12, "雪铁龙"},
		        {13, "现代"},
		        {14, "奇瑞"},
		        {15, "起亚"},
		        {16, "荣威"},
		        {17, "三菱"},
		        {18, "斯柯达"},
		        {19, "吉利"},
		        {20, "中华"},
		        {21, "沃尔沃"},
		        {22, "雷克萨斯"},
		        {23, "菲亚特"},
		        {24, "帝豪"},
		        {25, "东亚"},
		        {26, "比亚迪"},
		        {27, "铃木"},
		        {28, "金杯"},
		        {29, "海马"},
		        {30, "五菱"},
		        {31, "江淮"},
		        {32, "斯巴鲁"},
		        {33, "英伦"},
		        {34, "长城"},
		        {35, "哈飞"},
		        {36, "五十铃"},
		        {37, "东南"},
		        {38, "长安"},
		        {39, "福田"},
		        {40, "夏利"},
		        {41, "奔驰"},
		        {42, "一汽"},
		        {43, "依维柯"},
		        {44, "力帆"},
		        {45, "一汽奔腾"},
		        {46, "皇冠"},
		        {47, "雷诺"},
		        {48, "JMC"},
		        {49, "MG名爵"},
		        {50, "凯马"},
		        {51, "众泰"},
		        {52, "昌河"},
		        {53, "厦门金龙"},
		        {54, "上海汇众"},
		        {55, "苏州金龙"},
		        {56, "海格"},
		        {57, "宇通"},
		        {58, "中国重汽"},
		        {59, "北奔重卡"},
		        {60, "华菱星马"},
		        {61, "跃进"},
		        {62, "黄海"}
	        };
        }
        private static void PlateColorInit()
        {
	        PlateColor = new Dictionary<int, string>
	        {
		        {0, "蓝色"},
		        {1, "黄色"},
		        {2, "白色"},
		        {3, "黑色"},
		        {4, "绿色"},
		        {5, "民航黑色"},
		        {0xff, "其他"}
	        };
        }
        private static void PlateTypeInit()
        {
	        PlateType = new Dictionary<int, string>
	        {
		        {0, "92式标准民用车与军车车辆"},
		        {1, "02式民用车辆"},
		        {2, "武警车车辆"},
		        {3, "警车车辆"},
		        {4, "民用车双行尾牌"},
		        {5, "使馆车牌"},
		        {6, "农用车车牌"},
		        {7, "摩托车车牌"},
		        {0xff, "其他"}
	        };
        }
        private static void CarColorInit()
        {
	        CarColor = new Dictionary<int, string>
	        {
		        {0, "其他"},
		        {1, "白色"},
		        {2, "银色"},
		        {3, "灰色"},
		        {4, "黑色"},
		        {5, "红色"},
		        {6, "深蓝"},
		        {7, "蓝色"},
		        {8, "黄色"},
		        {9, "绿色"},
		        {10, "棕色"},
		        {11, "粉色"},
		        {12, "紫色"},
		        {0xff, "其他"}
	        };
        }
        private static void ClassIndexInit()
        {
	        ClassIndex = new Dictionary<int, string>
	        {
		        {0, "未知"},
		        {1, "客车"},
		        {2, "货车"},
		        {3, "轿车"},
		        {4, "面包车"},
		        {5, "小货车"},
		        {6, "行人"},
		        {7, "二轮车"},
		        {8, "三轮车"}
	        };
        }
    }

}
