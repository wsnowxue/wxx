using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHttp = System.Web.Http;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Application.IndicatorManage;
using System.Web.Mvc;
using System.Globalization;
using KPI.Code;

namespace KPI.Web.Areas.IndicatorManage.Controllers
{
    public class IndicatorsController : ControllerBase
    {
        private IndicatorsCountMethodApp indicatorsCountMethodApp = new IndicatorsCountMethodApp();
        private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();

        #region 获取系统已经维护的细项指标的统计方式
        /// <summary>
        /// 获取系统已经维护的细项指标的统计方式 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult GetallCountMethod()
        {
            var data = indicatorsCountMethodApp.GetList();
            return Success("操作成功。",data);

        }
        #endregion

        # region 获取系统已经维护的细项指标
        /// <summary>
        /// 获取系统已经维护的细项指标
        /// </summary>
        /// <param name="indicator_name">指标名称 模糊搜索</param>
        /// <param name="indicator_Define">指标定义 模糊搜索</param>
        /// <param name="count_method">统计方式 模糊搜索</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult Getindicators(string indicator_name, string indicator_Define, string count_method, int? statue, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            var data = new List<IndicatorsDefineEntity>();
            data = indicatorsDefineApp.GetItemList(indicator_name, indicator_Define, count_method, statue, pg);
            return Success("操作成功。", data, pg.records);
        }
        #endregion

        #region 新增细项指标
        /// <summary>
        /// 新增细项指标
        /// </summary>
        /// <param name="indicator_name">指标名称 模糊搜索</param>
        /// <param name="indicator_Define">指标定义 模糊搜索</param>
        /// <param name="count_method">统计方式 模糊搜索</param>
        /// <param name="start_index">起始索引</param>
        /// <param name="total">查询条数</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult Addindicators(string indicator_name, string indicator_define, string indicator_count_method, string creator_id)
        {
            if (indicatorsDefineApp.IsExists(indicator_name)) return Error("已存在相同条目。");
            IndicatorsDefineEntity indicatorsDefineEntity = new IndicatorsDefineEntity();
            indicatorsDefineEntity.indicator_name = indicator_name;
            indicatorsDefineEntity.indicator_define = indicator_define;
            indicatorsDefineEntity.indicator_count_method = indicator_count_method;
            indicatorsDefineEntity.statue = 1;

            indicatorsDefineApp.SubmitForm(indicatorsDefineEntity, string.Empty);
            return Success("操作成功。");
           

        }
        #endregion

        #region 启/停用系统已经维护的细项指标
        /// <summary>
        /// 启/停用系统已经维护的细项指标
        /// </summary>
        /// <param name="id">指标id 必填</param>
        /// <param name="statue">状态  0：停用，1：启用 必填</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult Changeindicatorsstatue(string id, int statue)
        {
            IndicatorsDefineEntity indicatorsDefineEntity = new IndicatorsDefineEntity();
            indicatorsDefineEntity.statue = statue;
            indicatorsDefineApp.SubmitForm(indicatorsDefineEntity, id);

            return Success("操作成功。");
        }
        #endregion

    }
}