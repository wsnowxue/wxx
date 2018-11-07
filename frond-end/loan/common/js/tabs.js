var comn, e, getWidth, menuItemClick, tabActive, tabClose, tabCloseAll, tabCloseOther, tabLeft, tabRefresh, tabRight, tabSwitch, toUrl, urlArr = [];

getWidth = function(t) {
  var outWidth;
  outWidth = 0;
  $(t).each(function() {
    return outWidth += $(this).outerWidth(true);
  });
  return outWidth;
};

e = function(e) {
  var n, nextAll, o, preAll, r, s;
  preAll = getWidth($(e).prevAll());
  nextAll = getWidth($(e).nextAll());
  n = getWidth($(".content-tabs").children().not(".J_menuTabs"));
  s = $(".content-tabs").outerWidth(true) - n;
  r = 0;
  if ($(".page-tabs-content").outerWidth() < s) {
    r = 0;
  } else if (nextAll <= s - $(e).outerWidth(true) - $(e).next().outerWidth(true)) {
    if (s - $(e).next().outerWidth(true) > nextAll) {
      r = preAll;
      o = e;
      while (r - $(o).outerWidth() > $('.page-tabs-content').outerWidth() - s) {
        r -= $(o).prev().outerWidth();
      }
      o = $(o).prev();
    }
  } else {
    preAll > s - $(e).outerWidth(true) - $(e).prev().outerWidth(true) && (r = preAll - $(e).prev().outerWidth(true));
  }
  return $(".page-tabs-content").animate({
    marginLeft: 0 - r + "px"
  }, "fast");
};

menuItemClick = function() {
  var add, arr, href, iframe, index, mask, menuTab, param, title, url;
  
  href = $(this).attr("href");
 
  if(href === "/"){	  
	  toastr.info('未配置子菜单');
	  return false;
  }
  
  
  if (href.indexOf("?") > -1) {
    arr = href.split("?");
    url = arr[0];
    param = arr[1];
  } else {
    url = href;
  }
  index = $(this).data("index");
  title = $.trim($(this).text() || this.title);
  add = true;
  if (url === 0 || $.trim(url).length === 0) {
    return false;
  }
  $(".J_menuTab").each(function() {
    if ($(this).data("id") === url) {
      $(this).hasClass("active") || $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
      e(this);
      $(".J_mainContent .J_iframe").each(function() {
        if ($(this).data("id") === url) {
          return $(this).show().siblings(".J_iframe").hide();
        }
      });
      add = false;
      if ((param != null ? param.length : void 0) > 0) {
        return tabRefresh({
          src: href,
          url: url
        });
      }
    }
  });
  if (add) {
    menuTab = "<a href = 'javascript:;' class = 'active J_menuTab' data-id= '" + url + "'> " + title + " <i class = 'fa fa-times-circle'></i> </a>";
    $(".J_menuTab").removeClass("active");
    iframe = "<iframe class = 'J_iframe' name='iframe_" + index + "' width='100%' height='100%' src='" + url + "?" + (param ? param + "&" : "") + "t=" + (new Date().getTime()) + "' frameborder='0' data-id='" + url + "' seamless></iframe>";
    $(".J_mainContent").find("iframe.J_iframe").hide().parents(".J_mainContent").append(iframe);
    mask = layer.load();
    $(".J_mainContent iframe:visible").load(function() {
      return layer.close(mask);
    });
    $(".J_menuTabs .page-tabs-content").append(menuTab);
    e($(".J_menuTab.active"));
	//urlArr.push(url);
  }
  return false;
};

function closeTab() { 
	tabClose.call($(".J_menuTabs .active.J_menuTab i")); 
	tabRefresh.call($(".J_menuTabs .active.J_menuTab"), {url: ''});
	//tabRefresh({url: urlArr[urlArr.length - 2]});
}

function closeCurrentTab() {
    tabClose.call($(".J_menuTabs .active.J_menuTab i"));
}

