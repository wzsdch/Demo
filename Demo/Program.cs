using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Windows.Forms;
using NetMQ;
using System.Threading.Tasks;
using FluentFTP;
using System.Threading;
using System.Web;
using System.Text;
using System.Collections;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace Demo
{

    internal class Program
    {


        private static int receivedCount = 0;
        private static System.Timers.Timer timer = new System.Timers.Timer();
        public static string port = "";

        private static void Main(string[] args)
        {
            Console.Title = "Demo" + args[0];
            port = args[0];
            timer.Elapsed += Timer_Tick;
            timer.Interval = 60 * 60 * 1000;
            timer.Enabled = true;
            //new Thread(isReceivedLink).Start();
            SysLog.LogName = $"port：{args[0]}.log";
            if (args.Length != 1)
            {
                SysLog.WriteError("参数数目不正确");
                Console.WriteLine("参数数目不正确");
            }
            //进程通信，给服务端通知这是开启的哪个端口
            //Info.ProInfo.GetProcessInfo(args[0]);
            //创建消息上下文对象
            using (NetMQContext context = NetMQContext.Create())
            {
                //创建Socket对象
                try
                {
                    NetMQSocket serverSocket = context.CreateResponseSocket();

                    //绑定本地端口，监听客户端的请求，端口号与配置一致
                    serverSocket.Bind($"tcp://*:{args[0]}");
                    Console.WriteLine($"开始监听{args[0]}");
                    SysLog.WriteLog($"开始监听{args[0]}");
                    string IP = "";
                    while (true)
                    {
                        //接收消息
                        byte[] data = serverSocket.Receive();
                        SysLog.WriteLog($"收到数据，来自：{serverSocket.Options.GetLastEndpoint}");
                        Console.WriteLine($"收到数据，来自：{serverSocket.Options.GetLastEndpoint}数据类型：{data[2]}");
                        receivedCount++;
                        //saveInFile(data);
                        IP = serverSocket.Options.GetLastEndpoint;
                        if (IP.Contains(":"))
                        {
                            IP = IP.Split(':')[0];
                        }
                        try
                        {
                            //解析消息
                            IMsg msg = MsgParser.Parse(data);

                            //创建回复消息
                            ConfirmMsg responseMsg = new ConfirmMsg
                            {
                                ReceiveMsgType = msg.Type,
                                ReceiveMsgId = "0"
                            };

                            //打印消息数据
                            switch (msg.Type)
                            {
                                //车辆消息--可以根据实际情况获取消息内容
                                case MsgType.Vehicle:
                                    {
                                        VehicleMsg vMsg = msg as VehicleMsg;
                                        responseMsg.ReceiveMsgId = vMsg.Id;
                                        SysLog.WriteLog(string.Format("车辆信息：({0} {1} {2}t {3})", vMsg.Id, vMsg.EvtTime, vMsg.Weight_T, vMsg.Status));
                                        Console.WriteLine(string.Format("车辆信息：({0} {1} {2} {3}t {4} {5})", vMsg.Id, vMsg.Plate, vMsg.EvtTime, vMsg.Weight_T, vMsg.Speed, vMsg.Status));
                                        Intodb(vMsg, IP, args[0]);
                                        vMsg = null;
                                        break;
                                    }
                                //称重消息--可以根据实际情况获取消息内容
                                case MsgType.Wim:
                                    {
                                        WIMMsg wMsg = msg as WIMMsg;
                                        SysLog.WriteLog(string.Format("称重信息(时间：{0}，车道：{1}, 轴数：{2}，总重：{3}t, 车速：{4}k/h，车长: {5}m)",
                                        wMsg.EvtTime.ToString("yyyy-MM-dd HH:mm:ss"), wMsg.LaneNo, wMsg.AxlesCount, wMsg.Weight_T, wMsg.Speed, wMsg.Length_M));
                                        responseMsg.ReceiveMsgId = wMsg.Id;
                                        wMsg = null;
                                        break;
                                    }
                                //心跳消息
                                case MsgType.Heart:
                                    {
                                        SysLog.WriteLog("Heart");
                                        Console.WriteLine("Heart");
                                        break;
                                    }
                            }

                            //发送回复消息
                            serverSocket.Send(responseMsg.Encode());
                          }
                          catch (Exception ex)
                          {
                              SysLog.WriteError(ex.Message);
                              Console.WriteLine(ex.Message);
                              Console.WriteLine(ex.InnerException.Message);
                              Console.WriteLine("接收数据出现错误");
                              return;
                          }
                          
                        Console.WriteLine();
                    }
                }
                /*   catch (SocketException es)
                     {
                         SysLog.WriteError(es.Message);
                         Console.WriteLine(es.Message);
                     }*/
                catch (NetMQException e2)
                {
                    SysLog.WriteError("error" + e2.InnerException.Message);
                    Console.WriteLine("error" + e2.InnerException.Message);

                }
            }
        }

        public static void Intodb(VehicleMsg vMsg, string IP, string port)
        {
            Hashtable ht = new Hashtable();
            ht.Add("vMsg", vMsg);
            ht.Add("IP", IP);
            ht.Add("port", port);
            ThreadPool.QueueUserWorkItem(new WaitCallback(saveintoDatabase), ht);
        }

        private static void saveintoDatabase(object oht)
        {
            Hashtable ht = (Hashtable)oht;
            VehicleMsg vMsg = (VehicleMsg)ht["vMsg"];
            string IP = (string)ht["IP"];
            string port = (string)ht["port"];
            //tv是全部车辆数据
            t_vehicle_msg tv = new t_vehicle_msg();
            //tvt是当日车辆数据，数据库每天零点会清空
            t_vehicle_msg_today tvt = null;
            //tvot是超重超载车辆数据，当车重符合条件并超过规定数额时，增加一条
            t_vehicle_overweight_temp tvot = null;

            t_vehicle_overweight tvo = null;

            t_vehicle_msg_abnormal tva = null;

            //tv.id_local = System.Guid.NewGuid().ToString("N");//用UUID作为当前一条数据的ID。
            tv.id_local = vMsg.Id;
            tv.Id = vMsg.Id;
            tv.Station_Id = vMsg.StationId;
            tv.Evt_Time = vMsg.EvtTime;
            tv.Msg_Time = vMsg.MsgTime;
            tv.Lane_No = (sbyte)vMsg.LaneNo;
            tv.Plate = vMsg.Plate;
            tv.Plate_Color = (sbyte)vMsg.PlateColor;
            tv.Class_Index = (sbyte)vMsg.ClassIndex;
            tv.Length = (short)vMsg.Length;
            tv.Speed = vMsg.Speed;
            tv.Direction = (sbyte)vMsg.Direction;
            tv.Axles_Count = (sbyte)vMsg.AxlesCount;
            tv.Total_Weight = (int)vMsg.TotalWeight;
            tv.Is_Straddle = (sbyte)vMsg.IsStraddle;
            tv.Temperature = (sbyte)vMsg.Temperature;
            tv.Over_Weight = (int)vMsg.OverWeight;
            tv.Over_Weight_Ratio = vMsg.OverWeightRatio;

            tv.Axle1 = vMsg.AxleWeights[0];
            tv.Axle2 = vMsg.AxleWeights[1];
            tv.Axle3 = vMsg.AxleWeights[2];
            tv.Axle4 = vMsg.AxleWeights[3];
            tv.Axle5 = vMsg.AxleWeights[4];
            tv.Axle6 = vMsg.AxleWeights[5];
            tv.Axle7 = vMsg.AxleWeights[6];
            tv.Axle8 = vMsg.AxleWeights[7];
            tv.Axle9 = vMsg.AxleWeights[8];
            tv.Axle10 = vMsg.AxleWeights[9];

            tv.Axle_space1 = vMsg.axleSpaces[0];
            tv.Axle_space2 = vMsg.axleSpaces[1];
            tv.Axle_space3 = vMsg.axleSpaces[2];
            tv.Axle_space4 = vMsg.axleSpaces[3];
            tv.Axle_space5 = vMsg.axleSpaces[4];
            tv.Axle_space6 = vMsg.axleSpaces[5];
            tv.Axle_space7 = vMsg.axleSpaces[6];
            tv.Axle_space8 = vMsg.axleSpaces[7];
            tv.Axle_space9 = vMsg.axleSpaces[8];
            tv.Station_IP = port;
            tv.WIM_Id = "";
            tv.LPR_id = "";

            if (vMsg.OverWeightDataState == -1)
            {
                tva = new t_vehicle_msg_abnormal();
                tva.id_local = tv.id_local;//用UUID作为当前一条数据的ID。
                tva.Id = tv.Id;
                tva.Station_Id = tv.Station_Id;
                tva.Evt_Time = tv.Evt_Time;
                tva.Msg_Time = tv.Msg_Time;
                tva.Lane_No = tv.Lane_No;
                tva.Plate = tv.Plate;
                tva.Plate_Color = tv.Plate_Color;
                tva.Class_Index = tv.Class_Index;
                tva.Length = tv.Length;
                tva.Speed = tv.Speed;
                tva.Direction = tv.Direction;
                tva.Axles_Count = tv.Axles_Count;
                tva.Total_Weight = tv.Total_Weight;
                tva.Is_Straddle = tv.Is_Straddle;
                tva.Temperature = tv.Temperature;
                tva.Over_Weight = tv.Over_Weight;
                tva.Over_Weight_Ratio = tv.Over_Weight_Ratio;

                tva.Axle1 = tv.Axle1;
                tva.Axle2 = tv.Axle2;
                tva.Axle3 = tv.Axle3;
                tva.Axle4 = tv.Axle4;
                tva.Axle5 = tv.Axle5;
                tva.Axle6 = tv.Axle6;
                tva.Axle7 = tv.Axle7;
                tva.Axle8 = tv.Axle8;
                tva.Axle9 = tv.Axle9;
                tva.Axle10 = tv.Axle10;

                tva.Axle_space1 = tv.Axle_space1;
                tva.Axle_space2 = tv.Axle_space2;
                tva.Axle_space3 = tv.Axle_space3;
                tva.Axle_space4 = tv.Axle_space4;
                tva.Axle_space5 = tv.Axle_space5;
                tva.Axle_space6 = tv.Axle_space6;
                tva.Axle_space7 = tv.Axle_space7;
                tva.Axle_space8 = tv.Axle_space8;
                tva.Axle_space9 = tv.Axle_space9;
                tva.Station_IP = tv.Station_IP;
                tva.WIM_Id = tv.WIM_Id;
                tva.LPR_id = tv.LPR_id;
            }
            
            if ((DateTime.Compare(DateTime.Now.Date, tv.Evt_Time.Date) == 0))
            {
                tvt = new t_vehicle_msg_today();
                tvt.id_local = tv.id_local;//用UUID作为当前一条数据的ID。
                tvt.Id = tv.Id;
                tvt.Station_Id = tv.Station_Id;
                tvt.Evt_Time = tv.Evt_Time;
                tvt.Msg_Time = tv.Msg_Time;
                tvt.Lane_No = tv.Lane_No;
                tvt.Plate = tv.Plate;
                tvt.Plate_Color = tv.Plate_Color;
                tvt.Class_Index = tv.Class_Index;
                tvt.Length = tv.Length;
                tvt.Speed = tv.Speed;
                tvt.Direction = tv.Direction;
                tvt.Axles_Count = tv.Axles_Count;
                tvt.Total_Weight = tv.Total_Weight;
                tvt.Is_Straddle = tv.Is_Straddle;
                tvt.Temperature = tv.Temperature;
                tvt.Over_Weight = tv.Over_Weight;
                tvt.Over_Weight_Ratio = tv.Over_Weight_Ratio;

                tvt.Axle1 = tv.Axle1;
                tvt.Axle2 = tv.Axle2;
                tvt.Axle3 = tv.Axle3;
                tvt.Axle4 = tv.Axle4;
                tvt.Axle5 = tv.Axle5;
                tvt.Axle6 = tv.Axle6;
                tvt.Axle7 = tv.Axle7;
                tvt.Axle8 = tv.Axle8;
                tvt.Axle9 = tv.Axle9;
                tvt.Axle10 = tv.Axle10;

                tvt.Axle_space1 = tv.Axle_space1;
                tvt.Axle_space2 = tv.Axle_space2;
                tvt.Axle_space3 = tv.Axle_space3;
                tvt.Axle_space4 = tv.Axle_space4;
                tvt.Axle_space5 = tv.Axle_space5;
                tvt.Axle_space6 = tv.Axle_space6;
                tvt.Axle_space7 = tv.Axle_space7;
                tvt.Axle_space8 = tv.Axle_space8;
                tvt.Axle_space9 = tv.Axle_space9;
                tvt.Station_IP = tv.Station_IP;
                tvt.WIM_Id = tv.WIM_Id;
                tvt.LPR_id = tv.LPR_id;
            }
            /*      if (tv.Total_Weight >= 80000)
                  {
                      tvot = new t_vehicle_overweight_temp();
                      tvot.id_local = tv.id_local;
                      tvot.Id = tv.Id;
                      tvot.Station_Id = tv.Station_Id;
                      tvot.Evt_Time = tv.Evt_Time;
                      tvot.Msg_Time = tv.Msg_Time;
                      tvot.Lane_No = tv.Lane_No;
                      tvot.Plate = tv.Plate;
                      tvot.Plate_Color = tv.Plate_Color;
                      tvot.Class_Index = tv.Class_Index;
                      tvot.Length = tv.Length;
                      tvot.Speed = tv.Speed;
                      tvot.Direction = tv.Direction;
                      tvot.Axles_Count = tv.Axles_Count;
                      tvot.Total_Weight = tv.Total_Weight;
                      tvot.Is_Straddle = tv.Is_Straddle;
                      tvot.Temperature = tv.Temperature;
                      tvot.Over_Weight = tv.Over_Weight;
                      tvot.Over_Weight_Ratio = tv.Over_Weight_Ratio;

                      tvot.Axle1 = tv.Axle1;
                      tvot.Axle2 = tv.Axle2;
                      tvot.Axle3 = tv.Axle3;
                      tvot.Axle4 = tv.Axle4;
                      tvot.Axle5 = tv.Axle5;
                      tvot.Axle6 = tv.Axle6;
                      tvot.Axle7 = tv.Axle7;
                      tvot.Axle8 = tv.Axle8;
                      tvot.Axle9 = tv.Axle9;
                      tvot.Axle10 = tv.Axle10;

                      tvot.Axle_space1 = tv.Axle_space1;
                      tvot.Axle_space2 = tv.Axle_space2;
                      tvot.Axle_space3 = tv.Axle_space3;
                      tvot.Axle_space4 = tv.Axle_space4;
                      tvot.Axle_space5 = tv.Axle_space5;
                      tvot.Axle_space6 = tv.Axle_space6;
                      tvot.Axle_space7 = tv.Axle_space7;
                      tvot.Axle_space8 = tv.Axle_space8;
                      tvot.Axle_space9 = tv.Axle_space9;
                      tvot.Station_IP = tv.Station_IP;
                      tvot.WIM_Id = tv.WIM_Id;
                      tvot.LPR_id = tv.LPR_id;
                  }
                  */
            if (tv.Over_Weight != 0)
            {
                tvo = new t_vehicle_overweight();
                tvo.id_local = tv.id_local;
                tvo.Id = tv.Id;
                tvo.Station_Id = tv.Station_Id;
                tvo.Evt_Time = tv.Evt_Time;
                tvo.Msg_Time = tv.Msg_Time;
                tvo.Lane_No = tv.Lane_No;
                tvo.Plate = tv.Plate;
                tvo.Plate_Color = tv.Plate_Color;
                tvo.Class_Index = tv.Class_Index;
                tvo.Length = tv.Length;
                tvo.Speed = tv.Speed;
                tvo.Direction = tv.Direction;
                tvo.Axles_Count = tv.Axles_Count;
                tvo.Total_Weight = tv.Total_Weight;
                tvo.Is_Straddle = tv.Is_Straddle;
                tvo.Temperature = tv.Temperature;
                tvo.Over_Weight = tv.Over_Weight;
                tvo.Over_Weight_Ratio = tv.Over_Weight_Ratio;

                tvo.Axle1 = tv.Axle1;
                tvo.Axle2 = tv.Axle2;
                tvo.Axle3 = tv.Axle3;
                tvo.Axle4 = tv.Axle4;
                tvo.Axle5 = tv.Axle5;
                tvo.Axle6 = tv.Axle6;
                tvo.Axle7 = tv.Axle7;
                tvo.Axle8 = tv.Axle8;
                tvo.Axle9 = tv.Axle9;
                tvo.Axle10 = tv.Axle10;

                tvo.Axle_space1 = tv.Axle_space1;
                tvo.Axle_space2 = tv.Axle_space2;
                tvo.Axle_space3 = tv.Axle_space3;
                tvo.Axle_space4 = tv.Axle_space4;
                tvo.Axle_space5 = tv.Axle_space5;
                tvo.Axle_space6 = tv.Axle_space6;
                tvo.Axle_space7 = tv.Axle_space7;
                tvo.Axle_space8 = tv.Axle_space8;
                tvo.Axle_space9 = tv.Axle_space9;
                tvo.Station_IP = tv.Station_IP;
                tvo.WIM_Id = tv.WIM_Id;
                tvo.LPR_id = tv.LPR_id;
            }

            dtczEntities1 de = new dtczEntities1();
            //先判断是否为异常数据，不是异常数据正常插入，判断是否超重。异常数据直接插入异常表
            if (vMsg.OverWeightDataState == 1)
            {
                de.t_vehicle_msg.Add(tv);
                if (tvt != null)
                {
                    de.t_vehicle_msg_today.Add(tvt);
                }
                if (tvot != null)
                {
                    if (vMsg.ImgData.Length > 0)
                    {
                        SysLog.WriteLog($"图片id名称:{tv.id_local}--Image,图片大小：{vMsg.ImgData.Length}");
                        Console.WriteLine($"图片id名称:{tv.id_local},图片大小：{vMsg.ImgData.Length}");
                        SaveImage2(vMsg.ImgData, port, tv.id_local, "Image",tv);
                    }
                    SysLog.WriteLog($"车辆超重超载将计入数据库");
                    Console.WriteLine($"车辆超重超载将计入数据库");
                    de.t_vehicle_overweight_temp.Add(tvot);
                }
                if (tvo != null)
                {
                    if (vMsg.ImgData.Length > 0)
                    {
                        SysLog.WriteLog($"图片id名称:{tv.id_local}--Image,图片大小：{vMsg.ImgData.Length}");
                        Console.WriteLine($"图片id名称:{tv.id_local},图片大小：{vMsg.ImgData.Length}");
                        SaveImage2(vMsg.ImgData, port, tv.id_local, "Image",tv);
                    }
                    SysLog.WriteLog($"车辆超重超载将计入数据库");
                    Console.WriteLine($"车辆超重超载将计入数据库");
                    de.t_vehicle_overweight.Add(tvo);
                    
                    //满足条件后发给json，最后到执法局
                    if (tvo.Over_Weight_Ratio >= 0.05 && tvo.Axles_Count < 8 && tvo.Length < 2200 && tvo.Axle1 < 9000 && tvo.Speed<=100 && !(tvo.Station_IP.Equals("10015")|| tvo.Station_IP.Equals("10016")))
                    {
                        string imgBase64 = Convert.ToBase64String(vMsg.ImgData);
                        string plate_imgBase64 = Convert.ToBase64String(vMsg.PlateImgData);
                        SysLog.WriteLog($"发送出json数据");
                        Console.WriteLine($"发送出json数据");
                        sendTvoJson(tvo,plate_imgBase64 ,imgBase64);
                    }
                    
                    
                }      
            }else
            {
                if (vMsg.ImgData.Length > 0)
                {
                    SysLog.WriteLog($"图片id名称:{tv.id_local}--Image,图片大小：{vMsg.ImgData.Length}");
                    Console.WriteLine($"图片id名称:{tv.id_local},图片大小：{vMsg.ImgData.Length}");
                    SaveImage2(vMsg.ImgData, port, tv.id_local, "Image",tv);
                }
                SysLog.WriteLog($"车辆超重超载数据异常，将计入异常数据库");
                Console.WriteLine($"车辆超重超载数据异常，将计入异常数据库");
                de.t_vehicle_msg_abnormal.Add(tva);
            }
            try
            {
                int c = de.SaveChanges();
                SysLog.WriteLog($"车辆信息写入成功,1条记录");
                Console.WriteLine($"车辆信息写入成功,1条记录");
            }
            catch (Exception err)
            {
                SysLog.WriteError($"车辆信息存入数据库出错，将写入本地：{err.Message}");
                Console.WriteLine($"车辆信息存入数据库出错，将写入本地：{err.Message}");
                saveInFile2(tv.Station_IP, tv,vMsg.ImgData,vMsg.PlateImgData,vMsg.OverWeightDataState);
            }

            if (vMsg.PlateImgData.Length > 0)
            {
                SysLog.WriteLog($"图片id名称:{tv.id_local}--PlateImg,图片大小：{vMsg.PlateImgData.Length}");
                Console.WriteLine($"图片id名称:{tv.id_local},图片大小：{vMsg.PlateImgData.Length}");
                SaveImage2(vMsg.PlateImgData, port, tv.id_local, "PlateImg",tv);
            }
            if (vMsg.LaserImgData.Length > 0)
            {
                SysLog.WriteLog($"图片id名称:{tv.id_local}--LaserImg,图片大小：{vMsg.LaserImgData.Length}");
                Console.WriteLine($"图片id名称:{tv.id_local},图片大小：{vMsg.LaserImgData.Length}");
                SaveImage2(vMsg.LaserImgData, port, tv.id_local, "LaserImg",tv);
            }
            Console.WriteLine("--------------------------------------------------------------------------------");
            vMsg = null;
            de = null;
            tv = null;
            tvt = null;
            tvot = null;
            tvo = null;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            dtczEntities1 de = new dtczEntities1();
            List<station_site> datas = de.station_site.Where(p => p.Del_State == 1 && p.Station_Port.Equals(port)).AsQueryable().ToList();
            if (receivedCount > 0)
            {
                datas[0].received_state = 1;
                receivedCount = 0;
            }
            else
            {
                datas[0].received_state = 0;
            }
            //de.station_site.Add(datas[0]);
            try
            {
                SysLog.WriteLog("[定时任务]定时判断接收状态");
                Console.WriteLine("[定时任务]定时判断接收状态");
                de.SaveChanges();
            }
            catch (Exception eirl)
            {
                SysLog.WriteError("[定时任务]定时判断接收状态保存出错" + eirl.Message);
                Console.WriteLine("[定时任务]定时判断接收状态保存出错" + eirl.Message);

            }
        }

        //用新线程去保存图片，避免保存图片过程中接收不到其它数据
        private static void SaveImage(byte[] imageBytes, string port, string id, string imgType)
        {
            //string path = "V:\\port" + port+"\\";
            string path = "port" + port + "\\";
            // string path = "E:\\DTCZ\\image_car\\port"+port;
            string filename = id + "-" + imgType + ".jpg";
            string fullname = Path.Combine(path, filename);
            /*if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream fs = File.Create(fullname))
            {
                fs.Write(imageBytes,0,imageBytes.Length);
                fs.Close();
                return 1;
            }*/

            using (var ftp = new FtpClient("cs.ldxx.top", 21, "dtcz", "ldxx`123"))
            {
                try
                {
                    ftp.DataConnectionType = FtpDataConnectionType.PORT;
                    if (!ftp.DirectoryExists(path))
                    {
                        ftp.CreateDirectory(path);
                    }
                    ftp.Upload(imageBytes, fullname);
                    Console.WriteLine($"图片已上传");
                }
                catch (Exception e)
                {
                    SysLog.WriteLog(e.Message);
                }
            }
            return;
        }

        private static void SaveImage2(byte[] imageBytes, string port, string id, string imgType,t_vehicle_msg tv)
        {
            int year = tv.Evt_Time.Year;
            string monthAndDay = tv.Evt_Time.ToString("MMdd");
            string path = "E:\\Program Files\\DTCZ\\DTCZimage\\port" + port + "\\"+year+"\\"+monthAndDay+"\\";
            string filename = id + "-" + imgType + ".jpg";
            string fullname = Path.Combine(path, filename);
            string imgBase64 = Convert.ToBase64String(imageBytes);

            WebClient webClient = new WebClient();
            //拼接post字符串
            NameValueCollection postValues = new NameValueCollection();
            postValues.Add("saveDiskUrl", fullname);
            postValues.Add("imgCode", imgBase64);

            System.Net.ServicePointManager.Expect100Continue = false;
            //ssl证书取消验证
            ServicePointManager.ServerCertificateValidationCallback =
                       delegate { return true; };
            string response = string.Empty;
            byte[] rByte;
            
            try
            {

                rByte = webClient.UploadValues("https://139.198.176.196:8071/down/saveImg", postValues);
                if (System.Text.Encoding.ASCII.GetString(rByte).Equals("true"))
                {
                    Console.WriteLine($"图片已上传");
                }
                else
                {
                    Console.WriteLine($"图片上传失败，将保存到本地");
                    SaveImageLocal(imageBytes, port, id, imgType,tv);
                }

            }
            catch (Exception ei)
            {
                Console.WriteLine($"图片上传失败，将保存到本地");
                SaveImageLocal(imageBytes, port, id, imgType,tv);
                SysLog.WriteError("图片上传失败,将保存到本地");
                SysLog.WriteError(ei.Message);
            }
            
            return;
        }
        private static void SaveImageLocal(byte[] imageBytes, string port, string id, string imgType,t_vehicle_msg tv)
        {
            int year = tv.Evt_Time.Year;
            string monthAndDay = tv.Evt_Time.ToString("MMdd");
            string imageFileName =port +"$"+year + "$" +monthAndDay + "$" + id + "-" + imgType + ".jpg";
            string imgPath = Application.StartupPath + "\\dataImageTemp\\";
            imageFileName = Path.Combine(imgPath, imageFileName);

            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }

            using (FileStream fs = File.Create(imageFileName))
            {
                fs.Write(imageBytes, 0, imageBytes.Length);
                fs.Close();
            }
        }

        //当网络连接断开时，写入数据到本地，等网络连通再上传
        private static void saveInFile(string port, t_vehicle_msg tv)
        {
            string fileName = port + "_" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".csv";
            string path = Application.StartupPath + "\\dataTemp\\";
            string fullname = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + tv.id_local + "\",");
            sb.Append("\"" + tv.Id + "\",");
            sb.Append("\"" + tv.Station_Id + "\",");
            sb.Append("\"" + tv.Evt_Time + "\",");
            sb.Append("\"" + tv.Msg_Time + "\",");
            sb.Append("\"" + tv.Lane_No + "\",");
            sb.Append("\"" + tv.Plate + "\",");
            sb.Append("\"" + tv.Plate_Color + "\",");
            sb.Append("\"" + tv.Class_Index + "\",");
            sb.Append("\"" + tv.Length + "\",");
            sb.Append("\"" + tv.Speed + "\",");
            sb.Append("\"" + tv.Direction + "\",");
            sb.Append("\"" + tv.Axles_Count + "\",");
            sb.Append("\"" + tv.Total_Weight + "\",");
            sb.Append("\"" + tv.Axle1 + "\",");
            sb.Append("\"" + tv.Axle2 + "\",");
            sb.Append("\"" + tv.Axle3 + "\",");
            sb.Append("\"" + tv.Axle4 + "\",");
            sb.Append("\"" + tv.Axle5 + "\",");
            sb.Append("\"" + tv.Axle6 + "\",");
            sb.Append("\"" + tv.Axle7 + "\",");
            sb.Append("\"" + tv.Axle8 + "\",");
            sb.Append("\"" + tv.Axle9 + "\",");
            sb.Append("\"" + tv.Axle10 + "\",");
            sb.Append("\"" + tv.Axle_space1 + "\",");
            sb.Append("\"" + tv.Axle_space2 + "\",");
            sb.Append("\"" + tv.Axle_space3 + "\",");
            sb.Append("\"" + tv.Axle_space4 + "\",");
            sb.Append("\"" + tv.Axle_space5 + "\",");
            sb.Append("\"" + tv.Axle_space6 + "\",");
            sb.Append("\"" + tv.Axle_space7 + "\",");
            sb.Append("\"" + tv.Axle_space8 + "\",");
            sb.Append("\"" + tv.Axle_space9 + "\",");
            sb.Append("\"" + tv.Is_Straddle + "\",");
            sb.Append("\"" + tv.Temperature + "\",");
            sb.Append("\"" + tv.Over_Weight + "\",");
            sb.Append("\"" + tv.Over_Weight_Ratio + "\",");
            sb.Append("\"" + tv.WIM_Id + "\",");
            sb.Append("\"" + tv.LPR_id + "\",");
            sb.Append("\"" + tv.Station_IP + "\"");
            object lockThis = new object();
            StreamWriter f = null;
            lock (lockThis)
            {
                try
                {

                    SysLog.WriteLog("[写入本地]数据库连接异常，数据写入本地文件：" + fileName);
                    Console.WriteLine("[写入本地]数据库连接异常，数据写入本地文件：" + fileName);
                    f = new StreamWriter(fullname, true);
                    f.WriteLine(sb);
                }
                catch (Exception ef)
                {
                    SysLog.WriteError("[写入本地]文件写入失败：" + fileName);
                    Console.WriteLine("[写入本地]文件写入失败：" + fileName);
                }
                finally
                {
                    f.Close();
                }
            }
        }

        private static void saveInFile2(string port, t_vehicle_msg tv, byte[] imageBytes, byte[] plateimageBytes,int overWeightDataState)
        {

            string fileName = tv.id_local + ".csv";
            string path = Application.StartupPath + "\\dataTemp\\";
            string fullname = "";
            try {
                fullname = Path.Combine(path, fileName);
            }
            catch (ArgumentException e) {
                SysLog.WriteError("[文件写入错误]路径拼接失败：path="+ path+"filename="+ fileName+ "\r\ne.Message");
                Console.WriteLine("[文件写入错误]路径拼接失败：path=" + path + "filename=" + fileName+"\r\ne.Message");
                return;
            }
            

            

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + tv.id_local + "\",");
            sb.Append("\"" + tv.Id + "\",");
            sb.Append("\"" + tv.Station_Id + "\",");
            sb.Append("\"" + tv.Evt_Time + "\",");
            sb.Append("\"" + tv.Msg_Time + "\",");
            sb.Append("\"" + tv.Lane_No + "\",");
            sb.Append("\"" + tv.Plate + "\",");
            sb.Append("\"" + tv.Plate_Color + "\",");
            sb.Append("\"" + tv.Class_Index + "\",");
            sb.Append("\"" + tv.Length + "\",");
            sb.Append("\"" + tv.Speed + "\",");
            sb.Append("\"" + tv.Direction + "\",");
            sb.Append("\"" + tv.Axles_Count + "\",");
            sb.Append("\"" + tv.Total_Weight + "\",");
            sb.Append("\"" + tv.Axle1 + "\",");
            sb.Append("\"" + tv.Axle2 + "\",");
            sb.Append("\"" + tv.Axle3 + "\",");
            sb.Append("\"" + tv.Axle4 + "\",");
            sb.Append("\"" + tv.Axle5 + "\",");
            sb.Append("\"" + tv.Axle6 + "\",");
            sb.Append("\"" + tv.Axle7 + "\",");
            sb.Append("\"" + tv.Axle8 + "\",");
            sb.Append("\"" + tv.Axle9 + "\",");
            sb.Append("\"" + tv.Axle10 + "\",");
            sb.Append("\"" + tv.Axle_space1 + "\",");
            sb.Append("\"" + tv.Axle_space2 + "\",");
            sb.Append("\"" + tv.Axle_space3 + "\",");
            sb.Append("\"" + tv.Axle_space4 + "\",");
            sb.Append("\"" + tv.Axle_space5 + "\",");
            sb.Append("\"" + tv.Axle_space6 + "\",");
            sb.Append("\"" + tv.Axle_space7 + "\",");
            sb.Append("\"" + tv.Axle_space8 + "\",");
            sb.Append("\"" + tv.Axle_space9 + "\",");
            sb.Append("\"" + tv.Is_Straddle + "\",");
            sb.Append("\"" + tv.Temperature + "\",");
            sb.Append("\"" + tv.Over_Weight + "\",");
            sb.Append("\"" + tv.Over_Weight_Ratio + "\",");
            sb.Append("\"" + tv.WIM_Id + "\",");
            sb.Append("\"" + tv.LPR_id + "\",");
            sb.Append("\"" + tv.Station_IP + "\",");
            sb.Append("\"" + overWeightDataState + "\"");
            object lockThis = new object();
            StreamWriter f = null;
            lock (lockThis)
            {
                try
                {

                    SysLog.WriteLog("[写入本地]数据库连接异常，数据写入本地文件：" + fileName);
                    Console.WriteLine("[写入本地]数据库连接异常，数据写入本地文件：" + fileName);
                    f = new StreamWriter(fullname, true);
                    f.WriteLine(sb);
                }
                catch (Exception ef)
                {
                    SysLog.WriteError("[写入本地]文件写入失败：" + fileName);
                    Console.WriteLine("[写入本地]文件写入失败：" + fileName);
                }
                finally
                {
                    f.Close();
                }
            }
        }


        //
        public static void sendTvoJson(t_vehicle_overweight tvo, string plate_imgBase64, string imgBase64)
        {

            WebClient webClient = new WebClient();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"id\":\""+tvo.Id+"\",");
            sb.Append("\"idLocal\":\"" + tvo.id_local + "\",");
            sb.Append("\"stationId\":\"" + tvo.Station_Id + "\",");
            sb.Append("\"evtTime\":\"" + tvo.Evt_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
            sb.Append("\"msgTime\":\"" + tvo.Msg_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
            sb.Append("\"laneNo\":\"" + tvo.Lane_No + "\",");
            sb.Append("\"plate\":\"" + tvo.Plate + "\",");
            sb.Append("\"plateColor\":\"" + tvo.Plate_Color + "\",");
            sb.Append("\"classLndex\":\"" + tvo.Class_Index + "\",");
            sb.Append("\"length\":\"" + tvo.Length + "\",");
            sb.Append("\"speed\":\"" + tvo.Speed + "\",");
            sb.Append("\"direcation\":\"" + tvo.Direction + "\",");
            sb.Append("\"axlesCount\":\"" + tvo.Axles_Count + "\",");
            sb.Append("\"totalWeight\":\"" + tvo.Total_Weight + "\",");
            sb.Append("\"axle1\":\"" + tvo.Axle1 + "\",");
            sb.Append("\"axle2\":\"" + tvo.Axle2 + "\",");
            sb.Append("\"axle3\":\"" + tvo.Axle3 + "\",");
            sb.Append("\"axle4\":\"" + tvo.Axle4 + "\",");
            sb.Append("\"axle5\":\"" + tvo.Axle5 + "\",");
            sb.Append("\"axle6\":\"" + tvo.Axle6 + "\",");
            sb.Append("\"axle7\":\"" + tvo.Axle7 + "\",");
            sb.Append("\"axle8\":\"" + tvo.Axle8 + "\",");
            sb.Append("\"axle9\":\"" + tvo.Axle9 + "\",");
            sb.Append("\"axle10\":\"" + tvo.Axle10 + "\",");
            sb.Append("\"axleSpace1\":\"" + tvo.Axle_space1 + "\",");
            sb.Append("\"axleSpace2\":\"" + tvo.Axle_space2 + "\",");
            sb.Append("\"axleSpace3\":\"" + tvo.Axle_space3 + "\",");
            sb.Append("\"axleSpace4\":\"" + tvo.Axle_space4 + "\",");
            sb.Append("\"axleSpace5\":\"" + tvo.Axle_space5 + "\",");
            sb.Append("\"axleSpace6\":\"" + tvo.Axle_space6 + "\",");
            sb.Append("\"axleSpace7\":\"" + tvo.Axle_space7 + "\",");
            sb.Append("\"axleSpace8\":\"" + tvo.Axle_space8 + "\",");
            sb.Append("\"axleSpace9\":\"" + tvo.Axle_space9 + "\",");
            sb.Append("\"isStraddle\":\"" + tvo.Is_Straddle + "\",");
            sb.Append("\"temperature\":\"" + tvo.Temperature + "\",");
            sb.Append("\"overWeight\":\"" + tvo.Over_Weight + "\",");
            sb.Append("\"overWeightRatio\":\"" + tvo.Over_Weight_Ratio + "\",");
            sb.Append("\"wimId\":\"" + tvo.WIM_Id + "\",");
            sb.Append("\"lprId\":\"" + tvo.LPR_id + "\",");
            sb.Append("\"stationIp\":\"" + tvo.Station_IP + "\",");
            sb.Append("\"stationName\":\"" + getStation_Name(tvo.Station_IP) + "\",");
            sb.Append("\"deviceNo\":\"" + getDeviceNo(tvo.Station_IP) + "\",");
            sb.Append("\"vehicleWeight\":\"" + tvo.Total_Weight + "\",");
            sb.Append("\"maxWeight\":\"" + getMaxWeight((uint)tvo.Axles_Count) + "\",");
            sb.Append("\"isBulkcar\":\"" + "0" + "\",");
            sb.Append("\"imgBinData\":\"" + plate_imgBase64 + "\",");
            sb.Append("\"imgData\":\"" + imgBase64 + "\"");
            sb.Append("}");

            byte[] bstring = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            string sendString = Convert.ToBase64String(bstring);
            sendString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sendString));

            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers.Add("token", "bXpqbVI1clBUR1k1TGVoejVLOEVKTU01ek1yTWpPOVg=");
            System.Net.ServicePointManager.Expect100Continue = false;
            string response = string.Empty;
            bool isSuccess = false;
            try
            {
                //response = webClient.UploadString("http://139.198.176.206:1914/audrate/weight/add", sb.ToString());
                response = webClient.UploadString("http://139.198.176.206:1980/audgateway/audrate/weight/add", sendString);
                //取出success后面的值，看是成功还是失败
                string s = response;
                s = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(response));
                s = s.Substring(s.LastIndexOf("success") + 9);
                string[] split = s.Split(new char[] { ',', '}' });
                isSuccess = split[0].Trim().Equals("true");
            }
            catch (Exception e)
            {
                SysLog.WriteError(e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine("[上传json]：失败,出现Exception");
            }

            if (isSuccess)
            {
                SysLog.WriteLog("[上传json]：成功");
                Console.WriteLine("[上传json]：成功");
            }
            else
            {
                SysLog.WriteLog("[上传json]：失败");
                Console.WriteLine("[上传json]：失败");
            }
        }

        private static int getMaxWeight(uint Axles_Count)
        {
            switch (Axles_Count)
            {
                case 2: return 18000;
                case 3: return 27000;
                case 4: return 36000;
                case 5: return 43000;
                case 6: return 49000;
                default: return 49000;
            }
        }
        private static int getDeviceNo(string port)
        {
            switch (port)
            {
                case "10001": return 34;
                case "10002": return 34;
                case "10003": return 37;
                case "10004": return 37;
                case "10005": return 34;
                case "10006": return 37;
                case "10007": return 37;
                case "10008": return 31;
                case "10009": return 31;
                case "100010": return 33;
                case "100011": return 33;
                case "100012": return 34;
                case "100013": return 34;
                case "100014": return 37;
                case "100015": return 34;
                case "100016": return 31;
                default: return 0;
            }
        }
        private static string getStation_Name(string Station_IP)
        {
            switch (Station_IP)
            {
                case "10001": return "G1516盐洛苏皖界";
                case "10002": return "S65徐明苏皖界";
                case "10003": return "G3京台苏鲁界";
                case "10004": return "S69济徐苏鲁界";
                case "10005": return "G30连霍苏皖界";
                case "10006": return "G15沈海苏鲁界";
                case "10007": return "G25长深苏鲁界";
                case "10008": return "G2京沪苏沪界";
                case "10009": return "G50沪渝苏沪界";
                case "10010": return "G50沪渝苏浙界";
                case "10011": return "G25长深苏浙界";
                case "10012": return "G4221沪武苏皖界";
                case "10013": return "S68溧芜苏皖界";
                case "10014": return "G2京沪苏鲁界";
                case "10015": return "G40沪陕苏沪界";
                case "10016": return "G36宁洛苏皖界";
                default: return "";
            }
        }

    }
}