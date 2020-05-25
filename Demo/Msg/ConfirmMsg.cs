using System;
using System.IO;
using System.Text;

namespace Demo
{
	/// <summary>
	/// 数据同步回复确认消息
	/// </summary>
	public class ConfirmMsg:MsgBase
	{
		//接收到的消息类型
	    public  MsgType ReceiveMsgType { get; set; }

		//接收到的消息Id
		public string ReceiveMsgId { get; set; }

		public override MsgType Type
		{
			get { return MsgType.Confirm; }
		}

		protected override void EncodeContent(BinaryWriter w)
		{
			w.Write((short)ReceiveMsgType);
			w.Write(ReceiveMsgId.ToCharArray(),0,25);
		}
	}
}