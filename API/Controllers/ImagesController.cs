using ImageBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tool;

namespace API.Controllers
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImagesController : BaseController
    {


        /// <summary>
        /// 保存地址
        /// </summary>
        public string SaveFile { get; set; } = System.Configuration.ConfigurationManager.AppSettings["souPath"];
        private string ServerPath { get; set; } = System.Configuration.ConfigurationManager.AppSettings["Path"];


        /// <summary>
        /// 门二维码
        /// </summary>
        /// <param name="doorId">门id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage OneWechatQR(int doorId)
        {
            Handle handle = new Handle();
            var msg = handle.OneDoorImg(doorId);
            return AboutHttp.ToJson(new
            {
                Code = msg == null ? 500 : 200,
                Info = msg == null ? "门不存在" : "操作成功",
                Data = msg
            });
        }


        /// <summary>
        /// 该社区所有二维码list
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage WechatQRList(int communityId)
        {
            Handle handle = new Handle();
            var msg = handle.ParallelDoorImgList(communityId);
            return AboutHttp.ToJson(new
            {
                Code = msg == null ? 500 : 200,
                Info = msg == null ? "没有门" : "操作成功",
                Data = msg
            });
        }



        /// <summary>
        /// 门二维码List
        /// （传入多个门id，用半月分号 ; 分隔）
        /// </summary>
        /// <param name="doorIds">门id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SomeWechatQR(string doorIds)
        {
            Handle handle = new Handle();
            var msg = handle.ParallelSomeDoorImg(doorIds);
            return AboutHttp.ToJson(new
            {
                Code = msg == null ? 500 : 200,
                Info = msg == null ? "操作成功" : "操作成功",
                Data = msg
            });
        }


        /// <summary>
        /// 一个社区二维码
        /// </summary>
        /// <param name="cId">社区id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage OneCommunity(int cId)
        {
            Handle handle = new Handle();
            var msg = handle.OneCommunity(cId);
            return AboutHttp.ToJson(new
            {
                Code = msg == null ? 500 : 200,
                Info = msg == null ? "社区不存在" : "操作成功",
                Data = msg
            });
        }

    }


}
