using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Application.IndicatorManage;
using System.Web.Mvc;
using System.Globalization;
using KPI.Code;

namespace KPI.Web.Areas.IndicatorManage.Controllers
{
    public class DimensionController : ControllerBase
    {
        private DimensionDetailApp dimensionDetailApp = new DimensionDetailApp();
        private DimensionApp dimensionApp = new DimensionApp();
        private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();




        #region 获取系统已经维护的考核维度
        /// <summary>
        /// 获取系统已经维护的考核维度
        /// </summary>
        /// <param name="dimension_name">考核维度名称 模糊搜索</param>
        /// <param name="pagination">分页</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult Getdimension(string dimension_name,int? statue,string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            List<DimensionEntity> retData = dimensionApp.GetItemList(dimension_name, statue, pg);
            return Success("操作成功。", retData, pg.records);
            

        }
        #endregion

        #region 新增考核维度
        /// <summary>
        /// 新增考核维度
        /// </summary>
        /// <param name="dimension_name">考核维度名称</param>
        /// <param name="creator_id">当前操作者id</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult Adddimension(string dimension_name, String creator_id)
        {
            if (dimensionApp.IsExists(dimension_name)) return Error("已存在相同条目。");
            DimensionEntity dimensionEntity = new DimensionEntity();
            dimensionEntity.dimension_name = dimension_name;
            dimensionEntity.statue = 1;

            dimensionApp.SubmitForm(dimensionEntity, string.Empty);
            return Success("操作成功。");
        }
        #endregion

        #region 启/停用系统已经维护的考核维度
        /// <summary>
        /// 启/停用系统已经维护的细项指标
        /// </summary>
        /// <param name="id">指标id 必填</param>
        /// <param name="statue">状态  0：停用，1：启用 必填</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult Changedimensionstatue(string id, int statue)
        {
            DimensionEntity dimensionEntity = new DimensionEntity();
            dimensionEntity.statue = statue;

            dimensionApp.SubmitForm(dimensionEntity, id);
            return Success("操作成功。");
        }
        #endregion

        /////////////细项///////////////////

        #region 获取系统已经维护的细项
        /// <summary>
        /// 获取系统已经维护的细项
        /// </summary>
        /// <param name="dimension_name">考核维度名称 模糊搜索</param>
        /// <param name="pagination">分页</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult GetdimensionDetail(string detail_name, int? statue, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            var data = dimensionDetailApp.GetDimensionDetailOverview(pg, detail_name, statue);
            //var data = dimensionDetailApp.GetItemList(detail_name, pg);
            return Success("操作成功。", data, pg.records);

        }
        #endregion

        #region 新增细项以及指标公式
        /// <summary>
        /// 新增考核维度
        /// </summary>
        /// <param name="dimension_name">考核维度名称</param>
        /// <param name="creator_id">当前操作者id</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult AddDimensionDetail(string detail_name, string formule,string method_id)
        {
            //添加公式之前要对公式中的指标进行验证
            if (!String.IsNullOrEmpty(formule)) {
                List<string> indicatorsDefineList = new List<string>();
                
                if (formule.Contains("^") && formule.Contains("$"))//如果有需要整体公式作为一个指标的
                {
                    indicatorsDefineList.Add(formule.Substring(formule.IndexOf("^"), formule.IndexOf("$") + 1));
                    formule = formule.Substring(formule.IndexOf("$") + 1);
                }
                if (formule.Contains("[") && formule.Contains("]"))//如果有考核办法需要外部附加数据的
                {
                    formule = formule.Replace('[', '|').Replace(']', '|');
                }
                string[] arr = formule.Replace('(', '|').Replace(')', '|').Replace('+', '|').Replace('-', '|').Replace('*', '|').Replace('/', '|').Replace('Σ', '|').Split('|');
                for (int j = 0; j < arr.Length; j++)
                {
                    if (!string.IsNullOrEmpty(arr[j])) {
                        //根据指标查找数据库中是否存在
                        if (!indicatorsDefineApp.IsExists(arr[j])) return Error("不存在指标："+ arr[j]);
                    }
                }
            }

            if (dimensionDetailApp.IsExists(detail_name)) return Error("已存在相同条目。");
            DimensionDetailEntity dimensioDetailnEntity = new DimensionDetailEntity();
            dimensioDetailnEntity.detail_name = detail_name;
            dimensioDetailnEntity.formule = formule;
            dimensioDetailnEntity.method_id = method_id;
            dimensioDetailnEntity.statue = 1;

            dimensionDetailApp.SubmitForm(dimensioDetailnEntity, string.Empty);
            return Success("操作成功。");
        }
        #endregion


        #region 启/停用系统已经维护的细项
        /// <summary>
        /// 启/停用系统已经维护的细项
        /// </summary>
        /// <param name="id">指标id 必填</param>
        /// <param name="statue">状态  0：停用，1：启用 必填</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult ChangeDimensionDetailStatue(string id, int statue)
        {
            DimensionDetailEntity dimensionDetailEntity = new DimensionDetailEntity();
            dimensionDetailEntity.statue = statue;

            dimensionDetailApp.SubmitForm(dimensionDetailEntity, id);
            return Success("操作成功。");
        }
        #endregion
    }
}