<template>
<!-- 考核维度维护 -->
    <div class="judge-term">
        <!-- 搜索 -->
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="细项名称：" prop="detail_name">
                <el-input v-model="tableSearch.detail_name" placeholder="请输入细项名称"></el-input>
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
            <div v-if="_has('/DimensionDetail/AddDimensionDetail')" class="text-right operation">
                <el-button type="primary" @click="dialogVisible = true;resetForm('tableObject')"> <i class="el-icon-circle-plus-outline"></i> 新增细项及公式</el-button>
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
                    {{scope.row.statue | state}}
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" v-show="_has('/DimensionDetail/ChangeDimensionDetailStatue')" label="操作">
                <template slot-scope="scope">
                    <el-button type="text" @click="openChangeStatue(scope.row)" v-if="scope.row.statue == 0">启用</el-button>
                    <el-button type="text" @click="openChangeStatue(scope.row)" v-if="scope.row.statue == 1">停用</el-button>
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
            title="新增细项及指标公式"
            :visible.sync="dialogVisible"
            width="1000px" class="performance-dialog">
            <el-form :model="tableObject" :rules="rules" status-icon ref="tableObject" label-width="130px" class="demo-ruleForm">
                <el-form-item label="细项名称：" prop="detail_name">
                    <el-input v-model="tableObject.detail_name" placeholder="请输入细项名称"></el-input>
                </el-form-item>
                <el-form-item label="可选择指标：">
                    <ul class="clear demission">
                        <li class="fl" v-for="(item,index) in definitions" :key="item.id">{{index+1}}. {{item.indicator_name}}</li>
                    </ul>
                </el-form-item>
                <el-form-item label="指标公式：" prop="formule">
                    <el-input type="textarea" class="text_font" v-model="tableObject.formule" placeholder="请输入指标公式"></el-input>
                </el-form-item>
                <el-form-item label="考核办法：" prop="method_id">
                    <el-select v-model="tableObject.method_id" placeholder="--请选择--">
                        <el-option v-for="item in check_methods" :key="item.id" :label="item.check_method_name" :value="item.id"></el-option>
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
import judgeTerm from "./judgeTerm.js";
export default {
  ...judgeTerm
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="less" scoped>
@import url(./judgeTerm.less);
</style>
