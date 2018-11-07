export default {
    name: "judgePerson",
    data() {
        return {
            // type  0 分公司，1 个人
            type:'',
            assessment_id:'',
            check_object:'',// 分公司或个人id
            assessment_detail_statue:0,// 考核状态  1：考核中-考核，0：已完成-查看详情
            url1:'',
            url2:'',
            url3:'',
            indicators: [],
            activeName: 'first',
            tableData: [],
            titleName: '',
            checkerList:[],
            fixed_score:0,
            checker_weight:0,
            checker_result_list:[],
            // 总分
            sum_score:'',
        }
    },
    methods: {
        backPage() {
            let name = ''
            if(this.type == 0){
                name = '/judgeManageFilialeDetail'
            }else{
                name = '/judgeManagePersonDetail'
            }
            this.$router.push({name:name,params:{id:this.assessment_id,type:this.type,filing_people:this.$route.params.filing_people}})
        },
        // 获取考核指标
        getIndicators(type){
            let params = {
                assessment_id: this.assessment_id,
                check_object: this.check_object
            }
            this.$http.get(this.url1,{params:params}).then((result) => {
                this.indicators = result.data
                this.titleName = this.type == 0 ? this.indicators[0].assessment_name : (this.indicators[0].assessment_name + '——' + this.$route.params.personal_name)
            }).catch((err)=>{
            
            })
        },
        // 获取大表格
        getData(type){
            let params = {
                assessment_id: this.assessment_id,
                check_object: this.check_object
            }
            this.$http.get(this.url2,{params:params}).then((result) => {
                this.tableData = result.data
                this.tableData.forEach(item => {
                    if (item.formule.toString().indexOf('@') != 0){
                        this.fixed_score += Number(item.assessment_result)
                    }
                });
                this.fixed_score = +this.fixed_score.toFixed(2)
                this.$store.commit('changeToArray',this.tableData)
            }).catch((err)=>{
            
            })
        },
        // 合并单元格
        objectSpanMethod({ row, column, rowIndex, columnIndex }) {
            if (columnIndex === 0) {
              	if(row.span != 0) {
	                return {
	                    rowspan: row.span,
	                    colspan: 1
	                };
	            }else if(row.span == 0){
	               	return {
	                    rowspan: 0,
	                    colspan: 0
	                };
              	}
            }
        },
        // 获取底部考评人得分信息
        GetPersonalCheckerResultDetail() {
            let params = {
                assessment_id: this.assessment_id,
                check_object: this.check_object
            }
            this.$http.get('PersonalAssessment/GetPersonalCheckerResultDetail',{params:params}).then((result) => {
                if(result.state == 1) {
                    this.checkerList = []
                    result.data.forEach(item => {
                        this.checkerList.push({checker_name:item.checker_name,result:item.result,checker_weight:item.checker_weight,is_checked:item.is_checked,is_checked_true:item.is_checked_true,check_order:item.check_order}) 
                        if(item.is_checked_true) {
                            this.checker_weight = item.checker_weight
                        }
                    })
                    this.checkerList.sort(this.compare("check_order"))
                }
            }).catch((err)=>{
            
            })
        },
        formatter: function (row, column, cellValue) {
            if (row.formule.toString().indexOf('@') == 0){
                return '——'
            }else{
                return cellValue
            }
        },
        //定义一个排序函数
        compare(pro) { 
            return function(a,b){
                var value1 = a[pro];
                var value2 = b[pro];
                return value1 - value2;
            }
        },
        // 提交考核
        submit() {
            let params = {
                assessment_id: this.assessment_id,
                check_object: this.check_object,
                fixed_score: this.fixed_score,
                unfixed_score: this.unfixed_score,
                total_score: this.total_score,
                checker_weight: this.checker_weight,
                checker_result_list: JSON.stringify(this.checker_result_list),
            }
            this.$confirm('是否确认提交?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.tableData.forEach(item => {
                    if (item.formule.toString().indexOf('@') == 0){
                        this.checker_result_list.push({indicator_id:'',indicator_name:item.formule,indicator_value:item.assessment_result})
                        console.log(item.assessment_result)
                    }
                });
                let params = {
                    assessment_id: this.assessment_id,
                    check_object: this.check_object,
                    fixed_score: this.fixed_score,
                    unfixed_score: this.unfixed_score,
                    total_score: this.total_score,
                    checker_weight: this.checker_weight,
                    checker_result_list: JSON.stringify(this.checker_result_list),
                }
                this.$http.post(this.url3,params).then((result) => {
                    if(result.state == 1) {
                        this.$message({
                            type: 'success',
                            message: result.message
                        });
                        this.backPage()
                    }
                }).catch((err)=>{
                
                })
            }).catch(() => {
                this.$message({
                    type: 'info',
                    message: '已取消提交'
                });          
            });
        },
    },
    activated(){
        this.fixed_score = 0
        this.type = this.$route.params.type
        this.assessment_id = this.$route.params.assessment_id
        this.check_object = this.$route.params.check_object
        this.assessment_detail_statue = this.$route.params.assessment_detail_statue
        this.url1 = this.type == 0 ? '/BranchAssessment/GetBranchAssessmentTempResult' : '/PersonalAssessment/GetPersonalAssessmentTempResult'
        this.url2 = this.type == 0 ? '/BranchAssessment/GetBranchAssessmentTempleteTempResult' : '/PersonalAssessment/GetPersonalAssessmentTempleteTempResult'
        this.url3 = this.type == 0 ? '/BranchAssessment/CheckAssessment' : '/PersonalAssessment/CheckAssessment'
        if(!this.assessment_id){
            this.$router.push({name:this.$route.name.replace('Check','')})
        }else {
            this.getIndicators()
            this.getData()
            this.GetPersonalCheckerResultDetail()
        }
    },
    computed: {
        unfixed_score: function() {
            let score = 0
            this.tableData && this.tableData.forEach(item => {
                if(item.formule.toString().indexOf('@') == 0){
                    score += Number(item.assessment_result)
                }
            });
            let str = score.toFixed(2).toString()
            if(str[str.length - 1] == 0){
                score = Number(str.substr(0,str.length - 1))
            }
            this.checkerList.forEach(item=>{
                if(item.is_checked_true == 1) {
                    item.result = score
                }
            })
            return Number(score)
        },
        total_score: function() {
            let personScore = 0
            this.checkerList.forEach(item=>{
                personScore += Number(item.result)*Number(item.checker_weight)*0.01
            })
            personScore = (this.fixed_score  + personScore).toFixed(2)
            let str = personScore.toString()
            if(str[str.length - 1] == 0){
                personScore = Number(str.substr(0,str.length - 1))
            }
            return Number(personScore)
        },
    }
}