import echarts from 'echarts';
export default {
    name: 'dashboard',
    data () {
        return {
            type: 0, // 0 分公司 1 个人
            lineTitle1:'',
            lineTitle2:'',
            seriesData1:[],
            seriesData2:[],
            taskList:[],
            charts: '',
        }
    },
    methods:{
        drawLine1(id){
            setTimeout(() => {
                this.charts = echarts.init(document.getElementById(id))
                this.charts.setOption({
                    title: {
                        left: 'center',
                        text: this.lineTitle1,
                    },
                    tooltip: {
                        trigger: 'axis'
                    },
                    xAxis: {
                        type: 'category',
                        data: ['1月', '2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月']
                    },
                    yAxis: {
                        type: 'value',
                        name: '考核得分（分）',
                        splitNumber: 9
                    },
                    series: [{
                        type: 'line',
                        data: this.seriesData1,
                        label: {
                            normal: {
                                show: true,
                                position: 'top',
                            }
                        },
                        color: '#6191d2'
                    }],
                })
            },100);
        },
        drawLine2(id){
            setTimeout(() => {
                this.charts = echarts.init(document.getElementById(id))
                this.charts.setOption({
                    title: {
                        left: 'center',
                        text: this.lineTitle2,
                    },
                    tooltip: {
                        trigger: 'axis'
                    },
                    xAxis: {
                        type: 'category',
                        data: ['1月', '2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月']
                    },
                    yAxis: {
                        type: 'value',
                        name: '考核得分（分）',
                        splitNumber: 9
                    },
                    series: [{
                        type: 'line',
                        data: this.seriesData2,
                        label: {
                            normal: {
                                show: true,
                                position: 'top',
                            }
                        },
                        color: '#6191d2'
                    }],
                })
            },100);
        },
        getAllTask() {
            let params = {
                person: this.$store.state.userData.UserId,
            }
            this.$http.get('/SelfTaskDetail/GetSelfAllTasks',{params:params}).then((result)=>{
                if(result.state == 1) {
                    this.taskList = result.data
                    this.taskList.forEach(item=>{
                        item.task_type_name = item.task_type == 1 ? '待考核' : item.task_type == 2 ? '待审核' : item.task_type == 3 ? '待自评' : item.task_type == 4 ? '待归档' :  item.task_type == 6 ? '方案退回修改' : ''
                        item.statue = item.task_type == 1 ? '0' : item.task_type == 2 ? '0' : item.task_type == 3 ? '0' : item.task_type == 4 ? '1' : item.task_type == 6 ? '2' : ''
                        item.elCard = item.task_count == 0 ? 'el_card' : 'el_card_life'
                        item.disabled = item.task_count == 0 ? false : true
                    })
                }
            }).catch(err=>{

            })
        },
        GetBranchYearlyAssessmentResult() {
            let params = {
                branch: this.$store.state.userData.CompanyId,
                year: new Date().getFullYear()
            }
            this.$http.get('/BranchAssessment/GetBranchYearlyAssessmentResult',{params:params}).then((result)=>{
                if(result.state == 1) {
                    this.seriesData1 = []
                    if(result.data.length > 0) {
                        result.data.forEach(item=>{
                            this.seriesData1.push(item.result)
                        })
                        this.lineTitle1 = result.data[0].title
                    }
                    this.drawLine1('main1')
                }
            }).catch(err=>{

            })
        },
        GetPersonalYearlyAssessmentResult() {
            let params = {
                person: this.$store.state.userData.UserId,
                year: new Date().getFullYear()
            }
            this.$http.get('/PersonalAssessment/GetPersonalYearlyAssessmentResult',{params:params}).then((result)=>{
                if(result.state == 1) {
                    this.seriesData2 = []
                    if(result.data.length > 0) {
                        result.data.forEach(item=>{
                            this.seriesData2.push(item.result)
                        })
                        this.lineTitle2 = result.data[0].title
                    }
                    this.drawLine2('main2')
                }
            }).catch(err=>{

            })
        },
        toTaskPage(row,index) {
            let list=this.$store.state.menuList;
            list.forEach(item=>{
                if(item.F_UrlAddress == '/judgeManagePerson') {
                    this.type = 1
                }
            })
            let name = ''
            let routerName = {}
            if(index == 1) { // 1,待审核
                name = this.type == 1 ? '/personVerifyManage' : '/filialeVerifyManage'
                routerName = this.type == 1 ?  {name:'/personVerifyManage',title:'个人考核方案审核管理',url:'/personVerifyManage'} : {name:'/filialeVerifyManage',title:'分公司考核方案审核管理',url:'/filialeVerifyManage'}
                this.$router.push({name:name,params:{templete_check_statue:row.statue}})
                this.$store.commit('setRouterName',routerName) 
            }else if(index == 3 && row.task_type_name == '方案退回修改') { // 4,方案退回修改
                name = this.type == 1 ? '/personJudgeManage' : '/filialeJudgeManage'
                routerName = this.type == 1 ?  {name:'/personJudgeManage',title:'个人考核方案管理',url:'/personJudgeManage'} : {name:'/filialeJudgeManage',title:'分公司考核方案管理',url:'/filialeJudgeManage'}
                this.$router.push({name:name,params:{templete_check_statue:row.statue}})
                this.$store.commit('setRouterName',routerName) 
            }else { // 待考核等
                name = this.type == 1 ? '/judgeManagePerson' : '/judgeManageFiliale'
                routerName = this.type == 1 ? {name:'/judgeManagePerson',title:'考核管理——个人',url:'/judgeManagePerson'} : {name:'/judgeManageFiliale',title:'考核管理——分公司',url:'/judgeManageFiliale'}
                this.$router.push({name:name,params:{assessment_statue:row.statue}})
                this.$store.commit('setRouterName',routerName) 
            }
        }
    },
    activated() {
        this.getAllTask()
        this.GetBranchYearlyAssessmentResult()
        this.GetPersonalYearlyAssessmentResult()
    }
}