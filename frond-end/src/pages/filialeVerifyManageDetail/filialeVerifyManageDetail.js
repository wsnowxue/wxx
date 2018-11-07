export default {
    data() {
        return {
            type: '',
            // 查看详情参数
            checkDetails:{
                templete_name:'',
                create_time:'',
                creator_user_name:'',
                templete_check_statue:'',
                back_reason:''
            },
            // 搜索参数
            tableSearch:{
                check_result: '',
                check_suggest: ''    
            },
            // 表格数据
            tableData:[],
            // 细则
            details:[],
            // flag为 true 时，为详情。false时为考核
            flag:true,
            // 校验规则
            rules:{
                check_result:{ required: true, message: '请选择审批结果', trigger: 'change' },
                check_suggest:[
                    { required: true, message: '请填写退回原因', trigger: 'blur'},
                    { min: 1, max: 20, message: '退回原因字数不得超过20字', trigger: 'blur'}
                ]
            }

            
        };
    },
    methods: {
        backPage(){
            this.$router.push({name:this.$route.name.replace('Detail','')})
        },
        submitTable(formName){
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$confirm('提交后将无法修改, 是否继续?', '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        let defaultParams = {
                            templete_id: this.$route.params.templete_id,
                            // check_templete_id: this.tableData[0]?this.tableData[0].id:'',
                            checker: this.$store.state.userData.UserId
                        }
                        let params = Object.assign(defaultParams,this.tableSearch)
                        this.$http.post('/templetecheck/PostTempleteCheckResult',params).then((result) => {
                            if(result.state == 1){
                                this.$message({
                                    type: 'success',
                                    message: '提交成功!'
                                });
                                let name = this.type?'/personVerifyManage':'/filialeVerifyManage'
                                this.$router.push({name:name})
                            }
                        }).catch((err)=>{
                        
                        })
                    }).catch(() => {
                        this.$message({
                            type: 'info',
                            message: '已取消提交'
                        });          
                    });
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        getData(templete_id){
            let params = {
                templete_id:templete_id
            }
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/templete/gettempletedetail',{params:params}).then((result) => {
                    console.log(result);
                    this.tableData = result.data
                    this.checkDetails = result.data[0]
                    console.log(this.checkDetails);
                    
                    this.$store.commit('changeToArray',this.tableData)
                    this.getDetail()
                }).catch((err)=>{
                
                })
            }, 0); 
        },
        // 获取细项指标及其指标公式
        getDetail(){
            let params = {
                pagination:{
                    rows:10,
                    page:0,
                    sidx:"id",
                    sord:"asc"
                }
            }
            this.$http.get('/dimension/GetdimensionDetail',{params:params}).then((result) => {
                this.details = result.data;
                this.tableData.forEach((item,index)=>{
                    this.details.forEach((list)=>{
                        if(list.id == item.detail_id){
                            item.detail_obj = list
                        }
                    })
                })
                this.tableData = this.tableData.splice(0)
            })
        },
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
    },
    activated(){
        this.tableSearch = {
            check_result: '',
            check_suggest: ''    
        }
        this.type = this.$route.name == '/filialeVerifyManageDetail'?0:1
        this.flag = this.$route.params.status
        // this.flag = 1
        if(!this.$route.params.templete_id){
            this.backPage()
        }
        this.getData(this.$route.params.templete_id)
    },
    filters:{
        filterCheck(value){
            return value == 1?'通过':'退回'
        }
    }
}