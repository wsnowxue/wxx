<template>
<!-- 分公司任务管理 - 页面 -->
<div>
    <!-- 搜索 -->
    <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
        <el-form-item label="任务名称：" prop="task_name">
            <el-input v-model="tableSearch.task_name" placeholder="请输入任务名称"></el-input>
        </el-form-item>
        <el-form-item label="任务分发状态：" prop="task_statue" style="width:330px">
            <el-select v-model="tableSearch.task_statue" placeholder="任务分发状态">
            <el-option label="已分发" value="1"></el-option>
            <el-option label="未分发" value="0"></el-option>
            </el-select>
        </el-form-item>
        <div class="text-center">
            <el-form-item>
                <el-button @click="resetForm('tableSearch')"><i class="el-icon-circle-close-outline"></i> 清除查询条件</el-button>
                <el-button type="primary" @click="getData('tableSearch')"><i class="el-icon-search"></i> 查询</el-button>
            </el-form-item>
        </div>
        <div class="text-right operation">
            <el-button type="primary" v-if="_has('/addtask'+type)" @click="dialogVisible = true"><i class="el-icon-circle-plus-outline"></i> 创建新任务</el-button>
            <el-button type="primary" @click="exportTask()"><i class="el-icon-circle-plus-outline"></i> 任务导出</el-button>
        </div>
    </el-form>
    <div class="main-space"></div>
    <!-- 表格 -->
    <el-table :data="tableData" stripe border style="width: 100%" @selection-change="handleSelectionChange">
            <el-table-column type="selection" label="序号"></el-table-column>
            <el-table-column v-for="item in tableThead" :key="item.id" :prop="item.value" :label="item.name" show-overflow-tooltip></el-table-column>>
        <el-table-column label="操作" :width="$store.state.width240">
            <template slot-scope="scope">
                <el-button type="text"  @click="openDialog2(scope.row)" v-if="scope.row.task_distribute_statue == 0 && scope.row.creator_user_id == UserId">上传任务</el-button>
                <el-button type="text" v-if="_has('/filialeTaskDetail'+type)" @click="toFilialeTaskDetail(scope.row)">查看详情</el-button>
                <el-button type="text" v-if="_has('/taskapi/deletetask'+type) && scope.row.task_distribute_statue == 0 && scope.row.creator_user_id == UserId" @click="deleteTask(scope.row)">删除</el-button>
                <el-button type="text" v-if="_has('/taskapi/sendtask'+type) && scope.row.task_distribute_statue == 0 && scope.row.creator_user_id == UserId" @click="sendTask(scope.row)">分发</el-button>
            </template>
        </el-table-column>
    </el-table>
    <div class="main-space"></div>    
    <!-- 分页 -->
    <el-pagination
      @size-change="sizeChange"
      @current-change="currentChange"
      :current-page.sync="currentPage"
      :page-sizes="pageNumber"
      layout="total, sizes, prev, pager, next, jumper"
      :total="total">
    </el-pagination>
    <!-- 弹窗 -->
    <el-dialog title="创建新任务" :visible.sync="dialogVisible" width="576px" class="performance-dialog task_dialog" @close="closeDialog">
        <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="140px" class="demo-ruleForm" style="width:500px">
            <el-form-item label="任务名称：" prop="task_name">
                <el-input v-model="ruleForm.task_name" placeholder="请输入任务名称" style="width:350px"></el-input>
            </el-form-item>
            <el-form-item label="考核起止时间：" prop="chose_time">
                <el-date-picker
                v-model="ruleForm.chose_time"
                type="daterange"
                value-format="yyyy-MM-dd"
                range-separator="至"
                start-placeholder="请选择"
                end-placeholder="请选择">
                </el-date-picker>
            </el-form-item>
            <el-form-item label="任务上传：" class="form_item">
                <span class="model_name">全年任务指标数据录入模板</span>
                <el-button type="primary" @click="download1()">下载</el-button>
            </el-form-item>
            <el-form-item class="form_item" v-if="type == 0">
                <span class="model_name">新车数据录入模板</span>
                <el-button type="primary" @click="download4()">下载</el-button>
            </el-form-item>
            <el-form-item class="form_item" v-if="type == 0">
                <span class="model_name">二手车数据录入模板</span>
                <el-button type="primary" @click="download5()">下载</el-button>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="submitForm('ruleForm')" :disabled="disabled">创建任务</el-button>
            </el-form-item>
        </el-form>
    </el-dialog>
    <!-- 上传弹窗 -->
    <el-dialog title="任务完成量数据上传" :visible.sync="dialogVisible2" width="576px" class="performance-dialog task_dialog" @close="closeDialog">
        <el-form :model="ruleForm" label-width="140px" class="demo-ruleForm" style="width:500px">
            <el-form-item label="任务上传：" prop="file_list" class="form_item">
                <span class="model_name">全年任务指标数据录入模板</span>
                <el-upload
                    class="upload-demo"
                    ref="upload1"
                    action="/UpLoad/UpLoadFile"
                    v-model="ruleForm.file_list"
                    :limit="1"
                    :on-remove="removeUpload(0)"
                    :on-success="uploadSuccess1"
                    :auto-upload="true">
                    <el-button slot="trigger" size="small" type="primary">上传</el-button>
                </el-upload>
            </el-form-item>
            <el-form-item label="" prop="" class="form_item" v-if="type == 0">
                <span class="model_name">新车数据录入模板</span>
                <el-upload
                    class="upload-demo"
                    ref="upload4"
                    action="/UpLoad/UpLoadFile"
                    :limit="1"
                    :on-remove="removeUpload(1)"
                    :on-success="uploadSuccess4"
                    :auto-upload="true">
                    <el-button slot="trigger" size="small" type="primary">上传</el-button>
                </el-upload>
            </el-form-item>
            <el-form-item label="" prop="" class="form_item" v-if="type == 0">
                <span class="model_name">二手车数据录入模板</span>
                <el-upload
                    class="upload-demo"
                    ref="upload5"
                    action="/UpLoad/UpLoadFile"
                    :limit="1"
                    :on-remove="removeUpload(2)"
                    :on-success="uploadSuccess5"
                    :auto-upload="true">
                    <el-button slot="trigger" size="small" type="primary">上传</el-button>
                    <div slot="tip" class="el-upload__tip">点击上传时，请上传与下载的模板类型一致的execl文件</div>
                </el-upload>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="upLoadTask()" >确认上传</el-button>
            </el-form-item>
        </el-form>
    </el-dialog>
</div>
</template>

<script>
import filialeTask from "./filialeTask";
export default {
  ...filialeTask
}
</script>

<style>
    .form_item {
        position: relative;
    }
    .task_dialog .el-upload {
        position: absolute;
        right: 10px;
        top: 0px;
    }
</style>
