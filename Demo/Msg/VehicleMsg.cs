using System;
using System.Collections.Generic;
using System.Text;

namespace Demo
{
    /// <summary>
    /// 将称重、抓拍、激光组合后的车辆消息
    /// </summary>
    [Serializable]
    public class VehicleMsg : MsgBase
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public string StationId { get; set; }
        /// <summary>
        ///流水号 
        /// </summary>
        public long EvtNo { get; set; }
        /// <summary>
        /// 车道(从1开始)
        /// </summary>
        public byte LaneNo { get; set; }

        public DateTime EvtTime { get; set; }

        public DateTime MsgTime { get; set; }

        /// <summary>
        ///有效性号
        /// </summary>
        public byte ValidityCode { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string Plate { get; set; }

        public byte PlateColor { get; set; }
        /// <summary>
        /// 图片数据
        /// </summary>
        public byte[] ImgData { get; set; }

        public byte[] PlateImgData { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public byte ClassIndex { get; set; }
        /// <summary>
        /// 轴数
        /// </summary>
        public byte AxlesCount { get; set; }
        /// <summary>
        /// 总重
        /// </summary>
        public uint TotalWeight { get; set; }
        /// <summary>
        /// 轴重
        /// </summary>
        public ushort[] AxleWeights { get; set; }
        /// <summary>
        /// 轴间距
        /// </summary>
        public ushort[] axleSpaces { get; set; }
        /// <summary>
        /// 是否跨道
        /// </summary>
        public byte IsStraddle { get; set; }

        /// <summary>
        /// 总轴距 cm
        /// </summary>
        public ushort Wheelbase { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public byte Direction { get; set; }
        /// <summary>
        /// 车长
        /// </summary>
        public ushort Length { get; set; }
        /// <summary>
        /// 车宽
        /// </summary>
        public ushort Width { get; set; }
        /// <summary>
        /// 车高
        /// </summary>
        public ushort Height { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public short Speed { get; set; }

        public byte[] LaserImgData { get; set; }

        public ushort OverLength { get; set; }

        public ushort OverWidth { get; set; }

        public ushort OverHeight { get; set; }

        public uint OverWeight { get; set; }

        public ushort OverSpeed { get; set; }

        public ushort OverLengthRatio { get; set; }
        public ushort OverWidthRatio { get; set; }
        public ushort OverHeightRatio { get; set; }
        public double OverWeightRatio { get; set; }
        
        public ushort OverSpeedRatio { get; set; }

        public string WIM_Id { get; set; }

        public string DMS_Id { get; set; }

        public string LPR_Id { get; set; }

        /// <summary>
        /// 车间距，单位：50cm
        /// </summary>
        public ushort Gap { get; set; }
        /// <summary>
        /// 车时间距，单位：毫秒
        /// </summary>
        public ushort TimeGap { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ushort Headway { get; set;}

        //是否超重状态，1：正常数据，-1：异常数据,0：未超重正常数据
        public int OverWeightDataState { get; set; }
        public bool FromRTV { get; set; }

        #region shortcut


        public string PlateColorText
        {
            get
            {
                return DictDataModel.PlateColors(PlateColor);
            }
        }

        public string ClassText
        {
            get
            {
                return DictDataModel.ClassIndexs(ClassIndex);
            }
        }

        public string IsStraddleText
        {
            get {
                return IsStraddle==0 ? "否" : "是"; }
        }

        public string DirectionText
        {
            get
            {
                return Direction == 0 ? "正常" : "逆行";
            }
        }

        public string Weight_T
        {
            get
            {
                return Convert.ToSingle(TotalWeight / 1000.0f).ToString("0.000");
            }
        }
        public short Axle1_T
        {
            get
            {

                return (short)AxleWeights[0];
            }
        }
        public short Axle2_T
        {
            get
            {
                return (short)AxleWeights[1];
            }
        }
        public short Axle3_T
        {
            get
            {
                return (short)AxleWeights[2];
            }
        }
        public short Axle4_T
        {
            get
            {
                return (short)AxleWeights[3];
            }
        }
        public short Axle5_T
        {
            get
            {
                return (short)AxleWeights[4];
            }
        }
        public short Axle6_T
        {
            get
            {
                return (short)AxleWeights[5];
            }
        }
        public short Axle7_T
        {
            get
            {
                return (short)AxleWeights[6];
            }
        }
        public short Axle8_T
        {
            get
            {
                return (short)AxleWeights[7];
            }
        }
        public short Axle9_T
        {
            get
            {
                return (short)AxleWeights[8];
            }
        }
        public short Axle10_T
        {
            get
            {
                return (short)AxleWeights[9];
            }
        }
        public short Axle_space1
        {
            get
            {
                return (short)axleSpaces[0];
            }
        }
        public short Axle_space2
        {
            get
            {
                return (short)axleSpaces[1];
            }
        }
        public short Axle_space3
        {
            get
            {
                return (short)axleSpaces[2];
            }
        }
        public short Axle_space4
        {
            get
            {
                return (short)axleSpaces[3];
            }
        }
        public short Axle_space5
        {
            get
            {
                return (short)axleSpaces[4];
            }
        }
        public short Axle_space6
        {
            get
            {
                return (short)axleSpaces[5];
            }
        }
        public short Axle_space7
        {
            get
            {
                return (short)axleSpaces[6];
            }
        }
        public short Axle_space8
        {
            get
            {
                return (short)axleSpaces[7];
            }
        }
        public short Axle_space9
        {
            get
            {
                return (short)axleSpaces[8];
            }
        }
        public string Length_M
        {
            get
            {
                return Convert.ToSingle(Length / 100.0f).ToString("0.000");
            }
        }

        public string Width_M
        {
            get
            {
                return Convert.ToSingle(Width / 100.0f).ToString("0.000");
            }
        }

        public string Height_M
        {
            get
            {
                return Convert.ToSingle(Height / 100.0f).ToString("0.000");
            }
        }

        public string OverWeight_T
        {
            get
            {
                return OverWeightRatio > 0 ? Convert.ToSingle(OverWeight / 1000.0f).ToString("0.000") : "--";
            }
        }

        public string OverLength_M
        {
            get
            {
                return OverLengthRatio > 0 ? Convert.ToSingle(OverLength / 100.0f).ToString("0.000") : "--";
            }
        }

        public string OverWidth_M
        {
            get
            {
                return OverWidthRatio > 0 ? Convert.ToSingle(OverWidth / 100.0f).ToString("0.000") : "--";
            }
        }

        public string OverHeight_M
        {
            get
            {
                return OverHeightRatio > 0 ? Convert.ToSingle(OverHeight / 100.0f).ToString("0.000") : "--";
            }
        }

        public string OverWeightRatioText
        {
            get
            {
                return OverWeightRatio > 0 ? string.Format("{0}%", OverWeightRatio) : "--";
            }
        }

        public string OverLengthRatioText
        {
            get
            {
                return OverLengthRatio > 0 ? string.Format("{0}%", OverLengthRatio) : "--";
            }
        }

        public string OverWidthRatioText
        {
            get
            {
                return OverWidthRatio > 0 ? string.Format("{0}%", OverWidthRatio) : "--";
            }
        }

        public string OverHeightRatioText
        {
            get
            {
                return OverHeightRatio > 0 ? string.Format("{0}%", OverHeightRatio) : "--";
            }
        }
        public string OverSpeedRatioText
        {
            get
            {
                return OverSpeedRatio > 0 ? string.Format("{0}%", OverSpeedRatio) : "--";
            }
        }



        #endregion
        /// <summary>
        /// 是否超重
        /// </summary>
        public bool IsOverrun
        {
            get
            {
                return OverWeightRatio > 0 || OverLengthRatio > 0 || OverWidthRatio > 0 || OverHeightRatio > 0;
            }
        }
        /// <summary>
        /// 车辆状态
        /// </summary>
        public string Status
        {
            get
            {
                var status = "";
                if (IsOverrun)
                {
                    if (OverWeightRatio > 0)
                    {
                        status += string.Format("超重{0:0.000}t({1}%);", OverWeight / 1000.0f, OverWeightRatio);
                    }
                    if (OverLengthRatio > 0)
                    {
                        //var overLength_M = Convert.ToSingle(this.OverLength / 100.0f).ToString("0.000");
                        status += string.Format("超长{0:0.000}m({1}%);", OverLength / 100.0f, OverLengthRatio);
                    }
                    if (OverWidthRatio > 0)
                    {
                        status += string.Format("超宽{0:0.000}m({1}%);", OverWeight / 100.0f, OverWidthRatio);
                    }
                    if (OverHeightRatio > 0)
                    {
                        status += string.Format("超高{0:0.000}m({1}%);", OverHeight / 100.0f, OverHeightRatio);
                    }
                }
                else
                {
                    status = "正常";
                }
                return status;
            }
        }

        public string StatusMultiLine
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                var arr = Status.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < arr.Length - 1; i++)
                {
                    sb.Append(arr[i]);
                    sb.Append("\r\n");
                }
                sb.Append(arr[arr.Length - 1]);
                return sb.ToString();
            }
        }


        public static List<VehicleMsg> Vehiclemsg { get; set; }

        public override MsgType Type
        {
            get { return MsgType.Vehicle; }
        }

        protected override void EncodeContent(System.IO.BinaryWriter w)
        {

        }
        protected override void DecodeContent(System.IO.BinaryReader r)
        {
            //25
            Id = Encoding.UTF8.GetString(r.ReadBytes(25)).Trim();       //车辆信息Id
            //6
            StationId = Encoding.UTF8.GetString(r.ReadBytes(6)).Trim(); //r.ReadUInt32();  //站点Id
            //8
            EvtTime = ConvertToDateTime(r.ReadUInt64());                //事件发生时间
            //8
            MsgTime = ConvertToDateTime(r.ReadUInt64());                 //消息接收时间
            //1
            LaneNo = r.ReadByte();                                      //车道序号
            //8
            EvtNo = r.ReadInt64();                                      //设备序号
            //4
            r.ReadUInt32();                                             //备用
            //2
            r.ReadUInt16();                                 //备用
            //1
            r.ReadByte();                                   //备用
            //1
            ValidityCode = r.ReadByte();                    //有效性号
            //15
            Plate = Encoding.UTF8.GetString(r.ReadBytes(15)).Trim("\0".ToCharArray());//车牌号码
            //Plate = Encoding.GetEncoding("gb2312").GetString(r.ReadBytes(15)).Trim("\0".ToCharArray());
            //1
            PlateColor = r.ReadByte();                      //车牌颜色
            //4
            r.ReadUInt32();                                 //备用
            //1
            ClassIndex = r.ReadByte(); //DictDataModel.ClassIndexs(r.ReadByte()); //r.ReadByte();         //车型
            //1
            AxlesCount = r.ReadByte();//axles count         //轴数
            //4
            TotalWeight = r.ReadUInt32();//weight   
            //20
            AxleWeights = new ushort[10];
            for (var idx = 0; idx < 10; idx++)
            {
                AxleWeights[idx] = r.ReadUInt16();//axle_x weight
            }
            //18
            axleSpaces = new ushort[9];
            for (var idx = 0; idx < 9; idx++)
            {
                axleSpaces[idx] = r.ReadUInt16();//axle_x weight
            }
            //1
            Direction = r.ReadByte();                           // == 1 ? "逆行" : "正常行驶";//direction   //DictDataModel.dictDemo(r.ReadByte()); 
            //2
            Speed = r.ReadInt16();                              //speed
            //2
            Length = r.ReadUInt16();                            //length
            //2
            Width = r.ReadUInt16();                             //width
            //2
            Height = r.ReadUInt16();                            //height                            
            //2
            Wheelbase = r.ReadUInt16();                         //总轴距 cm
            //1
            Temperature = r.ReadByte() - 100;                   //温度
            //1
            IsStraddle = r.ReadByte();                          // ? "跨道" : "未跨道";//isStraddle

            //2
            OverLength = r.ReadUInt16();                        //over width, height, length, speed, weight
            //2
            OverWidth = r.ReadUInt16();
            //2
            OverHeight = r.ReadUInt16();
            //4
            OverWeight = r.ReadUInt32();
            //2
            OverSpeed = r.ReadUInt16();     //是否超速：0否，1是
            //2
            OverLengthRatio = r.ReadUInt16();//over ratio : width, height, length, speed, weight
            //2
            OverWidthRatio = r.ReadUInt16();
            //2
            OverHeightRatio = r.ReadUInt16();
            //2
            OverWeightRatio = r.ReadUInt16();
            //2
            OverSpeedRatio = r.ReadUInt16();    //超速比率，百分比

            //2
            //r.ReadUInt16();
            //5
            //r.ReadBytes(4);//???????????????????
            var imgSize = r.ReadInt32();//img size 
            ImgData = r.ReadBytes(imgSize);

            var plateImgSize = r.ReadInt32();//img size
            PlateImgData = r.ReadBytes(plateImgSize);

            var laserImgSize = r.ReadInt32();//laser img size   //激光3D图片大小
            LaserImgData = r.ReadBytes(laserImgSize);//laser img data

            //r.ReadBytes(6);//备用
            Gap = r.ReadUInt16();                   //车间距，单位：50cm
            TimeGap =  r.ReadUInt16();             //车时间距，单位：毫秒
            Headway = r.ReadUInt16();               //车头时距，单位：100毫秒

            WIM_Id = Encoding.UTF8.GetString(r.ReadBytes(25));//wim    //动态称重记录Id 
            LPR_Id = Encoding.UTF8.GetString(r.ReadBytes(25));//lpr    //车牌识别记录Id
            DMS_Id = Encoding.UTF8.GetString(r.ReadBytes(25));//dms    //激光扫描记录Id

            OverWeightCalculate();
            if (this.AxlesCount > 10 || this.Length > 2500 || this.Length < 0 || this.Axle1_T > 9500||this.TotalWeight>100000)
            {
                OverWeightDataState = -1;
            }
            else {
                if (OverWeight > 0 && (this.Speed > 110 || this.Speed < 20||(this.Plate.Equals("无车牌") && !(Program.port.Equals("10015") || Program.port.Equals("10016"))) ))
                //if (OverWeight > 0 && (this.Speed > 110 || this.Speed < 20 || (this.Plate.Equals("无车牌"))))
                    {
                        OverWeightDataState = -1;
                }
                else
                {
                    //G15沈海苏沪省界2车道全部超重数据放入异常数据库 
                    //2020.10.30改
                    //start
                    if (Program.port.Equals("10017") && this.LaneNo == 2)
                    {
                        OverWeight = 0;
                        this.OverWeightRatio = 0;
                        OverWeightDataState = -1;
                    }else
                    //end
                    OverWeightDataState = 1;
                }
            }

        }


        //计算超重多少千克和多少倍率
        public void OverWeightCalculate() {
            if (AxlesCount == 2)
            //SysLog.WriteLog("AxlesCount:"+AxlesCount+ ",(int)TotalWeight：" + (int)TotalWeight + "(int)TotalWeight - 18000:" + ((int)TotalWeight - 18000)+ "(OverWeight / TotalWeight * 100):" + (OverWeight / TotalWeight * 100));
            {
                if (((int)TotalWeight - (18000+18000*0.1)) > 0)
                {
                    OverWeight = TotalWeight - 18000;
                    OverWeightRatio = (double)OverWeight / 18000;
                }
                else
                {
                    OverWeight = 0;
                    OverWeightRatio = 0;
                }
            }
            if (AxlesCount == 3)
            {
                if (((int)TotalWeight - (27000+27000 * 0.1)) > 0)
                {
                    OverWeight = TotalWeight - 27000;
                    OverWeightRatio = (double)OverWeight / 27000;
                }
                else
                {
                    OverWeight = 0;
                    OverWeightRatio = 0;
                }
            }
            if (AxlesCount == 4)
            {
                if (((int)TotalWeight - (36000+36000 * 0.1)) > 0)
                {
                    OverWeight = TotalWeight - 36000;
                    OverWeightRatio = (double)OverWeight / 36000;
                }
                else
                {
                    OverWeight = 0;
                    OverWeightRatio = 0;
                }
            }
            if (AxlesCount == 5)
            {
                if (((int)TotalWeight - (43000+43000*0.1)) > 0)
                {
                    OverWeight = TotalWeight - 43000;
                    OverWeightRatio = (double)OverWeight / 43000;
                }
                else
                {
                    OverWeight = 0;
                    OverWeightRatio = 0;
                }
            }
            if (AxlesCount >= 6)
            {
                if (((int)TotalWeight - (49000+49000 * 0.1)) > 0)
                {
                    OverWeight = TotalWeight - 49000;
                    OverWeightRatio = (double)OverWeight / 49000;
                }
                else
                {
                    OverWeight = 0;
                    OverWeightRatio = 0;
                }
            }
        }
    }
}