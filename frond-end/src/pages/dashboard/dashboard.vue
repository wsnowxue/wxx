<template>
    <div>
      <h3>通知栏</h3>
      <el-row :gutter="12">
        <el-col :span="4" v-for="(item,index) in taskList" :key="item.task_type" @click.native="item.disabled && toTaskPage(item,index)">
          <el-card shadow="hover" :class="item.elCard">
            <div>{{item.task_count}}</div>
            <div>{{item.task_type_name}}</div>
          </el-card>
        </el-col>
      </el-row>
      <h3>考核记录</h3>
      <el-card shadow="hover">
        <!-- 分公司折线图 -->
        <div id="main1" style="width: 100%;height: 500px;" v-show="_has('/YearlyAssessmentLine0')"></div>
        <!-- 个人折线图 -->
        <div id="main2" style="width: 100%;height: 500px;" v-show="_has('/YearlyAssessmentLine1')"></div>
      </el-card>
    </div>
</template>

<script>
import dashboard from './dashboard.js'
export default {
    ...dashboard
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  .el_card {
    text-align: center;
    background-color: #f3f3f3;
    /* margin-bottom: 10px; */
  }
  .el_card_life {
    text-align: center;
    cursor: pointer;
  }
</style>