tabClose = function() {
  var _curMenuTab, firstUrl, lastUrl, marginLeft, url, width;
  _curMenuTab = $(this).parents(".J_menuTab");
  url = _curMenuTab.data("id");
  width = _curMenuTab.width();
  if (_curMenuTab.hasClass("active")) {
    if (_curMenuTab.next(".J_menuTab").size()) {
      firstUrl = _curMenuTab.next(".J_menuTab:eq(0)").addClass("active").data("id");
      $(".J_mainContent .J_iframe").each(function() {
        if ($(this).data("id") === firstUrl) {
          return $(this).show().siblings(".J_iframe").hide();
        }
      });
      marginLeft = parseInt($(".page-tabs-content").css("margin-left"));
      if (marginLeft < 0) {
        $(".page-tabs-content").animate({
          marginLeft: marginLeft + width + "px"
        }, 'fast');
      }
      _curMenuTab.remove();
      $(".J_mainContent .J_iframe").each(function() {
        if ($(this).data("id") === url) {
          return $(this).remove();
        }
      });
    }
    if (_curMenuTab.prev(".J_menuTab").size()) {
      lastUrl = _curMenuTab.prev(".J_menuTab:last").addClass("active").data("id");
      $(".J_mainContent .J_iframe").each(function() {
        if ($(this).data("id") === lastUrl) {
          return $(this).show().siblings(".J_iframe").hide();
        }
      });
      _curMenuTab.remove();
      $(".J_mainContent .J_iframe").each(function() {
        if ($(this).data("id") === url) {
          return $(this).remove();
        }
      });
    }
  } else {
    _curMenuTab.remove();
  }
  $(".J_mainContent .J_iframe").each(function() {
    if ($(this).data("id") === url) {
      return $(this).remove();
    }
  });
  e($(".J_menuTab.active"));
};

tabCloseOther = function() {
  $(".page-tabs-content").children("[data-id]").not(":first").not(".active").each(function() {
    $(".J_iframe[data-id='" + ($(this).data("id")) + "']").remove();
    return $(this).remove();
  });
  return $(".page-tabs-content").css("margin-left", "0");
};

tabActive = function() {
  return e($(".J_menuTab.active"));
};

tabSwitch = function() {
  var url;
  if (!$(this).hasClass("active")) {
    url = $(this).data("id");
    $(".J_mainContent .J_iframe").each(function() {
      if ($(this).data("id") === url) {
        return $(this).show().siblings(".J_iframe").hide();
      }
    });
    $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
    return e(this);
  }
};

tabRefresh = function(o) {
  var $iframe, mask, src;
  $iframe = $(".J_iframe[data-id='" + (o.url || $(this).data("id")) + "']");
  src = $iframe.attr("src");
  nsrc = o.src || src;
  mask = layer.load();
  return $iframe.attr("src", nsrc.replace(/t=(14\d{8,16})/g, "t=" + new Date().getTime())).load(function() {
    return layer.close(mask);
  });
};

tabLeft = function() {
  var blank, firstMenuTab, marginLeft, menuTabs, n, r, sencodMenuTab;
  marginLeft = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
  blank = getWidth($(".content-tabs").children().not(".J_menuTabs"));
  menuTabs = $(".content-tabs").outerWidth(true) - blank;
  n = 0;
  if ($(".page-tabs-content").width() < menuTabs) {
    return false;
  }
  firstMenuTab = $('.J_menuTab:first');
  r = 0;
  while (r + firstMenuTab.outerWidth(true) <= marginLeft) {
    r += firstMenuTab.outerWidth(true);
  }
  sencodMenuTab = firstMenuTab.next();
  r = 0;
  if (getWidth(sencodMenuTab.prevAll()) > menuTabs) {
    while (r + sencodMenuTab.outerWidth(true) < menuTabs && sencodMenuTab.length > 0) {
      r += sencodMenuTab.outerWidth(true);
    }
    n = getWidth(firstMenuTab.prevAll());
  }
  return $(".page-tabs-content").animate({
    marginLeft: 0 - n + "px"
  }, "fast");
};

