using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Rabbit.Enums;
using Tool;
using System.Threading;

namespace ImageBL
{
    /// <summary>
    /// 请求数据处理
    /// </summary>
    public class Handle 
    {

        #region 参数

        /// <summary>
        /// 保存地址
        /// </summary>
        private string SaveFile { get; set; } = System.Configuration.ConfigurationManager.AppSettings["souPath"];
        private string ServerPath { get; set; } = System.Configuration.ConfigurationManager.AppSettings["Path"];

        #endregion

        #region public

        /// <summary>
        /// 一个门
        /// </summary>
        /// <param name="doorId"></param>
        /// <returns></returns>
        public Dictionary<string, string> OneDoorImg(int doorId)
        {
            try
            {
                var ModelContext = new RabbitMPEntities();
                var door = ModelContext.Doors
                    .Include(nameof(Doors.CommunityEntities))
                    .Include(nameof(Doors.WechatQrCodes))
                    .Include(nameof(Doors.DoorGroups))
                    .FirstOrDefault(item => item.DoorId == doorId);
                var newDoorName = door.DoorGroups != null
                        ? door.DoorGroups.Name.Split('|')[0] + "-" + door.Name.Split('|')[1]
                        : door.Name.Replace("|", "-");
                GeneratePictures gp = new GeneratePictures(SaveFile, (int)door.CommunityId, doorId, door.CommunityEntities.Name, newDoorName, ServerPath, SaveFile + "sou.jpg", SaveFile + "small.jpg");
                if (door != null && door.WechatQrCodes != null&&door.WechatQrCodes.WechatUrl!=null)
                {
                    var msg = gp.Generate(door.WechatQrCodes.WechatUrl,WechatQrCodeType.DM);
                    return msg;
                }
                else if (door != null && (door.WechatQrCodes == null||door.WechatQrCodes.WechatUrl==null))
                {
                    var newWechatQR = InsertOneWechatQrCodes(WechatQrCodeType.DM);
                    door.WechatQrCodeId = newWechatQR.Id;
                    ModelContext.SaveChanges();
                    var msg = gp.Generate(newWechatQR.WechatUrl,WechatQrCodeType.DM);
                    return msg;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                AboutLog.WriteBugLog(e);
                return null;
            }
        }


        /// <summary>
        /// 一个社区码
        /// </summary>
        /// <param name="communityId">社区码</param>
        /// <returns></returns>
        public Dictionary<string, string> OneCommunity(int communityId)
        {
            try
            {
                var ModelContext = new RabbitMPEntities();
                var community = ModelContext.CommunityEntities
                    .Include(nameof(CommunityEntities.WechatQrCodes))
                    .FirstOrDefault(item => item.CommunityId == communityId);
                GeneratePictures gp = new GeneratePictures(SaveFile, communityId, communityId, community.Name, community.Name, ServerPath, SaveFile + "sou.jpg", SaveFile + "small.jpg");
                if (community != null && community.WechatQrCodes != null)
                {
                    var msg = gp.Generate(community.WechatQrCodes.WechatUrl,WechatQrCodeType.SQ);
                    return msg;
                }
                else if(community!=null&&community.WechatQrCodes==null)
                {
                    var newWechatQR = InsertOneWechatQrCodes(WechatQrCodeType.SQ);
                    community.WechatQrCodeId = newWechatQR.Id;
                    ModelContext.SaveChanges();
                    var msg = gp.Generate(newWechatQR.WechatUrl, WechatQrCodeType.SQ);
                    return msg;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                AboutLog.WriteBugLog(e);
                return null;
            }
        }




        /// <summary>
        /// 并行获取社区下所有门二维码
        /// </summary>
        /// <param name="communtiyId"></param>
        /// <returns></returns>
        public List<object> ParallelDoorImgList(int communtiyId)
        {
            try
            {
                var ModelContext = new RabbitMPEntities();
                var doors = ModelContext.Doors
                    .Include(nameof(Doors.CommunityEntities))
                    .Include(nameof(Doors.DoorGroups))
                    .Where(item => item.IsEnable && item.CommunityId == communtiyId);
                if (doors == null) return null;
                var dictionary = new List<object>();
                Parallel.ForEach(doors, (item) =>
                {
                    dictionary.Add(Door(item));
                });

                //foreach (var d in doors)
                //{
                //    dictionary.Add(Door(d));
                //}
                return dictionary;
            }
            catch (Exception e)
            {
                AboutLog.WriteBugLog(e);
                return null;
            }
        }



        /// <summary>
        ///  并行获取多个门二维码
        /// </summary>
        /// <param name="doorIds">门id，（传入多个门id，用半月分号 ; 分隔）</param>
        /// <returns></returns>
        public List<object> ParallelSomeDoorImg(string doorIds)
        {
            try
            {
                var ModelContext = new RabbitMPEntities();
                var doorid = SeparateDoorIds(doorIds, ';');
                var doors = ModelContext.Doors.Include(nameof(Doors.CommunityEntities)).Where(item => doorid.Contains(item.DoorId));
                if (doors == null) return null;
                var dictionary = new List<object>();
                Parallel.ForEach(doors, (item) =>
                {
                    dictionary.Add(Door(item));
                });

                //foreach (var d in doors)
                //{
                //    dictionary.Add(Door(d));
                //}

                return dictionary;
            }
            catch (Exception e)
            {
                AboutLog.WriteBugLog(e);
                return null;
            }
        }



        #endregion

        #region private


        /// <summary>
        /// 处理门
        /// </summary>
        /// <param name="door"></param>
        /// <returns></returns> 
        private object Door(Doors door)
        {
            var newDoorName = door.DoorGroups != null
                       ? door.DoorGroups.Name.Split('|')[0] + "-" + door.Name.Split('|')[1]
                       : door.Name.Replace("|", "-");
            var newdata = new
            {
                communityId = door.CommunityEntities.CommunityId,
                communityName = door.CommunityEntities.Name,
                doorId = door.DoorId,
                doorName = newDoorName,
                doorImg = OneDoorImg(door.DoorId)
            };
            return newdata;
        }


        /// <summary>
        /// 生成一个二维码
        /// </summary>
        /// <param name="wqt">类型</param>
        /// <returns></returns>
        private WechatQrCodes InsertOneWechatQrCodes(WechatQrCodeType wqt)
        {
            try
            {
                var ModelContext = new RabbitMPEntities();
                var e = (int)Enum.Parse(typeof(WechatQrCodeType), wqt.ToString());
                var Max = ModelContext.WechatQrCodes.Where(item => item.WechatQrCodeType == e).OrderByDescending(item => item.Id).FirstOrDefault();
                var newContent = int.Parse(Max.Content) + 1;
                var newWecahatQR = new WechatQrCodes()
                {
                    WechatUrl = Wechat.WechatQR.GetQrcode(
                        System.Configuration.ConfigurationManager.AppSettings["appid"],
                        System.Configuration.ConfigurationManager.AppSettings["appsecrect"],
                        wqt.ToString() + newContent
                        ),
                    WechatQrCodeType = e,
                    Content = newContent.ToString(),
                    CreateDateTime = DateTime.Now
                };
                ModelContext.WechatQrCodes.Add(newWecahatQR);
                ModelContext.SaveChanges();
                return newWecahatQR;
            }
            catch (Exception e)
            {
                var isbool = AboutLog.WriteBugLog(e);
                return null;
            }

        }


        /// <summary>
        /// 分隔string数组返回int[]
        /// </summary>
        /// <param name="str">要分隔的string</param>
        /// <param name="str">分隔字符</param>
        /// <returns></returns>
        private int[] SeparateDoorIds(string str, char cha)
        {
            try
            {
                var strs = str.Split(new char[] { cha }, StringSplitOptions.RemoveEmptyEntries);
                var results = new int[strs.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    int.TryParse(strs[i], out results[i]);
                }
                return results;
            }
            catch (Exception e)
            {
                AboutLog.WriteBugLog(e);
                return null;
            }
        }


        #endregion
    }
}
