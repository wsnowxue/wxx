using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;

namespace KPI.Code
{
    public static class LuaHelper
    {
        /// <summary>
        /// 执行LUA脚本方法并取得返回结果
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="funcName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Object[] DoFile(string fileName, string funcName, params object[] param)
        {
            string basepath = AppDomain.CurrentDomain.BaseDirectory;
            Lua luavm = new Lua();
            luavm.DoFile(basepath + fileName);
            LuaFunction Func = luavm.GetFunction(funcName);
            object[] obj = Func.Call(param);
            luavm.Dispose();
            return obj;
        }

        /// <summary>
        /// 执行LUA脚本方法并取得返回结果
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="funcName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Object[] DoFile(string fileName, Dictionary<string, object> paramsDic)
        {
            object[] result;
            string basepath = AppDomain.CurrentDomain.BaseDirectory + @"bin\";
            using (Lua luavm = new Lua())
            {
                if (paramsDic != null)
                    foreach (var item in paramsDic)
                        luavm[item.Key] = item.Value;
                result = luavm.DoFile(basepath + fileName + ".lua");
                luavm.Dispose();
            }
            return result;
        }
    }

}