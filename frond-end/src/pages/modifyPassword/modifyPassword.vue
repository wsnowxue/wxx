<template>
    <div class="modify-password">
        <div class="header-title">
            <h3 class="text-center">修改个人信息</h3>
        </div>
        <div class="modify-password-form">
            <el-form label-width="120px" :model="changePass" ref="changePass" :rules="rules">
                <el-form-item label="原密码：" prop="oldpassword">
                    <el-input type="password" v-model="changePass.oldpassword" placeholder="请输入原密码"></el-input>
                </el-form-item>
                <el-form-item label="新密码：" prop="newpassword">
                    <el-input type="password" v-model="changePass.newpassword" placeholder="请输入密码" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="确认密码：" prop="checkPass">
                    <el-input type="password" v-model="changePass.checkPass" placeholder="请再次输入原密码" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="modifyPassword('changePass')">立即修改</el-button>
                </el-form-item>
            </el-form>
        </div>
    </div>
</template>

<script>
import modifyPassword from "./modifyPassword.js";
export default {
    ...modifyPassword
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="less" scoped>
@import url(./modifyPassword.less);
</style>
