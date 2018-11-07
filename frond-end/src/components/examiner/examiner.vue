<template>
    <div>
        <el-dialog title="发起绩效考核" :visible.sync="obj.flag" width="560px" class="performance-dialog" @open="openDialog" @close="closeDialog">
            <el-form :rules="rules" :model="tableObject" status-icon ref="tableObject" label-width="140px" class="demo-ruleForm" style="width:540px">
                <el-form-item label="绩效考核名称：" prop="assessment_name">
                    <el-input v-model="tableObject.assessment_name" placeholder="请输入绩效考核名称" style="width:350px"></el-input>
                </el-form-item>
                <el-form-item label="考核起止时间：" prop="start_end_time">
                    <el-date-picker type="daterange" v-model="tableObject.start_end_time" start-placeholder="考核开始时间" range-separator="至" end-placeholder="考核结束时间" align="center" value-format="yyyy-MM-dd"></el-date-picker>
                </el-form-item>
                <el-form-item label="考核对象：" v-if="type == '1'" prop="checkerObj">
                    <el-button type="primary" @click="chosePerson = true">选择考核对象</el-button>
                    <div>
                        <span v-show="choseButton" v-for="(item,index) in chosenChecker" :key="item.id"><span v-if="index > 0">，</span>{{item.name}}</span>
                    </div>
                </el-form-item>
                <el-form-item label="考核方案名称：" prop="templete_id">
                    <el-select v-model="tableObject.templete_id" placeholder="--请选择--" @change="templateChange($event)">
                        <el-option v-for="item in templeteList" :key="item.templete_id" :label="item.templete_name" :value="item.templete_id"></el-option>
                    </el-select>
                    <!-- <el-button type="primary" @click="download()" :disabled="disabled2">下载</el-button> -->
                </el-form-item>
                <el-form-item label="考评人：" prop="checker_list_rule" v-if="tableObject.templete_id">
                    <div class="item-examiner" v-if="checker_list.length > 0" v-for="(item,index) in checker_list" :key="item.checker_id">
                        <div class="examiner">
                            <span class="el-icon-error" @click="deleteExaminer(index)"></span>
                            <div class="examiner-name" :title="item.checker_name">{{item.checker_name.substr(0,2)}}</div>
                        </div>
                        <span class="el-icon-back"></span>
                    </div>
                    <div class="examiner item-examiner plus-examiner" @click="innerVisible = true;sourceChecker = 1">
                        <span class="el-icon-plus"></span>
                    </div>
                </el-form-item>
                
                <el-form-item>
                    <el-button type="primary" @click="submitForm('tableObject')" :disabled="disabled">发起考核</el-button>
                </el-form-item>
            </el-form>
            <el-dialog title="选择考核对象" width="40%" :visible.sync="chosePerson" class="performance-dialog" append-to-body>
                <el-tree :data="data" show-checkbox ref="tree" node-key="id" :props="defaultProps"></el-tree>
                <div class="main-space"></div> 
                <div class="text-center">
                    <el-button type="primary" @click="getCheckedKeys">确定</el-button>
                </div>
            </el-dialog>
            <el-dialog title="请选择" width="40%" :visible.sync="innerVisible" class="performance-dialog" append-to-body>
                <el-table :data="gridData" ref="singleTable" stripe border style="width: 100%" highlight-current-row @row-click="handleCurrentClick">
                    <el-table-column type="index" prop="id" label="序号"></el-table-column>
                    <el-table-column prop="role_name" label="角色名称"></el-table-column>
                    <el-table-column prop="organize_name" label="机构名称"></el-table-column>
                    <el-table-column prop="department_name" label="部门"></el-table-column>
                    <el-table-column prop="name" label="姓名"></el-table-column>
                </el-table>
            </el-dialog>
        </el-dialog>
    </div>
</template>
<script>
import examiner from "./examiner";
export default {
    ...examiner
}
</script>
<style lang="less" scoped>
    @import url(./examiner.less);
</style>
