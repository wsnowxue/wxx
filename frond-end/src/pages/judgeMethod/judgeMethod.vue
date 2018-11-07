<template>
    <div class="filiale-judge-manage">
        <!-- 搜索 -->
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="考核办法名称：" prop="check_method_name">
                <el-input v-model="tableSearch.check_method_name" placeholder="请输入考核办法名称"></el-input>
            </el-form-item>
            <el-form-item label="考核办法：" prop="check_method_define">
                <el-input v-model="tableSearch.check_method_define" placeholder="请输入考核办法"></el-input>
            </el-form-item>
            <el-form-item label="启用状态：" prop="statue">
                <el-select v-model="tableSearch.statue" placeholder="--请选择--">
                    <el-option label="启用" value="1"></el-option>
                    <el-option label="停用" value="0"></el-option>
                </el-select>
            </el-form-item>
            <div class="text-center">
                <el-form-item>
                    <el-button @click="resetForm('tableSearch')"><i class="el-icon-circle-close-outline"></i> 清除查询条件</el-button>
                    <el-button type="primary" @click="getData('tableSearch')"><i class="el-icon-search"></i> 查询</el-button>
                </el-form-item>
            </div>
            <div v-if="_has('/checkmethodapi/addcheckmethod')" class="text-right operation">
                <el-button type="primary" @click="dialogVisible = true;resetForm('tableObject')"> <i class="el-icon-circle-plus-outline"></i> 新增考核办法</el-button>
            </div>
        </el-form>
        <div class="main-space"></div>
        <!-- 表格 -->
        <el-table
        :data="tableData"
        stripe border>
            <el-table-column
            v-for="item in tableThead"
            :key="item.value"
            :prop="item.value"
            :label="item.name" show-overflow-tooltip>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="启用状态">
                <template slot-scope="scope">
                    {{scope.row.check_method_statue | state}}
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" v-show="_has('/checkmethodapi/changecheckmethodstatue')" label="操作">
                <template slot-scope="scope">
                    <el-button type="text" v-if="!scope.row.check_method_statue" @click="changeStatue(scope.row)">启用</el-button>
                    <el-button type="text" v-if="scope.row.check_method_statue" @click="changeStatue(scope.row)">停用</el-button>
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
        :page-size="10"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total">
        </el-pagination>
        
        <!-- 弹窗 -->
        <el-dialog
            title="新增考核办法"
            :visible.sync="dialogVisible"
            width="500px" class="performance-dialog">
            <el-form :model="tableObject" :rules="rules" status-icon ref="tableObject" label-width="130px" class="demo-ruleForm">
                <el-form-item label="考核办法名称：" prop="check_method_name">
                    <el-input v-model="tableObject.check_method_name" placeholder="请输入考核办法名称"></el-input>
                </el-form-item>
                <el-form-item label="考核办法：" prop="check_method_define">
                    <el-input
                        class="text_font"
                        type="textarea"
                        :rows="3"
                        placeholder="请输入考核办法"
                        v-model="tableObject.check_method_define">
                    </el-input>
                </el-form-item>
                <el-form-item label="处理方法：" prop="check_method_proc">
                    <el-select v-model="tableObject.check_method_proc" placeholder="--请选择--">
                        <el-option  label="脚本处理" value=".lua"></el-option>
                        <el-option  label="存储过程处理" value=".procdure"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submitForm('tableObject')">确认</el-button>
                    <!-- <el-button @click="resetForm('tableObject')">重置</el-button> -->
                </el-form-item>
            </el-form>
        </el-dialog>
    </div>
</template>

<script>
import judgeMethod from "./judgeMethod.js";
export default {
  ...judgeMethod
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
</style>
