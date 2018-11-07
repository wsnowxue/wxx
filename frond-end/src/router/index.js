import Vue from 'vue'
import Router from 'vue-router'
import filialeTask from '../pages/filialeTask/filialeTask.vue'
import filialeTaskDetail from '../pages/filialeTaskDetail/filialeTaskDetail.vue'
import judgeManageFiliale from '../pages/judgeManageFiliale/judgeManageFiliale.vue'
import judgeManageFilialeDetail from '../pages/judgeFilialeDetail/judgeFilialeDetail.vue'
import judgeManagePersonDetail from '../pages/judgePersonDetail/judgePersonDetail.vue'
import judgePerson from '../pages/judgePerson/judgePerson.vue'
import indexDefinition from '../pages/indexDefinition/indexDefinition.vue'
import judgeMethod from '../pages/judgeMethod/judgeMethod.vue'
import judgeDimension from '../pages/judgeDimension/judgeDimension.vue'
import judgeTerm from '../pages/judgeTerm/judgeTerm.vue'
import filialeJudgeManage from '../pages/filialeJudgeManage/filialeJudgeManage.vue'
import filialeJudgeManageAdd from '../pages/filialeJudgeManageAdd/filialeJudgeManageAdd.vue'
import filialeJudgeManageDetail from '../pages/filialeJudgeManageDetail/filialeJudgeManageDetail.vue'
import filialeVerifyManage from '../pages/filialeVerifyManage/filialeVerifyManage.vue'
import filialeVerifyManageDetail from '../pages/filialeVerifyManageDetail/filialeVerifyManageDetail.vue'
import modifyPassword from '../pages/modifyPassword/modifyPassword.vue'
import notfind from '../pages/notfind/notfind'
import dashboard from '../pages/dashboard/dashboard.vue'
import login from '../pages/login/login.vue'
import pages from '../pages/pages'

Vue.use(Router)

// 路由完全配置，在pages.vue里面获取后台给的地址
export default new Router({
  routes: [
    
    {
        path: '/login',
        name: '/login',
        component: login
    },
    {
        path: '/',
        component: pages,
        children:[
            {
                path: '/dashboard',
                name: '/dashboard',
                component: dashboard
            },
            // 任务分发管理-分公司和个人共用一个页面
            {
                // 分公司任务管理
                path: '/filialeTask',
                name: '/filialeTask',
                component: filialeTask
            },
            {
                // 个人任务分发管理
                path: '/personTask',
                name: '/personTask',
                component: filialeTask
            },
            {
                // 分公司任务管理-详情
                path: '/filialeTaskDetail',
                name: '/filialeTaskDetail',
                component:  filialeTaskDetail
            },
            {
                // 个人任务分发管理-详情
                path: '/personTaskDetail',
                name: '/personTaskDetail',
                component: filialeTaskDetail
            },
            // 考核管理-分公司和个人共用一个页面
            {
                // 考核管理-分公司
                path: '/judgeManageFiliale',
                name: '/judgeManageFiliale',
                component: judgeManageFiliale
            },
            {
                // 考核管理-个人
                path: '/judgeManagePerson',
                name: '/judgeManagePerson',
                component: judgeManageFiliale
            },
            {
                // 考核详情-分公司
                path: '/judgeManageFilialeDetail',
                name: '/judgeManageFilialeDetail',
                component: judgeManageFilialeDetail
            },
            {
                // 考核-分公司
                path: '/judgeManageFilialeCheck',
                name: '/judgeManageFilialeCheck',
                component: judgePerson
            },
            {
                // 考核详情-个人
                path: '/judgeManagePersonDetail',
                name: '/judgeManagePersonDetail',
                component: judgeManagePersonDetail
            },
            {
                // 考核-个人
                path: '/judgeManagePersonCheck',
                name: '/judgeManagePersonCheck',
                component: judgePerson
            },
            {
                // 指标定义维护
                path: '/indexDefinition',
                name: '/indexDefinition',
                component: indexDefinition
            },
            {
                // 考核办法维护
                path: '/judgeMethod',
                name: '/judgeMethod',
                component: judgeMethod
            },
            {
                // 考核维度维护
                path: '/judgeDimension',
                name: '/judgeDimension',
                component: judgeDimension
            },
            {
                // 考核细项及公式维护
                path: '/judgeTerm',
                name: '/judgeTerm',
                component: judgeTerm
            },
            {
                // 分公司考核方案管理
                path: '/filialeJudgeManage',
                name: '/filialeJudgeManage',
                component: filialeJudgeManage
            },
            {
                // 分公司考核方案 新增
                path: '/filialeJudgeManageAdd',
                name: '/filialeJudgeManageAdd',
                component: filialeJudgeManageAdd
            },
            {
                // 分公司考核方案 详情
                path: '/filialeJudgeManageDetail',
                name: '/filialeJudgeManageDetail',
                component: filialeJudgeManageDetail
            },
            {
                // 个人考核方案 管理  -- 引用 “分公司考核方案管理” 组件
                path: '/personJudgeManage',
                name: '/personJudgeManage',
                component: filialeJudgeManage
            },
            {
                // 个人考核方案 新增 -- 引用 “分公司考核方案 新增” 组件
                path: '/personJudgeManageAdd',
                name: '/personJudgeManageAdd',
                component: filialeJudgeManageAdd
            },
            {
                // 个人考核方案 详情  -- 引用 “分公司考核方案 详情” 组件
                path: '/personJudgeManageDetail',
                name: '/personJudgeManageDetail',
                component: filialeJudgeManageDetail
            },
            {
                // 分公司考核方案审核管理
                path: '/filialeVerifyManage',
                name: '/filialeVerifyManage',
                component: filialeVerifyManage
            },
            {
                // 分公司考核方案审核管理详情
                path: '/filialeVerifyManageDetail',
                name: '/filialeVerifyManageDetail',
                component: filialeVerifyManageDetail
            },
            {
                // 个人考核方案审核管理 -- 引用 “分公司考核方案审核管理” 组件
                path: '/personVerifyManage',
                name: '/personVerifyManage',
                component: filialeVerifyManage
            },
            {
                // 个人考核方案待审核详情 -- 引用 “分公司考核方案审核管理详情” 组件
                path: '/personVerifyManageDetail',
                name: '/personVerifyManageDetail',
                component: filialeVerifyManageDetail
            },
            {
                // 修改密码
                path: 'modifyPassword',
                name: '/modifyPassword',
                component:modifyPassword
            },
            {
                path: '*',
                name: '/notfind',
                component: notfind
            }
        ]
    },
    
    
  ]
})
