import examiner from '@/components/examiner/examiner.vue';
export default {
    name: "pages",
    data:function(){
        return {
            // type  0 集团考核分公司，1 分公司考核个人
            type: '',
            url: '',
            url1: '', // 归档url
            // 搜索参数
            tableSearch:{
                assessment_name:'',
                templete_name:'',
                assessment_statue:'',
            },
            // 表格数据
            tableData:[],
            // 表头数据
            tableThead:[
                {name:'绩效考核名称',value:'assessment_name'},
                {name:'考核方案名称',value:'templete_name'},
            ],
            // 分页条
            currentPage: 1,
            pageTotal: 10,
            total: 10,
            pageNumber: [10,25,50,100],
            timeOut: null,
            // 弹窗
            outerVisible: {flag: false},
            // saveResult: false, //归档权限
            // 当前行的id
            assessment_id:'',
            // 查看上传进度
            seeProcess:false,
            // 未上传的人
            unuploadBranch:'',
        }
    },
    components: {
        examiner
    },
    methods:{
        resetForm(formName) {
            this.$refs[formName].resetFields()
        },
        // resetExaminerForm() {
        //     this.$refs.examiner.resetForm() 父组件调用子组件的方法
        // },
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
                    sidx:"",
                    sord:"",
                    records:0,
                    total:0,
                }
            }
            // this.tableSearch.assessment_statue = typeof this.$route.params.assessment_statue == 'undefined' ? this.tableSearch.assessment_statue : this.$route.params.assessment_statue
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get(this.url,{params: params}).then((result) => {
                    this.tableData = result.data
                    this.total = result.total
                    this.tableData.forEach(item=>{
                        item.start_end_time = item.start_time.substr(0,10) + ' 至 ' + item.end_time.substr(0,10);
                        if(item.assessment_statue == '0') {
                            item.assessment_statue_name = '上传中'
                        }else if(item.assessment_statue == '1') {
                            item.assessment_statue_name = '待归档'
                        }else {
                            item.assessment_statue_name = '已归档'
                        }
                        item.saveResult = item.filing_people == this.$store.state.userData.UserId ? true : false
                    })
                }).catch((err)=>{
                    console.log(err);
                })
            },0)
        },
        // 归档弹框
        SaveBranchAssessmentResult(item) {
            this.$confirm('请确认是否归档?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
                }).then(() => {
                    this.$http.post(this.url1,{assessment_id:item.id}).then((result) => {
                        if(result.state == 1){
                            this.getData()
                            this.$message({
                                type: 'success',
                                message: '归档成功!'
                            });
                        }                        
                    }).catch((err)=>{
                        console.log(err);
                    })
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消归档'
                    });          
            });
        },
        toJudgeFilialeDetail(item) {
            let name = ''
            if(this.type == 0) {
                name = '/judgeManageFilialeDetail'
            }else if(this.type == 1) {
                name = '/judgeManagePersonDetail'
            }
            this.$router.push({name:name,params:{id:item.id,type:this.type,assessment_sponsor_id:item.assessment_sponsor_id}})
        },
        // 下载
        download(rows){
            console.log(rows);
            let assessment_id = rows.id;
            let start_time = rows.start_time;
            let end_time = rows.end_time;
            let assessment_type = -1;
            let templete_type = this.type;
            let templete_id = rows.templete_id;
            let assessment_name = rows.assessment_name;
            let baseUrl = '/DealAssessmentManage/DownLoadAssessmentTemplete'
            let params = `?assessment_id=${assessment_id}&start_time=${start_time}&end_time=${end_time}&assessment_type=${assessment_type}&templete_type=${templete_type}&templete_id=${templete_id}&assessment_name=${assessment_name}`
            window.open(baseUrl + params)
        },
        handleSelectionChange(val){
            console.log(val);
        },
        // 上传成功的回调
        uploadSuccess(result) {
            console.log(result)
            let obj = result.data[0]
            if(result.state == 1) {
                let params = {
                    assessment_id:this.assessment_id,
                    file_name:obj.destfilename,
                    type:this.type
                }
                // console.log(params);
                let url = '/DealAssessmentManage/UploadAssessmentData'
                this.$http.post(url,params).then((result) => {
                    if(result.state == 1){
                        this.getData()
                        this.$message({
                            type: 'success',
                            message: '上传成功!'
                        });
                    }else{
                        console.log(result);
                    }                     
                }).catch((err)=>{
                    console.log(err);
                })
            }
        },
        uploadData(row){
            this.assessment_id = row.id
            console.log(row);
            
        },
        removeUpload(){},
        // 查看上传进度
        checkProcess(item){
            let params = {
                assessment_id:item.id,
            }
            let url = '/DealAssessmentManage/GetAssessmentState'
            this.$http.get(url,{params:params}).then((result) => {
                if(result.state == 1){
                    // 这里，后台会把部门，人名，状态，信息全都给出来，需要哪些，前端自己过滤未上传的字段
                    // [{organize_name:'xx部门',uploader:'张三',upload_state:0,state_msg:'未上传'}]
                    let filterData = result.data.filter(item => item.upload_state == 0)
                    this.unuploadBranch = filterData.map(item => item.uploader).join('、')
                    this.seeProcess = true
                }else{
                    console.log(result);
                }                     
            }).catch((err)=>{
                console.log(err);
            })
        },
        // 关闭弹窗
        closeDialog(){
            
        },
    },
    activated() {
        this.type = this.$route.name == '/judgeManageFiliale' ? 0:1;
        this.url = this.type == 0 ? '/BranchAssessment/GetBranchAssessmentOverview' : '/PersonalAssessment/GetPersonalAssessmentOverview'
        this.url1 = this.type == 0 ? '/BranchAssessment/SaveBranchAssessmentResult' : '/PersonalAssessment/SavePersonalAssessmentResult'
        if(this.$route.params.assessment_statue != undefined){
            this.tableSearch.assessment_statue = this.$route.params.assessment_statue
        }
        this.getData()
        !this.outerVisible.flag && this.getData()
    },
    watch:{
        currentPage(){
            this.getData()
        }
    }
};