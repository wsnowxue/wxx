<template>
  <el-container class="clear page-content">
    <!-- 导航 -->
    <el-aside class="page-aside" width="220px">
        <div class="border-right main-logo text-center">
            <img src="@/assets/cheguo-logo.png" alt="">
        </div>
        <nav-menu ></nav-menu>
    </el-aside>
    <el-container class="container-kpi">
        <!-- header -->        
        <el-header>
            <div class="headers clear">
                <h1 class="fl">绩效考核管理系统</h1>
                <div class="fr">
                    <ul class="header-person">
                        <li><span>你好，{{name}}</span></li>
                        <li @click="modifyPassword()">
                            <!-- <i class="iconfont icon-xiugaimima"></i><router-link :to="{path:'/modifyPassword'}">修改密码</router-link> -->
                            <i class="iconfont icon-xiugaimima"></i>修改密码
                        </li>
                        <li @click="openOutLogin" style="cursor:pointer"><i class="iconfont icon-kaiguan"></i>退出</li>
                    </ul>
                </div>
            </div>
        </el-header>
        <!-- tabs -->
        <nav-tabs></nav-tabs>
        <!-- 路由 -->
        <el-main>
            <keep-alive>
                <router-view :key="key"/>
            </keep-alive>
        </el-main>
    </el-container>
  </el-container>
</template>

<script>
import header from '../components/header/header.vue'
import navMenu from '../components/nav-menu/nav-menu.vue'
import navTabs from '../components/nav-tabs/nav-tabs.vue'
import { setTimeout } from 'timers';
export default {
    components: {
        "my-header": header,
        "nav-menu":navMenu,
        "nav-tabs":navTabs
    },
    data(){
        return {
            // name:''
        }
    },
    methods: {
        openOutLogin() {
            this.$confirm('退出后需要重新登录, 是否继续?', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
            }).then(() => {
                let loading = this.$loading({
                    lock: true,
                    text: '拼命加载中',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                })
                this.$http.get('/Login/OutLogin').then((result) => {
                    if(result.state == 1){
                        loading.close()
                        this.$message({
                            type: 'success',
                            message: '退出成功!'
                        });
                        setTimeout(() => {
                            window.location = result.data 
                        }, 1000);
                        // this.$router.push({name:'/login'})
                    }
                }).catch((err)=>{
                    this.$message({
                        type: 'success',
                        message: '退出失败!'
                    });
                })
            }).catch(() => {
            this.$message({
                type: 'info',
                message: '已取消退出'
            });          
            });
        },
        // 统一登录
        unifiedLogin(){
            this.$http.get('/Login/GetCurrent').then((result) => {
                console.log(result);
                // 保存用户信息到sessionStorage
                this.disabled = false
                if(result.state == 1 && result.data){
                    this.$store.commit('saveUesrData',result.data)
                    this.$store.commit('resetRouterName')
                    sessionStorage.setItem('userData',Base64.encode(JSON.stringify(result.data)))
                    document.title = '首页'
                    let data = sessionStorage.getItem('userData')
                    let userData = JSON.parse(Base64.decode(data)) 
                    this.$store.commit('saveUesrData',userData)
                    this.name = userData.UserName
                    this.$router.push({name:'/dashboard'})
                }
            }).catch((err)=>{
                console.log(err);
            })
        },
        modifyPassword(){
            window.open('http://uc.cheguo.com/')
        }
    },
    created(){
        // // 拿到用户信息，若没有，则跳到统一登录
        // let data = sessionStorage.getItem('userData')
        // if(!data){
        //     // this.unifiedLogin()
        //     return
        // }
        // let userData = JSON.parse(Base64.decode(data)) || {}
        // this.$store.commit('saveUesrData',userData)
        // this.name = userData.UserName
    },
    computed:{
        key() {
            return this.$route.name
        },
        name(){
            return this.$store.state.name
        }
    }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.container-kpi{
    width: 100px
}
.page-content{
    height: 100%;
    overflow: auto;
}
.main-logo{
    /* background: #5e564c; */
    padding:30px
}
header{
    border-bottom:1px solid #bebebe
}
.header-person li{
    float:left;
    padding:0 10px
}
</style>
