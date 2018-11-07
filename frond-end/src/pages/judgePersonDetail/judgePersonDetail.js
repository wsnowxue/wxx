export default{
    name: 'judgeFilialeDetail',
    data() {
        return {
            type: '',// 0:分公司 1：个人
            // 搜索参数
            tableSearch:{
                personal_name:'',
                telphone:'',
                assessment_detail_statue:'',
            },
            // 表格数据
            tableData:[],
            reUploadOpen:false,
            file_name: '',
            disabled: false,
            // 表头数据
            tableThead:[
                {name:'员工姓名',value:'personal_name'},
                {name:'手机号码',value:'telphone'},
                {name:'所属组织',value:'department'},
                {name:'考核起止时间',value:'start_end_time'},
                {name:'绩效考核名称',value:'assessment_name'},
                {name:'考核方案名称',value:'templete_name'},
                // {name:'考核状态',value:'assessment_statue_detail'},
            ],
            // 弹窗
            outerVisible:{flag:false},
            // 分页条
            currentPage: 1,
            pageTotal: 10,
            total: 10,
            pageNumber: [10,25,50,100],
            timeOut:null,
            file_name: [],
            // 判断是否隐藏操作列
            flag:false,
            finished:false,
            // 排序
            sidx:''
        }
    },
    methods: {
        resetForm(formName) {
            this.$refs[formName].resetFields()
        },
        handleExceed(files, fileList) {
          this.$message.warning(`当前限制选择 1 个文件`);
        },
        // 退回
        deleteTask(item) {
            this.$confirm('请确认是否退回?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
                }).then(() => {
                    this.getData()
                    this.$message({
                        type: 'success',
                        message: '退回成功!'
                    });
                }).catch(() => {
                this.$message({
                    type: 'info',
                    message: '已取消退回'
                });          
            });
        },
        // 改变每页条数
        sizeChange(total){
            this.pageTotal = total
            this.getData()
        },
        // 翻页
        currentChange(currentPage){
            this.currentPage = currentPage
            this.getData()
        },
        // 请求表格
        getData(){
            let defaultParams = {
                pagination:{
                    rows: this.pageTotal,
                    page: this.currentPage,
                    sidx: 'score',
                    sord: this.sidx || '',
                    records: 0,
                    total: 0,
                },
                assessment_id: this.$route.params.id
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/PersonalAssessment/GetPersonalAssessmentDetail',{params:params}).then((result) => {
                    this.flag = result.data[0].assessment_detail_statue != 0?true:false
                    this.finished = result.data[0].finished ?true:false
                    this.tableData = result.data
                    this.total = result.total
                    this.tableData.forEach(item=>{
                        item.start_end_time = item.start_time.substr(0,10) + ' 至 ' + item.end_time.substr(0,10)
                    })
                }).catch((err)=>{
                
                })
            }, 0); 
        },
        getData2(column){
            if (column.order == 'ascending') {
                this.sidx = 'asc'
            }else if (column.order == "descending") {
                this.sidx = 'desc'
            }else{
                this.sidx = ''
            }
            this.getData()            
        },
        // 导出数据
        download(){
            let params = {
                assessment_id:this.$route.params.id,
                type:this.type
            }
            window.open(`/BranchAssessment/ExportResultGroupByOrg?assessment_id=${this.$route.params.id}&type=${this.type}`)
        },
        // 跳转
        toJudgeManageFilialeCheck(item) {
            let name = ''
            if(this.type == 0) {
                name = '/judgeManageFilialeCheck'
            }else if(this.type == 1) {
                name = '/judgeManagePersonCheck'
            }
            this.$router.push({
                name:name,
                params:{
                    assessment_id:item.assessment_id,
                    check_object:item.personal_id,
                    assessment_detail_statue:item.assessment_detail_statue,
                    personal_name:item.personal_name,
                    type:this.type,
                    filing_people:this.$route.params.filing_people
                }
            })
        },
        // 上传成功的回调
        uploadSuccess(result) {
            if(result.state == 1) {
                this.file_name = result.data[0].destfilename
                this.disabled = true
            }
        },
        // 删除文件
        removeUpload() {
            this.disabled = false
            this.$refs.upload.clearFiles()
        },
        // 重新导入指标量值
        ReAssessment() {
            let params = {
                assessment_id: this.$route.params.id,
                file_name: this.file_name
            }
            this.$http.post('/PersonalAssessment/ReLaunchPersonalAssessment',params).then((result) => {
                if(result.state == 1) {
                    this.$message({
                        type: 'success',
                        message: result.message
                    });
                    this.reUploadOpen = false
                }
            }).catch((err)=>{
            
            })
        },
        closeDialog() {
            this.file_name = ''  // 清空上传文件列表
            this.removeUpload()
        },
    },
    created() {
    },
    activated(){
        this.type = this.$route.params.type;
        if(!this.$route.params.id){
            this.$router.push({name:this.$route.name.replace('Detail','')})
        }else{
            this.getData()
        }
    },
    watch:{
        currentPage(){
            this.getData()
        }
    }
}