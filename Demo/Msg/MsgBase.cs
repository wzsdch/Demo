using System;
using System.IO;
using System.Text;

namespace Demo
{
	/// <summary>
	/// 消息基类
	/// </summary>
    public abstract class MsgBase : IMsg
    {
        public abstract MsgType Type
        {
            get;
        }

		/// <summary>
		/// 编码消息数据
		/// </summary>
		/// <returns></returns>
        public byte[] Encode()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    EncodeHeader(w);
                    EncodeContent(w);
                    FinishedEncode(w, (int)(stream.Length - 8));
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// 编码消息头部
        /// </summary>
        /// <param name="w"></param>
        private void EncodeHeader(BinaryWriter w)
        {
            w.Write((byte)0x02);
            w.Write((byte)0x00);
            w.Write((short)Type);
            w.Write(0x00);
        }
        /// <summary>
        /// 完成编码，设置消息长度
        /// </summary>
        /// <param name="w"></param>
        /// <param name="contentLength"></param>
        private void FinishedEncode(BinaryWriter w, int contentLength)
        {
            w.Seek(4, SeekOrigin.Begin);
            w.Write(contentLength);
        }
        /// <summary>
        /// 编码消息内容
        /// </summary>
        /// <param name="w"></param>
        protected virtual void EncodeContent(BinaryWriter w)
        {

        }

		/// <summary>
		/// 解码消息数据
		/// </summary>
		/// <param name="data"></param>
        public void Decode(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    DecodeAndCheckHeader(r);
                    try
                    {
                        DecodeContent(r);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("解码发生错误:\r\n"+e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 解码并检查消息头部
        /// </summary>
        /// <param name="r"></param>
        private void DecodeAndCheckHeader(BinaryReader r)
        {
            r.ReadBytes(8);
        }
        /// <summary>
        /// 解码消息内容
        /// </summary>
        /// <param name="r"></param>
        protected virtual void DecodeContent(BinaryReader r)
        {

        }

        /// <summary>
        /// 从流中读取字符串的辅助方法
        /// </summary>
        /// <param name="r"></param>
        /// <param name="bytesCount"></param>
        /// <returns></returns>
        protected string ReadString(BinaryReader r, int bytesCount)
        {
            byte[] data = r.ReadBytes(bytesCount);
            return Encoding.UTF8.GetString(data).Trim("\0".ToCharArray());
        }
        /// <summary>
        /// 将毫秒表达的日期转换为Datetime
        /// </summary>
        /// <param name="milliSecs"></param>
        /// <returns></returns>
        protected DateTime ConvertToDateTime(ulong milliSecs)
        {
            DateTime time1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return time1970.AddMilliseconds(milliSecs);
        }
    }
}
