import MD5 from 'js-md5';
let Base64 = require('js-base64').Base64;
export default {
    data() {
        var validatePass = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('请再次输入新密码'));
            } else if (value !== this.changePass.newpassword) {
                callback(new Error('两次输入密码不一致!'));
            } else {
                callback();
            }
        };
        return {
            changePass: {
                oldpassword: '',
                newpassword: '',
                checkPass: ''
            },
            rules: {
                oldpassword: [
                    { required: true, message: '请输入原密码', trigger: 'blur' }
                ],
                newpassword: [
                    { required: true, message: '请输入新密码', trigger: 'blur' },
                    { min: 6, max: 16, message: '长度在 6 到 16 个字符', trigger: 'blur' }
                ],
                checkPass: [
                    { required: true, validator: validatePass, trigger: 'blur' }
                ],
            }
        };
    },
    methods: {
        modifyPassword(formName){
            this.$refs[formName].validate((valid) => { 
                if (valid) {
                    let params = {
                        username: JSON.parse(Base64.decode(sessionStorage.getItem('userData'))).UserCode,
                        oldpassword: MD5(this.changePass.oldpassword),
                        newpassword: this.changePass.newpassword
                    }
                    this.$http.post('/User/ChangePwd',params).then((res)=>{
                        console.log(res);
                        if(res.state == 1){
                            this.$message({
                                showClose: true,
                                message: res.message,
                                type: 'success'
                            });
                        }
                    })
                } else {
                    return false;
                }
            });
        },
    },
    activated(){
        let obj = {
            name:"/modifyPassword",
            title:"修改密码",
            url:"/modifyPassword"
        }
        this.$store.commit('setRouterName',obj)
        // console.log(this.$store.state.navTabs);
    }
}