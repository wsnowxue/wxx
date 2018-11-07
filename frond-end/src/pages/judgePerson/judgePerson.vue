<template>
<!-- 分公司/个人考核详情-详情/考核/自评 -->
    <div>
        <div class="text-center">{{titleName}}</div>
        <div class="main-space"></div>
        <el-tabs v-model="activeName">
            <el-tab-pane label="考核指标" name="first">
                <ul class="indicators">
                    <li v-for="item in indicators" :key="item.id">{{item.indicator_name}}：{{item.indicator_value}}</li>
                </ul>
            </el-tab-pane>
        </el-tabs>
        <div class="main-space"></div>
        <div>
            <el-table :data="tableData" stripe border style="width: 100%" :span-method="objectSpanMethod">
                <el-table-column :width="$store.state.width" prop="dimension_name" label="考核维度" show-overflow-tooltip></el-table-column>
                <el-table-column prop="dimension_detail_name" label="细项指标" show-overflow-tooltip></el-table-column>
                <el-table-column :width="$store.state.width" prop="base_score" label="基础分值"></el-table-column>
                <!-- <el-table-column :width="$store.state.width" prop="base_standard_score" label="基础分标准值" show-overflow-tooltip :formatter="formatter"></el-table-column> -->
                <!-- <el-table-column :width="$store.state.width" prop="finish" label="实际完成" show-overflow-tooltip :formatter="formatter"></el-table-column> -->
                <!-- <el-table-column prop="formule" label="指标公式" show-overflow-tooltip :formatter="formatter"></el-table-column> -->
                <!-- <el-table-column prop="check_method" label="考核办法" show-overflow-tooltip :formatter="formatter"></el-table-column> -->
                <el-table-column :width="$store.state.width" label="考评得分">
                    <template slot-scope="scope">
                        <div v-if="scope.row.formule.toString().indexOf('@') != 0">{{scope.row.assessment_result}}</div>
                        <el-input-number v-if="scope.row.formule.toString().indexOf('@') == 0" v-model.number="scope.row.assessment_result" :precision="2" :min="0" :max="scope.row.base_score" :step="0.1" :disabled="assessment_detail_statue == 0"></el-input-number>
                    </template>
                </el-table-column>
            </el-table>
            <div class="main-space"></div>
            <div v-if="type == 0 || (type == 1 && assessment_detail_statue == 0)">
                考评得分：{{total_score}}分
            </div>
            <div v-if="type == 1 && assessment_detail_statue == 1">
                <span>考评得分：{{fixed_score}}分</span>
                <!-- <span>考评得分：{{fixed_score}}分 + (</span> -->
                <!-- <span v-for="(item,index) in checkerList" :key="item.checker_name"><span v-if="index > 0">+</span>{{item.checker_name}}： -->
                <!-- <el-input v-if="item.is_checked_true" v-model.number="unfixed_score" :readonly="true" style="width:60px"></el-input> -->
                <!-- <el-input v-if="!item.is_checked_true" v-model.number="item.result" :readonly="true" :disabled="true" style="width:60px"></el-input> -->
                <!-- 分*{{item.checker_weight}}%</span>) = {{total_score}}分 -->
            </div>
            <div class="text-center back-page">
                <el-button @click="backPage">返回</el-button>
                <!-- <el-button type="primary" v-if="_has('/submitJudge'+type),assessment_detail_statue == 1" @click="submit">提交</el-button> -->
            </div>
        </div>
    </div>
</template>

<script>
import judgePerson from "./judgePerson"
export default {
    ...judgePerson
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="less">
.indicators {
    float: left;
    >li{
        float: left;
        width: 320px;
        margin-bottom: 10px;
    }
}
</style>
