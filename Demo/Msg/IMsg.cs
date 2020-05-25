namespace Demo
{
	/// <summary>
	/// 消息接口
	/// </summary>
    public interface IMsg
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        MsgType Type { get; }

        byte[] Encode();

        void Decode(byte[] data);
    }
}
