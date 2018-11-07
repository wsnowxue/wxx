var comn, queryParams, ref, tableData, tip;
comn = {};

tip = null;

function isArray(o) {
    return Object.prototype.toString.call(o) == '[object Array]';
}

function initDropdown(o) {
    var selectObj = $("#" + o.id);
    comn.ajax({
        url: "cache/getDropDown",
        data: {"type": o.type},
        success: function (res) {
            var data = res.data;
            if (o.noSelect) {
                selectObj.append("<option value=''>--请选择--</option>");
            }
            data = JSON.parse(data);
            for (var key in data) {
                selectObj.append("<option value='" + key + "'>" + data[key] + "</option>");
            }
        }
    });
}

function initDropdownDataAll(data) {
    var x = '';
    comn.ajax({
        url: "cache/getDropDownAll",
        data: {"type": JSON.stringify(data)},
        async: false,
        success: function (res) {
            x = res.data;
        }
    });
    return $.parseJSON(x);
}


(function () {
    tip = function (o) {
        var base;
        return typeof (base = window.parent.comn).tip === "function" ? base.tip(o) : void 0;
    };
    return comn = {
        user: window.parent.user,
        cache: window.parent.cache,
        table: {
            "undefinedText": "--",
            "classes": "table-striped table-hover table",
            "pagination": true,
            "sidePagination": "server",
            "queryParams": "queryParams",
            "paginationFirstText": "第一页",
            "paginationPreText": "上一页",
            "paginationNextText": "下一页",
            "paginationLastText": "最后一页",
            "clickToSelect": true,
            "height": "500"
        },
        toUrl: function (o) {
            var base;
            if (o.url.indexOf(".html") > -1) {
                return typeof (base = window.parent).toUrl === "function" ? base.toUrl(o.url) : void 0;
            }
        },
        closeTab: function () {
            window.parent.closeTab();
        },
        closeCurrentTab: function () {
            window.parent.closeCurrentTab();
        },
        addTab: function (o) {
            if (o.href) {
                return window.parent.menuItemClick.call(o);
            }
        },
        accAdd: function (arg1, arg2) { //js精度问题(加法)
            var r1, r2, m;
            try {
                r1 = arg1.toString().split(".")[1].length
            } catch (e) {
                r1 = 0
            }
            try {
                r2 = arg2.toString().split(".")[1].length
            } catch (e) {
                r2 = 0
            }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        },
        accSub: function (arg1, arg2) { //js精度问题(减法)
            var r1, r2, m, n;
            try {
                r1 = arg1.toString().split(".")[1].length
            } catch (e) {
                r1 = 0
            }
            try {
                r2 = arg2.toString().split(".")[1].length
            } catch (e) {
                r2 = 0
            }
            m = Math.pow(10, Math.max(r1, r2));
            //last modify by deeka
            //动态控制精度长度
            n = (r1 >= r2) ? r1 : r2;
            return ((arg1 * m - arg2 * m) / m).toFixed(n);
        },
        accMul: function (arg1, arg2) {  //js精度问题(乘法)
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try {
                m += s1.split(".")[1].length
            } catch (e) {
            }
            try {
                m += s2.split(".")[1].length
            } catch (e) {
            }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
        },
        accDiv: function (arg1, arg2) {  //js精度问题(除法)
            var t1 = 0, t2 = 0, r1, r2;
            try {
                t1 = arg1.toString().split(".")[1].length
            } catch (e) {
            }
            try {
                t2 = arg2.toString().split(".")[1].length
            } catch (e) {
            }
            with (Math) {
                r1 = Number(arg1.toString().replace(".", ""))
                r2 = Number(arg2.toString().replace(".", ""))
                return (r1 / r2) * pow(10, t2 - t1);
            }
        },
        ajax: function (o) {
            var _this, mask;
            //console.log((o.url + " -->") + JSON.stringify(o.data));
            mask = layer.load();
            _this = this;
            if (o.url) {
                return $.ajax({
                    url: interUrl.basic + o.url,
                    type: o.type || "POST",
                    dataType: "json",
                    async: o.async,
                    data: o.data || {},
                    complete: function (jqXHR, textStatus) {
                        return layer.close(mask);
                    },
                    success: function (data) {
                        if (data.code === 20000) {
                            return tip({
                                content: data.message || "<code>" + o.url + "</code><br /> 接口异常！！！"
                            });
                        } else if (data.code === 30000) {
                            return window.parent.location.href = "../../../index.html";
                        } else {
                            if (typeof data === "string") {
                                data = JSON.parse(data);
                            }
                            return typeof o.success === "function" ? o.success(data) : void 0;
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        return typeof o.error === "function" ? o.error(textStatus) : void 0;
                    }
                });
            }
        },
        getArgs: function () {
            var args, i, item, items, name, qs, value;
            qs = (location.search.length > 0 ? location.search.substring(1) : "");
            items = (qs.length ? qs.split("&") : []);
            args = {};
            i = 0;
            while (i < items.length) {
                item = items[i].split("=");
                name = decodeURIComponent(item[0]);
                value = decodeURIComponent(item[1]);
                if (name.length) {
                    args[name] = value;
                }
                i++;
            }
            return args;
        },
        /*
         *省市区三级联动,传递参数
         *{type: "car", level: [{
         *    el: $("#carBrandID")  渲染对象
         *    key: code  选中值
         *    target: $("#id") 中文赋值对象
         *},{
         *    el: $("#carMakeID")
         *    key: code
         *    target: $("#id")
         *},{
         *    el: $("#carModelID")
         *    key: code
         *}]}
         */
        linkage: function (o) {
            var o0, o1, o2;
            if (o.type === "car") {
                o0 = o.level[0];
                o1 = o.level[1];
                o2 = o.level[2];
                if (o1.key) {
                    o1.el.getCarList(o0.key, o1.key).unbind("change").change(function () {
                        if (o1.target) {
                            o1.target.val($(this).find("option:selected").text());
                        }
                        o2.el.val("");
                        if (this.value) {
                            return o2.el.getCarModel(this.value);
                        }
                    });
                }
                if (o2.key) {
                    o2.el.getCarModel(o1.key, o2.key).unbind("change").change(function () {
                        if (o1.target) {
                            return o2.target.val($(this).find("option:selected").text());
                        }
                    });
                }
                return o0.el.getBrand(o0.key || "").unbind("change").change(function () {
                    if (o0.target) {
                        o0.target.val($(this).find("option:selected").text());
                    }
                    if (this.value) {
                        o2.el.val("");
                        return o1.el.getCarList(this.value).unbind("change").change(function () {
                            if (o1.target) {
                                o1.target.val($(this).find("option:selected").text());
                            }
                            if (this.value) {
                                return o2.el.getCarModel(this.value).unbind("change").change(function () {
                                    if (o2.target) {
                                        return o2.target.val($(this).find("option:selected").text());
                                    }
                                });
                            }
                        });
                    }
                });
            }
        },
        getDropDown: function (o) {
            if (isArray(o)) {
                $.each(o, function (i, obj) {
                    initDropdown(obj);
                });
            } else {
                initDropdown(o);
            }
        }, /*
    函数，加法函数，用来得到精确的加法结果  
    说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
    参数：arg1：第一个加数；arg2第二个加数；d要保留的小数位数（可以不传此参数，如果不传则不处理小数位数）
    调用：Calc.Add(arg1,arg2,d)  
    返回值：两数相加的结果
    */
        Add: function (arg1, arg2) {
            arg1 = arg1.toString(), arg2 = arg2.toString();
            var arg1Arr = arg1.split("."), arg2Arr = arg2.split("."), d1 = arg1Arr.length == 2 ? arg1Arr[1] : "",
                d2 = arg2Arr.length == 2 ? arg2Arr[1] : "";
            var maxLen = Math.max(d1.length, d2.length);
            var m = Math.pow(10, maxLen);
            var result = Number(((arg1 * m + arg2 * m) / m).toFixed(maxLen));
            var d = arguments[2];
            return typeof d === "number" ? Number((result).toFixed(d)) : result;
        },
        /*
        函数：减法函数，用来得到精确的减法结果
        说明：函数返回较为精确的减法结果。
        参数：arg1：第一个加数；arg2第二个加数；d要保留的小数位数（可以不传此参数，如果不传则不处理小数位数
        调用：Calc.Sub(arg1,arg2)
        返回值：两数相减的结果
        */
        Sub: function (arg1, arg2) {
            return Calc.Add(arg1, -Number(arg2), arguments[2]);
        },
        /*
        函数：乘法函数，用来得到精确的乘法结果
        说明：函数返回较为精确的乘法结果。
        参数：arg1：第一个乘数；arg2第二个乘数；d要保留的小数位数（可以不传此参数，如果不传则不处理小数位数)
        调用：Calc.Mul(arg1,arg2)
        返回值：两数相乘的结果
        */
        Mul: function (arg1, arg2) {
            var r1 = arg1.toString(), r2 = arg2.toString(), m, resultVal, d = arguments[2];
            m = (r1.split(".")[1] ? r1.split(".")[1].length : 0) + (r2.split(".")[1] ? r2.split(".")[1].length : 0);
            resultVal = Number(r1.replace(".", "")) * Number(r2.replace(".", "")) / Math.pow(10, m);
            return typeof d !== "number" ? Number(resultVal) : Number(resultVal.toFixed(parseInt(d)));
        },
        /*
        函数：除法函数，用来得到精确的除法结果
        说明：函数返回较为精确的除法结果。
        参数：arg1：除数；arg2被除数；d要保留的小数位数（可以不传此参数，如果不传则不处理小数位数)
        调用：Calc.Div(arg1,arg2)
        返回值：arg1除于arg2的结果
        */
        Div: function (arg1, arg2) {
            var r1 = arg1.toString(), r2 = arg2.toString(), m, resultVal, d = arguments[2];
            m = (r2.split(".")[1] ? r2.split(".")[1].length : 0) - (r1.split(".")[1] ? r1.split(".")[1].length : 0);
            resultVal = Number(r1.replace(".", "")) / Number(r2.replace(".", "")) * Math.pow(10, m);
            return typeof d !== "number" ? Number(resultVal) : Number(resultVal.toFixed(parseInt(d)));
        }
    };
})();

$.fn.extend({
    nameValues: function () {
        var arg;
        arg = arguments[0];
        return $(this).find("[data-name]").each(function (index, item) {
            var key, keySwitch, value;
            key = $(this).data("name");
            keySwitch = $(this).data("formatter");
            if (keySwitch) {
                value = window[keySwitch](arg[key]) || "";
            }
            if (key) {
                return $(item).html(value || arg[key] || "");
            }
        });
    },
    getProvince: function (value, callback) {
        comn.ajax({
            url: interUrl.common.getProvince,
            success: (function (_this) {
                return function (res) {
                    var o;
                    $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.areacode + "'>" + o.province + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                    if (typeof(callback) == "function") {
                        callback()
                    }
                    ;
                };
            })(this)
        });
        return this;
    },
    getCity: function (provinceCode, value) {
        if (provinceCode) {
            comn.ajax({
                url: interUrl.common.getCity,
                data: {
                    areacode: provinceCode
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.areacode + "'>" + o.city + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getArea: function (cityCode, value) {
        if (cityCode) {
            comn.ajax({
                url: interUrl.common.getArea,
                data: {
                    areacode: cityCode
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.areacode + "'>" + o.county + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getOrg: function (value) {
        comn.ajax({
            url: interUrl.common.orgList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data.module;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "").change();
                };
            })(this)
        });
        return this;
    },
//获取颜色接口
    getColors: function () {
        comn.ajax({
            url: interUrl.second.getColor,
            data: {},
            async: false,
            success: (function (_this) {
                return function (res) {
                    var j, len, o, ref, str;
                    str = "<option value=''>--请选择--</option>";
                    ref = res.data;
                    var defaultValue = $(_this).attr('defaultValue');
                    for (var i in ref) {
                        str += "<option value='" + ref[i] + "' " + (defaultValue == ref[i] ? "selected" : "") + ">" + ref[i] + "</option>";
                    }
                    return $(_this).html(str);
                };
            })(this)
        });
        return this;
    },
    getProvince: function (value, callback) {
        comn.ajax({
            url: interUrl.common.getProvince,
            success: (function (_this) {
                return function (res) {
                    var o;
                    $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.areacode + "'>" + o.province + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                    if (typeof(callback) == "function") {
                        callback()
                    }
                    ;
                };
            })(this)
        });
        return this;
    },
    //获取业务类型接口
    getBusinessTypes: function (value, callback) {
        comn.ajax({
            url: interUrl.second.getBusinessTypes,
            data: {},
            success: (function (_this) {
                /*return function(res) {
                  var j, len, o, ref, str;
                  str = "<option value=''>--请选择--</option>";
                  ref = res.data;
                  var defaultValue = $(_this).attr('defaultValue');
                  for(var i = 0;i<ref.length;i++){
                      o=ref[i];
                      str += "<option value='" + o.key + "' "+(defaultValue==o.key?"selected":"")+">" + o.value + "</option>";
                  }
                  return $(_this).html(str);
                };*/
                return function (res) {
                    var o;
                    $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.key + "'>" + o.value + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                    if (typeof(callback) == "function") {
                        callback()
                    }
                    ;
                };
            })(this)
        });
        return this;
    },
    //征信发起获取业务类型接口
    getCreditBusinessTypes: function (callback) {
        comn.ajax({
            url: interUrl.second.getBusinessTypes,
            data: {},
            success: (function (_this) {
                return function (res) {
                    var j, len, o, ref, str;
                    str = "<option value=''>--请选择--</option>";
                    ref = res.data;
                    if (ref.length == 1) {
                        var defaultValue = ref[0].key;
                        $(_this).attr("disabled", true);
                    }
                    var onlyCYR = true;
                    for (var i = 0; i < ref.length; i++) {
                        o = ref[i];
                        if (o.key != '5') {
                            onlyCYR = false;
                        }
                        str += "<option value='" + o.key + "' " + (defaultValue == o.key ? "selected" : "") + ">" + o.value + "</option>";
                    }
                    if (onlyCYR) {
                        $("#addParty").addClass("hide");
                    }
                    return $(_this).html(str);
                };
            })(this)
        });
        return this;
    },
    getRuleList: function (value) {
        comn.ajax({
            url: interUrl.common.ruleList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    /*
     1、省获取(改造)
     2、参数1: eg: { code: '选中的code值', value: '选中值的中文名称', callback: '获取数据后回调方法'}
     3、参数2: 标识查看/修改(true/false)
     */
    getProvinceC: function (c, flag) {
        if (flag) {
            $(this).html("<option value='" + c.code + "'>" + (c.value || '') + "</option>");
        } else {
            $(this).getProvince(c.code, c.callback);
        }
        return this;
    },
    getBusinessTypesC: function (c, flag) {
        if (flag) {
            $(this).html("<option value='" + c.code + "'>" + (c.value || '') + "</option>");
        } else {
            $(this).getBusinessTypes(c.code, c.callback);
        }
        return this;
    },
    /*
     1、市获取(改造)
     2、id 从上一级获取过来的ID值
     3、参数1: eg: { code: '选中的code值', value: '选中值的中文名称'}
     4、参数2: 标识查看/修改(true/false)
     */
    getCityC: function (id, c, flag) {
        if (flag) {
            $(this).html("<option value='" + c.code + "'>" + (c.value || '') + "</option>");
        } else {
            $(this).getCity(id, c.code);
        }
        return this;
    },

    /*
     1、区获取(改造)
     2、参数1: eg: { code: '选中的code值', value: '选中值的中文名称'}
     3、参数2: 标识查看/修改(true/false)
     */
    getAreaC: function (id, c, flag) {
        if (flag) {
            $(this).html("<option value='" + c.code + "'>" + (c.value || '') + "</option>");
        } else {
            $(this).getArea(id, c.code);
        }
        return this;
    },
    /*
            1、参数1: 标识查看/修改(true/false)
     */
    getBrandC: function (flag) {
        if (flag == null) {
            flag = true;
        }
        if (!flag) {
            $(this).getBrand();
        }
        return this;
    },
    /*
     1、车系获取(改造)
     2、参数1: eg: { code: '选中的code值', value: '选中值的中文名称'}
     3、参数2: 标识查看/修改(true/false)
     */
    getCarListC: function (id, c, flag) {
        if (flag) {
            $(this).html("<option value='" + c.code + "'>" + (c.value || '') + "</option>");
        } else {
            $(this).getCarList(id, c.code);
        }
        return this;
    },

    /*
     1、车型(改造)
     2、参数1: eg: { code: '选中的code值', value: '选中值的中文名称'}
     3、参数2: 标识查看/修改(true/false)
     */
    getCarModelC: function (id, c, flag) {
        if (flag) {
            $(this).html("<option value='" + c.code + "'>" + (c.value || '') + "</option>");
        } else {
            $(this).getCarModel(id, c.code);
        }
        return this;
    },
    getBrand: function () {
        var codeItem;
        if ($(this).parent().find(".select-box").length) {
            return;
        }
        codeItem = function (arr) {
            var o;
            return ((function () {
                var j, len, results;
                results = [];
                for (j = 0, len = arr.length; j < len; j++) {
                    o = arr[j];
                    results.push("<li data-code='" + o.brandid + "'>" + o.brandname + "</li>");
                }
                return results;
            })()).join("");
        };
        comn.ajax({
            url: interUrl.common.brandList,
            success: (function (_this) {
                return function (res) {
                    var $element, item, j, len, o, ref;
                    item = {};
                    $element = ["<ul class='select-box hidden'>", "<div class='select-box-list'></div>", "<div class='select-box-letter'>"];
                    ref = res.data;
                    for (j = 0, len = ref.length; j < len; j++) {
                        o = ref[j];
                        if (o.cars.length) {
                            $element.push("<a href='javascript:;'>" + o.name + "</a>");
                            item[o.name] = o.cars;
                        }
                    }
                    $element.push("</div></ul>");

                    /*
                                     *  事件绑定
                     */
                    $(_this).css("background-color", "#FFF").on("click", function () {
                        $(".select-box").addClass("hidden");
                        return $(this).next(".select-box").removeClass("hidden").scrollTop(0);
                    }).parent().append($element.join("")).on("click", ".select-box-letter a", function () {
                        var htmlCode;
                        htmlCode = codeItem(item[$(this).text()]);
                        return $(this).parents(".select-box").scrollTop(0).find(".select-box-list").html(htmlCode);
                    }).on("click", ".select-box-list li", function () {
                        return $(this).parents(".select-box").addClass("hidden").prev("input").val($(this).text()).data("code", $(this).data("code")).trigger("change");
                    }).find(".select-box-letter").each(function () {
                        return $(this).find("a").eq(0).trigger("click");
                    });
                    return $("body").on("click", function (e) {
                        var flag;
                        _this = e.target;
                        flag = !$(_this).closest(".input-tip").not(".select-box-letter").length;
                        if (flag) {
                            return $(".select-box").addClass("hidden");
                        }
                    });
                };
            })(this)
        });
        return this;
    },
    getCarList: function (code, value) {
        if (code) {
            comn.ajax({
                url: interUrl.common.carList,
                data: {
                    brandcode: code
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            results = [];
                            ref = res.data.manuInfo;
                            for (var i = 0; i < ref.length; i++) {
                                var carList = ref[i].child;
                                for (j = 0, len = carList.length; j < len; j++) {
                                    o = carList[j];
                                    results.push("<option value='" + o.brandcode + "'>" + o.brandname + "</option>");
                                }
                            }
                            return results;
                        })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getCarModel: function (code, value) {
        if (code) {
            comn.ajax({
                url: interUrl.common.carModels,
                data: {
                    brandcode: code
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                for (var o in ref[j].cars)
                                    results.push("<option value='" + ref[j].cars[o].carid + "' data-msrp='" + ref[j].cars[o].msrp + "'>" + ref[j].cars[o].carname + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "");
                    };
                })(this)
            });
        }
        return this;
    },
    getInsurance: function (value, callback) {
        comn.ajax({
            url: interUrl.common.insuranceList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.id + "'>" + o.insuranceCompanyName + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                    if (typeof(callback) == 'function') {
                        callback();
                    }
                };
            })(this)
        });
        return this;
    },
    getLoad: function (callback) {
        if (!$(this).hasClass("loaded")) {
            $(this).load($(this).data("url") + ("?t=" + (new Date().getTime())), (function (_this) {
                return function () {
                    if (typeof callback === "function") {
                        callback();
                    }
                    return $(_this).addClass("loaded");
                };
            })(this));
        }
        return this;
    },
    getBank: function (value) {
        comn.ajax({
            url: interUrl.gr.bankList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.id + "'>" + o.bankName + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getCarDealer: function (value) {
        comn.ajax({
            url: interUrl.common.loanCarList,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.id + "'>" + o.dealerName + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getCompanys: function (type, value) {
        comn.ajax({
            url: interUrl.common.branchAndGroupComo,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            if (type == "0" && o.id != 1) {
                                continue;
                            }
                            results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getCompany: function (value) {
        comn.ajax({
            url: interUrl.common.getCompany,
            success: (function (_this) {
                return function (res) {
                    var o;
                    return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                        var j, len, ref, results;
                        ref = res.data;
                        results = [];
                        for (j = 0, len = ref.length; j < len; j++) {
                            o = ref[j];
                            results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                        }
                        return results;
                    })()).join("")).val(value || "");
                };
            })(this)
        });
        return this;
    },
    getGroup: function (companyId, value) {
        if (companyId) {
            comn.ajax({
                url: interUrl.common.getGroup,
                data: {
                    companyId: companyId
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "").change();
                    };
                })(this)
            });
        }
        return this;
    },
    getDepart: function (companyId, type, value) {

        if (companyId) {
            comn.ajax({
                url: interUrl.common.getDepart,
                data: {
                    companyId: companyId,
                    type: type
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "").change();
                    };
                })(this)
            });
        }
        return this;
    },
    getBizDepart: function (bizDepartId, value) {
        if (bizDepartId) {
            comn.ajax({
                url: interUrl.common.getBizDepart,
                data: {
                    bizDepartId: bizDepartId
                },
                success: (function (_this) {
                    return function (res) {
                        var o;
                        return $(_this).html("<option value=''>--请选择--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.uid + "'>" + o.realname + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "").change();
                    };
                })(this)
            });
        }
        return this;
    },

    getGroup_comp: function (companyId, value) {
        if (companyId) {
            comn.ajax({
                url: interUrl.common.getGroup,
                data: {
                    companyId: companyId
                },
                success: (function (_this) {

                    return function (res) {
                        var o;
                        return $(_this).html("<option value='-2'>--所有业务组--</option>" + ((function () {
                            var j, len, ref, results;
                            ref = res.data;
                            results = [];
                            for (j = 0, len = ref.length; j < len; j++) {
                                o = ref[j];
                                results.push("<option value='" + o.id + "'>" + o.name + "</option>");
                            }
                            return results;
                        })()).join("")).val(value || "").change();
                    };
                })(this)
            });
        }
        return this;
    },
    flexBtnInit: function (value) {//flexBtn初始化，value取值0或1，0代表展开状态，1代表折叠状态
        value = value ? value : 0;
        var pbody = $(this).parents('.collapseFlex').find('.panel-body');
        $(this).off('click').click(function () {
            var status = $(this).attr('data-status');
            if (status == '0') {
                pbody.slideUp();
                $(this).attr('data-status', 1).css('transform', 'rotate(0)');
            } else {
                pbody.slideDown();
                $(this).attr('data-status', 0).css('transform', 'rotate(90deg)');
            }
        });
        if (value == 0) {
            pbody.show();
            $(this).css('transform', 'rotate(90deg)');
        } else {
            pbody.hide();
            $(this).css('transform', 'rotate(0)');
        }
    }
});

$(function () {
    $(window).resize(function () {
        var base;
        return typeof (base = $("table")).bootstrapTable === "function" ? base.bootstrapTable('resetView') : void 0;
    });
    $("body").on("click", "a", function (e) {
        var ref;
        if (((ref = $(this).href) != null ? ref.index(".html") : void 0) > -1) {
            e.preventDefault();
            return comn.toUrl({
                "url": $(this).href
            });
        }
    }).on("focus", ".date", function () {
        //if(!$(this).is(":disabled")){ $(this).attr("readonly", true).css("background-color", "#FFF"); }
        var base;
        return typeof (base = $(this)).datetimepicker === "function" ? base.datetimepicker({
            format: "yyyy-mm-dd",
            pickerPosition: "bottom-right",
            language: "zh-CN",
            minView: 2,
            todayHighlight: true,
            autoclose: true,
            todayBtn: true,
            show: true
        }) : void 0;
    }).on("show.bs.tab", "[data-toggle='tab']", function (e) {
        return $($(this).attr("href")).find("[data-url]").each(function () {
            $(this).getLoad();
        });
    }).on("click", ".btn[modal='reset']", function () {
        var ref;
        return (ref = $(this).parents("form")[0]) != null ? ref.reset() : void 0;
    }).on("keyup", ".number, .mobile", function () {
        //if(!/^\d*(?:\.\d{0,2})?$/.test(this.value)){
        //this.value = '';
        //}
        //this.value = this.value.replace(/\D+.]/g,'');
    });
    $(".modal").on("show.bs.modal", function () {
        if ($(this).find("form").length) {
            return $(this).find("form")[0].reset();
        }
    });
    return $("#btn-search").click(function () {
        return $("#table").bootstrapTable('refresh', {url: "..."});
    });
});

if (typeof Mock !== "undefined" && Mock !== null) {
    Mock.mock(/list.json/, {
        'totalItem': 500,
        'data|40': [
            {
                'id': '@INT(1000, 60000)',
                'customerId': '@INT(1000, 60000)',
                'cardNo': '@INT(1000000000000000, 6000000000000000)',
                'loanAmount': '@INT(1000, 60000)',
                'loanTerm|1': [1, 2, 3],
                'customerName': '@CHINESENAME',
                'cardId': '@INT(1,100)',
                'projcetName|1': ['车贷项目申请', '某某项目申请'],
                'mobile': '@INT(600000)',
                'proceing|1': ['调度岗', '集团估计师', '录入内勤', '审核内勤'],
                'handleP': '@NAME',
                'proced|1': ['银行征信', '公安征信', '签单分配', '签单调查'],
                'orgname|1': ['杭州分公司', '湖北分公司'],
                'faqiren': '@CHINESENAME',
                'dbe': '@FLOAT(1,2)',
                'modifyTime': Random.datetime('yyyy-MM-dd A HH:mm:ss')
            }
        ]
    });
}

if ((ref = $.validator) != null) {
    ref.setDefaults({
        highlight: function (e) {
            return $(e).closest(".input-tip").removeClass("has-success").addClass("has-error");
        },
        success: function (e, r) {
            return $(r).closest(".input-tip").removeClass("has-error").addClass("has-success");
        },
        errorPlacement: function (e, r) {
            if (r.parent('.input-group').length) {
                e.insertAfter(r.parent());
            } else {
                if (e.text()) {
                    return e.appendTo((r.is(":radio") || r.is(":checkbox") ? r.parent().parent().parent() : r.parent()));
                }
            }
        }
    });
}

tableData = function (params, data, url, callback) {
    var p;
    p = params.data;
    if (url) {
        return comn.ajax({
            url: url,
            data: $.extend(data, p),
            success: function (res) {
                params.success({
                    'total': res.totalItem,
                    'rows': res.data
                });
                params.complete();
                return typeof callback === "function" ? callback(res) : void 0;
            }
        });
    }
};

queryParams = function (params) {
    return {
        search: params.search,
        page: (params.limit + params.offset) / params.limit,
        pageSize: params.limit
    };
};
$.fn.extend({
    initCacheDropDown: function (o) {
        var dropdown_type = [];
        var dropdown_nd = {};
        var dropdown_nd_v = {};//如果有不选择的情况的默认值
        var dropdown_exclu = [];//排除某一些选择
        var dropdownele = {};
        $(this).each(function () {
            dropdown_type.push($(this).attr("data-type"));//获取redis的key
            dropdown_nd[$(this).attr("data-type")] = $(this).attr("data-nd");
            dropdownele[$(this).attr("data-type")] = $(this);
            dropdown_nd_v[$(this).attr("data-type")] = $(this).attr("data-nd-val") || "";//如果有不选择的情况的默认值
            dropdown_exclu.push($(this).attr("data-exclu") || " ");//排除某一些选择
        });
        comn.ajax({
            url: "cache/getDropDown",
            data: {"type": dropdown_type.join("|"), "exclu": dropdown_exclu.join("|")},
            success: (function (_this) {
                return function (res) {
                    var data = JSON.parse(res.data);
                    // console.log(data);
                    for (var key in data) {
                        results = [];
                        dropjson = data[key];
                        if (!dropdown_nd[key]) {
                            results.push("<option value='" + dropdown_nd_v[key] + "'>--请选择--</option>");
                        }
                        for (var k in dropjson) {
                            results.push("<option value='" + k + "'>" + dropjson[k] + "</option>");
                        }
                        dropdownele[key].html(results.join(""));
                    }
                    //console.log(typeof o);
                    o();
                };
            })(this)
        });
    }
});

/**
 *
 * 获取当前时间
 */
function getNowTime() {
    function p(s) {
        return s < 10 ? '0' + s : s;
    }

    var myDate = new Date();
    //获取当前年
    var year = myDate.getFullYear();
    //获取当前月
    var month = myDate.getMonth() + 1;
    //获取当前日
    var date = myDate.getDate();
    var h = myDate.getHours();       //获取当前小时数(0-23)
    var m = myDate.getMinutes();     //获取当前分钟数(0-59)
    var s = myDate.getSeconds();

    var now = year + '-' + p(month) + "-" + p(date)
    /*+" "+p(h)+':'+p(m)+":"+p(s);*/
    return now;
}

var readDpData = function (dropdown_type, v) {
    if (comn[dropdown_type]) {
        dict = comn[dropdown_type];
        if (dict) {
            return dict[v];
        }
    }
    return "";
};
var initDpData = function (dropdown_type) {
    if (comn[dropdown_type]) {
        return;
    }
    comn.ajax({
        url: "cache/getDropDown",
        data: {"type": dropdown_type},
        success: (function (_this) {
            return function (res) {
                var data = JSON.parse(res.data);
                comn[dropdown_type] = data[dropdown_type];
            };
        })(this)
    });
};
$(function () {
    $(".rcDropdownInit").each(function () {
        $(this).initCacheDropDown(function () {
        });
    })
});

$('#area').change(function () {
    $('#position').val('');
});

$('#area_1').change(function () {
    $('#homePosition').val('');
});

$('#area_2').change(function () {
    $('#companyPosition').val('');
});

$('#area_3').change(function () {
    $('#spouseCompanyPosition').val('');
});