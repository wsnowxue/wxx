(function( factory ) {
	if ( typeof define === "function" && define.amd ) {
		define( ["jquery", "../jquery.validate"], factory );
	} else {
		factory( jQuery );
	}
}(function( $ ) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: ZH (Chinese, 中文 (Zhōngwén), 汉语, 漢語)
 */
var e = "<i class='glyphicon glyphicon-remove-sign'></i>&nbsp;";
$.extend($.validator.messages, {
	required: e + "这是必填字段",
	remote: e + "请修正此字段",
	email: e + "请输入有效的电子邮件地址",
	mobile: e + "请输入正确的手机号码",
	idCard: e + "请输入正确的身份证号",
	phone: e + "电话号码格式错误",
	money: e + "请输入正确的金额",
	isbankno: e + "请输入正确的银行卡号",
	url: e + "请输入有效的网址",
	date: e + "请输入有效的日期",
	dateISO: e + "请输入有效的日期 (YYYY-MM-DD)",
	number: e + "请输入有效的数字",
	digits: e + "只能输入数字",
	creditcard: e + "请输入有效的信用卡号码",
	equalTo: e + "你两次的输入不相同",
	extension: e + "请输入有效的后缀",
	maxlength: e + $.validator.format("最多可以输入 {0} 个字符"),
	minlength: e + $.validator.format("最少要输入 {0} 个字符"),
	rangelength: e + $.validator.format("请输入长度在 {0} 到 {1} 之间的字符串"),
	range: e + $.validator.format("请输入范围在 {0} 到 {1} 之间的数值"),
	max: e + $.validator.format("请输入不大于 {0} 的数值"),
	min: e + $.validator.format("请输入不小于 {0} 的数值"),
	integer: e + "请输入整数"
});

}));

