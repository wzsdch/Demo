using System;

namespace Demo
{
    /// <summary>
    /// 解码二进制消息
    /// </summary>
	internal static class MsgParser
	{
		/// <summary>
		/// 解码消息数据
		/// </summary>
		/// <param name="data">消息数据</param>
		/// <returns>解码后的消息实例</returns>
		public static IMsg Parse(byte[] data)
		{
			if (data == null || data.Length < 8)
			{
                //throw new Exception("数据为null或长度小于8");
                SysLog.WriteError("数据为null或长度小于8");
                Console.WriteLine("数据为null或长度小于8");
                return null;
			}

			MsgType msgType = (MsgType) BitConverter.ToUInt16(data, 2);
			uint msgLength = BitConverter.ToUInt32(data, 4);

			if (msgLength != (data.Length - 8))
			{
				//throw new Exception("消息实际长度与声明的长度不一致。");
                SysLog.WriteError("消息实际长度与声明的长度不一致");
                Console.WriteLine("消息实际长度与声明的长度不一致");
                return null;
            }

			IMsg msg = Create(msgType);
			if (msg == null)
			{
				//throw new Exception("无法处理的消息类型");
                SysLog.WriteError("无法处理的消息类型");
                Console.WriteLine("无法处理的消息类型");
                return null; 
            }
			else
			{
				try
				{
					msg.Decode(data);
				}
				catch (Exception ex)
				{
                    SysLog.WriteError(ex.Message);
                    SysLog.WriteError(ex.InnerException.Message);
					//throw new Exception("解码消息数据失败");
                    SysLog.WriteError("解码消息数据失败");
                    Console.WriteLine("解码消息数据失败");
                    return null;
                }
			}
			return msg;
		}

		/// <summary>
		/// 创建消息，目前只支持涉及三种：Heart、Vehicle和Wim
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <returns>新创建的消息实例</returns>
		private static IMsg Create(MsgType type)
		{
			switch (type)
			{
				case MsgType.Heart:
					return new HeartMsg();
				case MsgType.Vehicle:
					VehicleMsg vMsg = new VehicleMsg {FromRTV = true};
					return vMsg;
				case MsgType.Wim:
					WIMMsg wMsg = new WIMMsg {FromRTV = true};
					return wMsg;
			}
			return null;
		}
	}
}