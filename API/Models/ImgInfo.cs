using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// 生成图片参数
    /// </summary>
    public class ImgInfo
    {
        /// <summary>
        /// 社区id
        /// </summary>
        public int CommunityId { get; set; }

        /// <summary>
        /// 社区名称
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 门id
        /// </summary>
        public int DoorId { get; set; }

        /// <summary>
        /// 门名称
        /// </summary>
        public string DoorName { get; set; }

        /// <summary>
        /// 原始二维码地址
        /// </summary>
        public string OriginalImgUrl { get; set; }

    }
}