namespace Demo
{
	/// <summary>
	/// 心跳消息
	/// </summary>
    public class HeartMsg : MsgBase
    {
        public override MsgType Type
        {
            get { return MsgType.Heart; }
        }
    }
}
