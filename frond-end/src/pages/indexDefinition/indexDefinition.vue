<template>
    <div class="filiale-judge-manage">
        <!-- 搜索 -->
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="指标名称：" prop="indicator_name">
                <el-input v-model="tableSearch.indicator_name" placeholder="请输入指标名称"></el-input>
            </el-form-item>
            <el-form-item label="定义：" prop="indicator_define">
                <el-input v-model="tableSearch.indicator_define" placeholder="请输入定义"></el-input>
            </el-form-item>
            <el-form-item label="统计方式：" prop="indicator_count_method">
                <el-select v-model="tableSearch.indicator_count_method" placeholder="--请选择--">
                    <el-option v-for="item in countMethod" :key="item.id" :label="item.count_method_name" :value="item.id"></el-option>
                </el-select>
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
            <div v-if="_has('/indicatorsapi/addindicators')" class="text-right operation">
                <el-button type="primary" @click="dialogVisible = true;resetForm('tableObject')"> <i class="el-icon-circle-plus-outline"></i> 新增指标</el-button>
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
            <el-table-column label="统计方式" show-overflow-tooltip>
                <template slot-scope="scope">
                    {{scope.row.indicator_count_method | filterMethod(countMethod)}}
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="启用状态">
                <template slot-scope="scope">
                    {{scope.row.statue | state}}
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" v-show="_has('/indicatorsapi/changeindicatorsstatue')" label="操作">
                <template slot-scope="scope">
                    <el-button type="text" @click="openChangeStatue(scope.row)" v-if="!scope.row.statue">启用</el-button>
                    <el-button type="text" @click="openChangeStatue(scope.row)" v-if="scope.row.statue">停用</el-button>
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
            title="新增考核指标"
            :visible.sync="dialogVisible"
            width="500px" class="performance-dialog">
            <el-form :model="tableObject" :rules="rules" status-icon ref="tableObject" label-width="120px" class="demo-ruleForm">
                <el-form-item label="指标名称：" prop="indicator_name">
                    <el-input v-model="tableObject.indicator_name" placeholder="请输入指标名称"></el-input>
                </el-form-item>
                <el-form-item label="定义：" prop="indicator_define">
                    <el-input v-model="tableObject.indicator_define" placeholder="请输入定义"></el-input>
                </el-form-item>
                <el-form-item label="统计方式：" prop="indicator_count_method">
                    <el-select v-model="tableObject.indicator_count_method" placeholder="--请选择--">
                        <el-option v-for="item in countMethod" :key="item.id" :label="item.count_method_name" :value="item.id"></el-option>
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
import indexDefinition from "./indexDefinition.js";
export default {
  ...indexDefinition
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
