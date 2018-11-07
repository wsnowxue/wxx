<template>
    <div class="login" v-loading="loadding" element-loading-text="拼命加载中" element-loading-background="rgba(0, 0, 0, 0.8)">
        <img src="@/assets/background.jpg" alt="" class="bg-img">
        <header>
            <div class="login-container">
                <h1><img src="@/assets/logo.png" alt=""><span>绩效考核管理系统</span></h1>
            </div>
        </header>
        <div class="login-container">
            <el-main>
                <div class="login-main">
                    <h3 class="text-center">用户登录</h3>
                    <el-form :model="user" ref="user" :rules="rules" status-icon label-width="0px" class="demo-dynamic">
                    <el-form-item prop="userName">
                        <el-input size="medium" v-model="user.userName" placeholder="请输入用户名">
                            <template slot="prepend"><i class="iconfont icon-yonghuming"></i></template>
                        </el-input>
                    </el-form-item>
                    <el-form-item prop="password">
                        <el-input size="medium" type="password" v-model="user.password" placeholder="请输入密码">
                            <template slot="prepend"><i class="iconfont icon-mima"></i></template>                        
                        </el-input>
                    </el-form-item>
                    <el-form-item>
                        <el-checkbox v-model="rememberMe" class="login-check">记住密码</el-checkbox>
                    </el-form-item>
                    <el-form-item class="text-center">
                        <el-button size="medium" type="primary" @click="getData('user')" :disabled="disabled">登录</el-button>
                    </el-form-item>
                    </el-form>
                </div>
            </el-main>
        </div>
        
    </div>
</template>

<script>
import login from "./login.js";
export default {
  ...login
}
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="less" scoped>
@import url(./login.less);
</style>
