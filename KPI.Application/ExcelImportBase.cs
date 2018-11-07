using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using KPI.Code;
using System.IO;

namespace KPI.Application
{
    public abstract class ExcelImportBase
    {
        public List<ErrorItem> Scan(string filePath, string[] keyValue)
        {
            List<ErrorItem> list = new List<ErrorItem>();
            //检查表头
            List<String> headerList = NPOIExcel.ExcelHeaderToList(filePath);
            //if (headerList == null) throw new Exception("数据模板错误");
            if (!CheckHeader(headerList, list, keyValue))
            {
                File.Delete(filePath);
                return list;
                
            }
            //检查数据
            DataTable dt = NPOIExcel.ExcelToTable(filePath);
            if (dt == null || dt.Rows.Count == 0)
            {
                //删除excel
                File.Delete(filePath);
                throw new Exception("Excel数据为空！");
            }
            
            return CheckData(dt, list, keyValue);
        }

        public void Delete(string filePath, int rowNo, string keyValue)
        {
            NPOIExcel.DeleteRow(filePath, rowNo);
            //return Scan(filePath, keyValue);
        }
        private List<ErrorItem> CheckData(DataTable dt, List<ErrorItem> ErrorList, string[] keyValue)
        {
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];
                ErrorItem item = new ErrorItem();
                item.RowNo = i;
                if (!CheckDataRow(dr, item, keyValue))
                {
                    ErrorList.Add(item);
                }
                else
                {
                    if (!CheckDataTable(dt, dr, item))
                        ErrorList.Add(item);
                }
            }
            return ErrorList;
        }

        protected abstract bool CheckDataTable(DataTable dt, DataRow dr, ErrorItem item);
        protected abstract bool CheckDataRow(DataRow dr, ErrorItem item, string[] keyValue);
        protected abstract bool CheckHeader(List<String> headerList, List<ErrorItem> ErrorList, string[] keyValue);
        public bool Import(string filePath, params string[] arr)
        {
            DataTable dt = NPOIExcel.ExcelToTable(filePath);
            if (dt == null) throw new Exception("数据导入异常");
            if (SaveData(dt, arr))
            {
                //删除excel
                File.Delete(filePath);
                return true;
            }
            else
                return false;
        }
        public bool ImportEx(string filePath, params string[] arr)
        {
            DataTable dt = NPOIExcel.ExcelToTable(filePath);
            if (dt == null) throw new Exception("数据导入异常");
            if (SaveData(dt, arr))
            {
                //删除excel
                //File.Delete(filePath);
                return true;
            }
            else
                return false;
        }

        protected abstract bool SaveData(DataTable dt, params string[] arr);
        
        public class ErrorItem
        {
            public int RowNo { get; set; }
            public string ErrorReson { get; set; }
        }
    }
}
