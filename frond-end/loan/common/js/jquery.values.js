!function (t) {
    t.fn.values = function (e) {
        var i = t(this).find(":input").get();
        return "object" != typeof e ? (e = {}, t.each(i, function () {
            this.name && !this.disabled && (this.checked || /select|textarea/i.test(this.nodeName) || /text|hidden|password/i.test(this.type)) && (e[this.name] = t(this).val())
        }), e) : (t.each(i, function () {
            this.name && e[this.name] + "" && "undefined" != typeof e[this.name] && ("checkbox" == this.type || "radio" == this.type ? t(this).prop("checked", e[this.name] == t(this).val()) : t(this).val(e[this.name]))
        }), t(this))
    }
}(jQuery);
