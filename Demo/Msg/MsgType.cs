namespace Demo
{
	/// <summary>
	/// 消息类型
	/// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 动态称重消息
        /// </summary>
        Wim = 0x0001,
        /// <summary>
        /// 将称重、抓拍、激光组合后测车辆信息
        /// </summary>
        Vehicle = 0x0007,
        /// <summary>
        /// 数据同步确认消息
        /// </summary>
		Confirm=0xfffd,
        /// <summary>
        /// 心跳信息
        /// </summary>
        Heart = 0xfffe,
        /// <summary>
        /// 错误信息
        /// </summary>
        Error = 0xffff
    }
}
