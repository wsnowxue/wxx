<template>
    <div class="filiale-judge-manage">
        <!-- 搜索 -->
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="考核维度名称：" prop="dimension_name">
                <el-input v-model="tableSearch.dimension_name" placeholder="请输入考核办法名称"></el-input>
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
            <div v-if="_has('/dimensionapi/adddimension')" class="text-right operation">
                <el-button type="primary" @click="dialogVisible = true;resetForm('tableObject')"> <i class="el-icon-circle-plus-outline"></i> 新增维度</el-button>
            </div>
        </el-form>
        <div class="main-space"></div>
        <!-- 表格 -->
        <el-table
        :data="tableData"
        stripe border>
            <el-table-column prop="dimension_name" label="维度名称" show-overflow-tooltip>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="启用状态">
                <template slot-scope="scope">
                    {{scope.row.statue | state}}
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" v-show="_has('/dimensionapi/changedimensionstatue')" label="操作">
                <template slot-scope="scope">
                    <el-button type="text" @click="openChangeStatue(scope.row)" v-if="scope.row.statue == '0'">启用</el-button>
                    <el-button type="text" @click="openChangeStatue(scope.row)" v-if="scope.row.statue == '1'">停用</el-button>
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
            title="新增维度"
            :visible.sync="dialogVisible"
            width="500px" class="performance-dialog">
            <el-form :model="tableObject" :rules="rules" status-icon ref="tableObject" label-width="130px" class="demo-ruleForm">
                <el-form-item label="考核维度名称：" prop="dimension_name">
                    <el-input v-model="tableObject.dimension_name" placeholder="请输入维度名称"></el-input>
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
import judgeDimension from "./judgeDimension.js";
export default {
  ...judgeDimension
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
