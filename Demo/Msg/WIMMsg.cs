using System;
using System.Text;

namespace Demo
{
	/// <summary>
	/// 动态称重消息
	/// </summary>
    public class WIMMsg : MsgBase
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
        public Int64 EvtNo { get; set; }
        /// <summary>
        /// 车道(从1开始)
        /// </summary>
        public byte LaneNo { get; set; }
        public DateTime EvtTime { get; set; }
        public DateTime MsgTime { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        public UInt16 ClassIndex { get; set; }
		
		/// <summary>
		/// 类型名称
		/// </summary>
        private string _className;
        public string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _className = "";
                }
                else
                {
                    _className = value.Trim();
                }
            }
        }
        public byte DevType { get; set; }

		/// <summary>
		/// 轴数
		/// </summary>
        public byte AxlesCount { get; set; }
		/// <summary>
		/// 总重量
		/// </summary>
        public UInt32 Weight { get; set; }
		/// <summary>
		/// 轴重
		/// </summary>
        public UInt16[] AxleWeights { get; set; }
		/// <summary>
		/// 轴间距
		/// </summary>
        public UInt16[] AxleSpaces { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public byte Direction { get; set; }
		/// <summary>
		/// 速度
		/// </summary>
        public UInt16 Speed { get; set; }
		/// <summary>
		/// 车长
		/// </summary>
        public UInt16 Length { get; set; }
        public bool Straddle { get; set; }
        public byte AxlesOnTractor { get; set; }
        public byte DrawBarTrailer { get; set; }
        public byte Temperature { get; set; }

        private string _chassisCode;
        //20bytes
        public string ChassisCode
        {
            get
            {
                return _chassisCode;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _chassisCode = "";
                }
                else
                {
                    _chassisCode = value.Trim();
                }
            }
        }
        public UInt16 Acceleration { get; set; }
        public UInt16 Gap { get; set; }
        public UInt16 TimeGap { get; set; }
        public UInt16 Headway { get; set; }
        public UInt16 TimeOffset { get; set; }
        public UInt16 LoopOnTime { get; set; }
        public UInt16 LegalStatus { get; set; }
        public byte SensorFailure { get; set; }
        public UInt16 ValidityCode { get; set; }
        public UInt32 OverWeight { get; set; }
        public UInt16 OverWeightRatio { get; set; }
        public UInt16 OverLength { get; set; }
        public UInt16 OverLengthRatio { get; set; }
        public UInt16 OverSpeed { get; set; }        
        public UInt16 OverSpeedRatio { get; set; }
        /// <summary>
        /// 是否为实时数据
        /// </summary>
        public bool FromRTV { get; set; }
        #region shortcut

        public string IsStraddleText
        {
            get { return Straddle ? "是" : "否"; }
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
                return Convert.ToSingle(Weight / 1000.0f).ToString("0.00");
            }
        }
        public int TempC {
            get {
                return Temperature > 0 ? Temperature - 100 : 0;
            }
        }
        public string Axle1_T
        {
            get
            {
                return AxlesCount > 0 ? Convert.ToSingle(AxleWeights[0] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle2_T
        {
            get
            {
                return AxlesCount > 1 ? Convert.ToSingle(AxleWeights[1] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle3_T
        {
            get
            {
                return AxlesCount > 2 ? Convert.ToSingle(AxleWeights[2] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle4_T
        {
            get
            {
                return AxlesCount > 3 ? Convert.ToSingle(AxleWeights[3] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle5_T
        {
            get
            {
                return AxlesCount > 4 ? Convert.ToSingle(AxleWeights[4] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle6_T
        {
            get
            {
                return AxlesCount > 5 ? Convert.ToSingle(AxleWeights[5] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle7_T
        {
            get
            {
                return AxlesCount > 6 ? Convert.ToSingle(AxleWeights[6] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle8_T
        {
            get
            {
                return AxlesCount > 7 ? Convert.ToSingle(AxleWeights[7] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle9_T
        {
            get
            {
                return AxlesCount > 8 ? Convert.ToSingle(AxleWeights[8] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string Axle10_T
        {
            get
            {
                return AxlesCount > 9 ? Convert.ToSingle(AxleWeights[9] / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace1_M
        {
            get
            {
                return AxlesCount > 1 ? Convert.ToSingle(AxleSpaces[0] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace2_M
        {
            get
            {
                return AxlesCount > 2 ? Convert.ToSingle(AxleSpaces[1] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace3_M
        {
            get
            {
                return AxlesCount > 3 ? Convert.ToSingle(AxleSpaces[2] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace4_M
        {
            get
            {
                return AxlesCount > 4 ? Convert.ToSingle(AxleSpaces[3] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace5_M
        {
            get
            {
                return AxlesCount > 5 ? Convert.ToSingle(AxleSpaces[4] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace6_M
        {
            get
            {
                return AxlesCount > 6 ? Convert.ToSingle(AxleSpaces[5] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace7_M
        {
            get
            {
                return AxlesCount > 7 ? Convert.ToSingle(AxleSpaces[6] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace8_M
        {
            get
            {
                return AxlesCount > 8 ? Convert.ToSingle(AxleSpaces[7] / 100.0f).ToString("0.00") : "--";
            }
        }
        public string AxleSpace9_M
        {
            get
            {
                return AxlesCount > 9 ? Convert.ToSingle(AxleSpaces[8] / 100.0f).ToString("0.00") : "--";
            }
        }

        public string Length_M
        {
            get
            {
                return Convert.ToSingle(Length / 100.0f).ToString("0.00");
            }
        }

        public string OverWeight_T
        {
            get
            {
                return OverWeightRatio > 0 ? Convert.ToSingle(OverWeight / 1000.0f).ToString("0.00") : "--";
            }
        }
        public string OverLength_M
        {
            get
            {
                return OverLengthRatio > 0 ? Convert.ToSingle(OverLength / 100.0f).ToString("0.00") : "--";
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
        public string OverSpeedRatioText
        {
            get
            {
                return OverSpeedRatio > 0 ? string.Format("{0}%", OverSpeedRatio) : "--";
            }
        }
       
        public string TimeGap_Second
        {
            get
            {
                return Convert.ToSingle(TimeGap / 10.0f).ToString("0.00");
            }
        }

        public string HeadWay_Second
        {
            get
            {
                return Convert.ToSingle(Headway / 10.0f).ToString("0.00");
            }
        }
        public string Gap_M
        {
            get
            {
                return Convert.ToSingle(Gap / 100.0f).ToString("0.00");
            }
        }
        #endregion

        public bool IsOverrun
        {
            get
            {
                return OverWeightRatio > 0 || OverLengthRatio > 0 || OverSpeedRatio > 0;
            }
        }
        public string Status
        {
            get
            {
                var status = "";
                if (IsOverrun)
                {
                    if (OverWeightRatio > 0)
                    {
                        //var overWeight_T = Convert.ToSingle(this.OverWeight / 1000.0f).ToString("0.000");
                        status += string.Format("超重{0:0.00}t({1}%);", OverWeight / 1000.0f, OverWeightRatio);
                    }
                    if (OverLengthRatio > 0)
                    {
                        //var overLength_M = Convert.ToSingle(this.OverLength / 100.0f).ToString("0.000");
                        status += string.Format("超长{0:0.00}m({1}%);", OverLength / 100.0f, OverLengthRatio);
                    }
                    if (OverSpeedRatio > 0)
                    {
                        status += string.Format("超速{0}km/h({1}%);", OverSpeed, OverSpeedRatio);
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
        public override MsgType Type
        {
            get { return MsgType.Wim; }
        }
        protected override void DecodeContent(System.IO.BinaryReader r)
        {
            Id = ReadString(r, 25); //r.ReadInt64();  //车辆信息Id

            StationId = ReadString(r, 6); //r.ReadInt32();  //站点Id
            EvtTime = ConvertToDateTime(r.ReadUInt64());    //事件发生时间
            MsgTime = ConvertToDateTime(r.ReadUInt64());      //消息接收时间

            LaneNo = r.ReadByte();   //车道序号
            EvtNo = r.ReadInt64();  //设备序号

            DevType = r.ReadByte();//devType
            ClassIndex = r.ReadUInt16();
            ClassName = ReadString(r, 20);
            AxlesCount = r.ReadByte();
            Weight = r.ReadUInt32();

            AxleWeights = new ushort[10];
            for (var idx = 0; idx < 10; idx++)
            {
                AxleWeights[idx] = r.ReadUInt16();//axle_x weight
            }

            AxleSpaces = new ushort[9];
            for (var idx = 0; idx < 9; idx++)
            {
                AxleSpaces[idx] = r.ReadUInt16();//axle_x inner space 
            }

            Direction = r.ReadByte();// == 1 ? "逆行" : "正常行驶";//direction   //DictDataModel.dictDemo(r.ReadByte()); 
            Speed = r.ReadUInt16();//speed
            Length = r.ReadUInt16();//length
            Straddle = r.ReadByte() == 1;// ? "跨道" : "未跨道";//isStraddle

            AxlesOnTractor = r.ReadByte();
            DrawBarTrailer = r.ReadByte();

            Temperature = r.ReadByte();
            ChassisCode = ReadString(r, 20);
            Acceleration = r.ReadUInt16();

            Gap = r.ReadUInt16();//length
            TimeGap = r.ReadUInt16();//length
            Headway = r.ReadUInt16();
            TimeOffset = r.ReadUInt16();
            LoopOnTime = r.ReadUInt16();

            LegalStatus = r.ReadUInt16();//length
            SensorFailure = r.ReadByte();
            ValidityCode = r.ReadUInt16();

            OverWeight = r.ReadUInt16();
            OverWeightRatio = r.ReadUInt16();

            OverSpeed = r.ReadUInt16();
            OverSpeedRatio = r.ReadUInt16();

            OverLength = r.ReadUInt16();
            OverLengthRatio = r.ReadUInt16();

        }
    }
}