tabRight = function() {
  var blank, firstMenuTab, marginLeft, menuTabs, n, r, sencodMenuTab, threeMenuTab;
  marginLeft = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
  blank = getWidth($(".content-tabs").children().not(".J_menuTabs"));
  menuTabs = $(".content-tabs").outerWidth(true) - blank;
  n = 0;
  if ($(".page-tabs-content").width() < menuTabs) {
    return false;
  }
  firstMenuTab = $('.J_menuTab:first');
  r = 0;
  while (r + firstMenuTab.outerWidth(true) <= marginLeft) {
    r += firstMenuTab.outerWidth(true);
  }
  sencodMenuTab = firstMenuTab.next();
  r = 0;
  while (r + sencodMenuTab.outerWidth(true) < menuTabs && sencodMenuTab.length > 0) {
    r += sencodMenuTab.outerWidth(true);
  }
  threeMenuTab = sencodMenuTab.next();
  n = getWidth(threeMenuTab.prevAll());
  return n > 0 && $(".page-tabs-content").animate({
    marginLeft: 0 - n + "px"
  }, "fast");
};

tabCloseAll = function() {
  $(".page-tabs-content").children("[data-id]").not(":first").each(function() {
    $(".J_iframe[data-id='" + ($(this).data("id")) + "']").remove();
    return $(this).remove();
  });
  $(".page-tabs-content").children("[data-id]:first").each(function() {
    $(".J_iframe[data-id='" + ($(this).data("id")) + "']").show();
    return $(this).addClass("active");
  });
  return $(".page-tabs-content").css("margin-left", "0");
};

toUrl = function(o) {
  return $(".J_mainContent .J_iframe").each(function() {
    var mask;
    if (!$(this).is(":hidden")) {
      mask = layer.load();
      return $(this).attr("src", o.url.replace(/t=(14\d{8,16})/g, "t=" + new Date().getTime())).load(function() {
        return layer.close(mask);
      });
    }
  });
};

$.fn.nameValues = function() {
  var arg;
  arg = arguments[0];
  return $(this).find("[data-name]").each(function(index, item) {
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
};

comn = {
  ajax: function(o) {
    var mask;
    mask = layer.load();
    if (o.url) {
      return $.ajax({
        url: o.url,
        type: o.type || "POST",
        dataType: "json",
        async: o.async || true,
        data: o.data || {},
        complete: function(jqXHR, textStatus) {
          var data;
          data = JSON.parse(jqXHR.responseText);
          if (data.code === 10000) {
            if (typeof o.success === "function") {
              o.success(data);
            }
          } else if (data.code === 20000) {
            comn.tip({
              content: data.message || "<code>" + o.url + "</code><br /> 接口异常！！！"
            });
          }
          return layer.close(mask);
        },
        error: function(jqXHR, textStatus, errorThrown) {
          return typeof o.error === "function" ? o.error(textStatus) : void 0;
        }
      });
    }
  },
  tip: function(o) {
    return toastr[o.status || "info"](o.content || "", "消息提示");
    //$("#dialogTip").nameValues(o);
    //return $("#dialogTip").modal("show");
  }
};

$(function() {
  toastr.options = {
    "closeButton": false,
    "preventDuplicates": true,
    "debug": false,
    "positionClass": "toast-top-center",
    "showDuration": "300",
    "hideDuration": "800",
    "timeOut": "5000",
    "extendedTimeOut": "800",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
  };
  $("body").on("click", ".J_menuItem", menuItemClick);
  $(".J_menuTabs").on("click", ".J_menuTab i", tabClose);
  $(".J_tabCloseOther").on("click", tabCloseOther);
  $(".J_tabShowActive").on("click", tabActive);
  $(".J_menuTabs").on("click", ".J_menuTab", tabSwitch);
  $(".J_menuTabs").on("dblclick", ".J_menuTab", tabRefresh);
  $(".J_tabLeft").on("click", tabLeft);
  $(".J_tabRight").on("click", tabRight);
  return $(".J_tabCloseAll").on("click", tabCloseAll);
});
