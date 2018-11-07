<template>
  <div class="filialeJudgeManageAdd">
        <!-- 标题 -->
        <div class="text-center">{{type == '0' ? '分公司' : '个人'}}任务详情</div>
        <div class="main-space"></div>
        <el-tabs v-model="activeName">
            <el-tab-pane v-if="_has('/taskapi/getyearlytask'+type)" label="年度任务指标" name="first1">
                <el-table
                :data="tableDataYearly"
                border
                style="width: 100%">
                    <el-table-column type="index" label="序号"></el-table-column>
                    <el-table-column width="120" :label="typeFlag" prop="task_object_name" key="task_object_name"></el-table-column>
                    <el-table-column
                    v-for="item in yearTask"
                    :key="item.value"
                    :prop="item.value"
                    :label="item.name"
                    show-overflow-tooltip>
                    </el-table-column>
                </el-table>
            </el-tab-pane>
            <!-- <el-tab-pane v-if="_has('/taskapi/getfinanicalproductstask') && type == '0'" label="金融产品" name="first2">
                <el-table
                :data="tableFinanical"
                border
                style="width: 100%">
                    <el-table-column type="index" label="序号"></el-table-column>
                    <el-table-column
                    v-for="item in financeProduct"
                    :key="item.value"
                    :prop="item.value"
                    :label="item.name"
                    show-overflow-tooltip>
                    </el-table-column>
                </el-table>
            </el-tab-pane>
            <el-tab-pane v-if="_has('/taskapi/getbanktask') && type == '0'" label="银行分类" name="first3">
                <el-table
                :data="tableDataBank"
                border
                style="width: 100%">
                    <el-table-column type="index" label="序号"></el-table-column>
                    <el-table-column
                    v-for="item in bankType"
                    :key="item.value"
                    :prop="item.value"
                    :label="item.name"
                    show-overflow-tooltip>
                    </el-table-column>
                </el-table>
            </el-tab-pane> -->
            <el-tab-pane v-if="_has('/taskapi/getfinanicalproductstask') && type == '0'" label="新车" name="first4">
                <el-table
                :data="tableDataNewCar"
                border
                style="width: 100%">
                    <el-table-column type="index" label="序号"></el-table-column>
                    <el-table-column width="120" label="分公司" prop="task_object_name" key="task_object_name"></el-table-column>
                    <el-table-column
                    v-for="item in yearTask"
                    :key="item.value"
                    :prop="item.value"
                    :label="item.name"
                    show-overflow-tooltip>
                    </el-table-column>
                </el-table>
            </el-tab-pane>
            <el-tab-pane v-if="_has('/taskapi/getbanktask') && type == '0'" label="二手车" name="first5">
                <el-table
                :data="tableDataOldCar"
                border
                style="width: 100%">
                    <el-table-column type="index" label="序号"></el-table-column>
                    <el-table-column width="120" label="分公司" prop="task_object_name" key="task_object_name"></el-table-column>
                    <el-table-column
                    v-for="item in yearTask"
                    :key="item.value"
                    :prop="item.value"
                    :label="item.name"
                    show-overflow-tooltip>
                    </el-table-column>
                </el-table>
            </el-tab-pane>
        </el-tabs>
        <!-- 表格 -->
        
        <div class="text-center back-page">
            <el-button @click="backPage">返回</el-button>
        </div>
    </div>
</template>

<script>
import filialeTaskDetail from "./filialeTaskDetail";
export default {
  ...filialeTaskDetail
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
