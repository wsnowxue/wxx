import MD5 from 'js-md5';
let Base64 = require('js-base64').Base64;
export default {
    data() {
        return {
            user:{
                userName:'',
                password:''
            },
            rules:{
                userName:[{required:true,message:' ',trigger:'blur'}],                
                password:[{required:true,message:' ',trigger:'blur'}],                
            },
            rememberMe:'',
            loadding:false,
            disabled:false
        };
    },
    methods: {
        getData(uesr){
            console.log(this.user);
            this.$refs[uesr].validate((valid) => {
                if (valid) {
                    // alert('submit!');
                    this.logIn()
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        logIn(){
            this.disabled = true
            // let params = Object.assign({},this.user)
            if(this.rememberMe){
                // 保存密码到sessionStorage
                sessionStorage.setItem('userMessage',Base64.encode(JSON.stringify(this.user)))
            }else{
                sessionStorage.setItem('userMessage',Base64.encode(JSON.stringify({})))
            }
            let params = {
                username:this.user.userName,
                password:MD5(this.user.password),
                code:''
            }
            this.loadding = true;
            this.$http.post('/Login/CheckLogin',params).then((result) => {
                console.log(result);
                // 保存用户信息到sessionStorage
                this.disabled = false
                if(result.state == 1){
                    this.$store.commit('saveUesrData',result.data)
                    this.$store.commit('resetRouterName')
                    sessionStorage.setItem('userData',Base64.encode(JSON.stringify(result.data)))
                    document.title = '首页'
                    this.$router.push({name:'/dashboard'})
                }
                this.loadding = false
            }).catch((err)=>{
                console.log(err);
                this.loadding = false
            })
        }
    },
    created(){
        // this.$router.push({name:'/dashboard'})
        document.title = '绩效考核-登录';
        let userMessage = sessionStorage.getItem('userMessage') 
        if(userMessage){
            this.user = JSON.parse(Base64.decode(userMessage))
            if(this.user.userName){
                this.rememberMe = true
            }
        }
    }
}