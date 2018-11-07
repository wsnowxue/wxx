export default {
    name: 'filialeTask',
    data() {
        return {
            type: '',// 0:分公司任务 1：个人任务
            // 搜索表单
            tableSearch:{
                task_name:'',
                task_statue:'',
            },
            UserId:'',
            fileList:[],
            // 弹窗表单   
            ruleForm: {
                task_name: '',
                templete_type:'',
                chose_time: [],
                start_time: '',
                end_time: '',
                task_type: '', 
                file_list: [], 
                // file1: '',
                // file2: '',
                // file3: '',
            },
            fileNum: 0,
            // 表格内容
            tableData: [],
            // 表头
            tableThead: [
                {name:'任务名称',value:'task_name'},
                {name:'考核起止时间',value:'start_end_assessment_time'},
                {name:'任务分发人',value:'task_sponsor_name'},
                {name:'任务分发状态',value:'task_statue_name'},
                {name:'创建时间',value:'create_time'},
            ],
            // 分页条
            currentPage: 1,
            pageTotal: 10,
            total: 10,
            pageNumber: [10,25,50,100],
            timeOut: null,
            dialogVisible:false,
            dialogVisible2:false,
            // fileLimit: 1, //个人1，公司3
            disabled:false,// 防止重复提交
            disabled2:true,// 防止空点下载
            rules: {
                task_name: [
                    { required: true, message: '请输入任务名称', trigger: 'blur' },
                    { min: 1, max: 50, message: '长度在 1 到 50 个字符', trigger: 'blur' }
                ],
                chose_time: [
                    { type: 'array', required: true, message: '请选择日期', trigger: 'change' }
                ]
            },  
            // 存储需要上传的行
            item:{},
            multipleSelection:[]
        }
    },
    methods: {
        resetForm(formName) {
            this.$refs[formName].resetFields()
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
                    sidx:"create_time",
                    sord:"desc",
                    records:0,
                    total:0,
                },
                task_type: this.type,
                user_id: this.UserId,
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/task/GetTask',{params: params}).then((result) => {
                    this.tableData = result.data || []
                    this.total = result.total
                    this.tableData.forEach(item=>{
                        if(item.task_distribute_statue == 0) {
                            item.task_statue_name = '未分发'
                        }else {
                            item.task_statue_name = '已分发'
                        }
                        item.start_end_assessment_time = item.assessment_start_time.substr(0,10) + ' 至 ' + item.assessment_end_time.substr(0,10)
                    })
                }).catch((err)=>{
                    console.log(err);
                })
            }, 0); 
        },
        // 删除
        deleteTask(item) {
            this.$confirm('请确认是否删除?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
                }).then(() => {
                    this.$http.post('/task/DeleteTask',{task_id:item.id}).then((result) => {
                        if(result.state == 1){
                            this.getData()
                            this.$message({
                                type: 'success',
                                message: '删除成功!'
                            });
                        }                        
                    }).catch((err)=>{
                        console.log(err);
                    })
                }).catch(() => {
                this.$message({
                    type: 'info',
                    message: '已取消删除'
                });          
            });
        },
        // 分发
        sendTask(item) {
            this.$confirm('请确认是否分发?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
                }).then(() => {
                    this.$http.post('/task/UpdateTaskStatue',{task_id:item.id}).then((result) => {
                        if(result.state == 1){
                            this.getData()
                            this.$message({
                                type: 'success',
                                message: '分发成功!'
                            });
                        }
                    }).catch((err)=>{
                        console.log(err);
                    })
                }).catch(() => {
                this.$message({
                    type: 'info',
                    message: '已取消分发'
                });          
            });
        },
        // 模板下拉 change
        changeTemplete(templete_type) {
            console.log(templete_type)
            this.disabled2 = templete_type ? false : true
        },
        // 下载
        download1() {
            window.location='/task/DownTaskTemplete?templete_type=1&task_type='+this.type
        },
        download4() {
            window.location='/task/DownTaskTemplete?templete_type=4&task_type='+this.type
        },
        download5() {
            window.location='/task/DownTaskTemplete?templete_type=5&task_type='+this.type
        },
        exportTask(){

            let arr = this.multipleSelection.map(item=>{
                return {taskId:item.id,taskName:item.task_name}
            })
            let flag = this.multipleSelection.find(item => item.task_distribute_statue ==0)
            console.log(arr);
            console.log(flag);
            if (!arr.length) {
                this.$message({
                    type: 'error',
                    message: '请选择需要导出的数据'
                });
                return
            }
            if(flag){
                this.$message({
                    type: 'error',
                    message: '未分发的数据不能导出'
                });
                return
            }
            if(this.type == 0){
                window.open(`/Task/ExportTaskTarget?taskInfos=${JSON.stringify(arr)}`)
            }else{
                window.open(`/Task/ExportPersonalTaskTarget?taskInfos=${JSON.stringify(arr)}`)
            }
        },
        // 已选中的行
        handleSelectionChange(val){
            this.multipleSelection = val
        },
        toFilialeTaskDetail(item) {
            if(this.type == '0') {
                this.$router.push({name:'/filialeTaskDetail',params:{id:item.id,type:this.type}})
            }else if(this.type == '1') {
                this.$router.push({name:'/personTaskDetail',params:{id:item.id,type:this.type}})
            }
        },
        // 上传任务 
        // todo 为清除上一次选择的
        openDialog2(row){
            this.ruleForm.file_list = []
            this.dialogVisible2 = true;
            this.removeUpload()
            this.item = row
        },
        upLoadTask(){
            let flag = false;
            this.ruleForm.file_list.length == 0?flag = true:''
            let arr = this.ruleForm.file_list.map(item=>{
                if(!item.file_name){
                    flag = true
                }
                return {
                    file_name:item.file_name,
                    templete_type:1,
                    originalfilename:item.originalfilename
                }
            })
            if (arr.length > 1) {
                arr[1].templete_type = 4
                arr[2].templete_type = 5
            }
            let params = {
                taskId:this.item.id,
                task_type:this.type,
                file_list:arr,
            }
            if(this.type == 0 && arr.length != 3){
                flag = true
            }
            if(flag){
                this.$message({
                    type: 'error',
                    message: '请先上传数据'
                });
                return
            }
            const loading = this.$loading({
                lock: true,
                text: '上传中，请稍后',
                background: 'rgba(0, 0, 0, 0.5)'
            })
            console.log(JSON.stringify(params));
            params.file_list = JSON.stringify(params.file_list)
            this.$http.post('/Task/UpLoadTask',params).then((result) => {
                console.log(result);
                if (result.state == 1) {
                    this.dialogVisible2 = false
                    this.$message({
                        type: 'success',
                        message: result.message
                    });
                }else{
                    this.$message({
                        type: 'error',
                        message: result.message
                    });
                }
                
                loading.close()
            }).catch((err)=>{
                console.log(err);
                loading.close()
            })
            
        },
        removeUpload(num){
            this.ruleForm.file_list.splice(num,1)
        },
        // uploadSuccess(result) {
        //     if(result.state == 1) {
        //         this.fileList = result.data
        //         console.log(result.data)
        //         this.fileNum ++
        //         if(this.fileNum == 1) {
        //             this.ruleForm.file1 = result.data[0].destfilename
        //             this.ruleForm.file2 = ''
        //             this.ruleForm.file3 = ''
        //         }else if(this.fileNum == 2) {
        //             this.ruleForm.file2 = result.data[0].destfilename
        //             this.ruleForm.file3 = ''
        //         }else if(this.fileNum == 3) {
        //             this.ruleForm.file3 = result.data[0].destfilename
        //         }
        //     }
        // },
        uploadSuccess1(result) {
            if(result.state == 1) {
                this.fileList = result.data
                this.ruleForm.file_list[0] = {file_name:result.data[0].destfilename,templete_type:1,originalfilename:result.data[0].originalfilename}
            }
        },
        uploadSuccess4(result) {
            if(result.state == 1) {
                this.fileList = result.data
                this.ruleForm.file_list[1] = {file_name:result.data[0].destfilename,templete_type:4,originalfilename:result.data[0].originalfilename}
            }
        },
        uploadSuccess5(result) {
            if(result.state == 1) {
                this.fileList = result.data
                this.ruleForm.file_list[2] = {file_name:result.data[0].destfilename,templete_type:5,originalfilename:result.data[0].originalfilename}
            }
        },
        // 提交任务
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
            if (valid) {
                this.disabled = true
                this.ruleForm.start_time = this.ruleForm.chose_time[0]
                this.ruleForm.end_time = this.ruleForm.chose_time[1]
                this.ruleForm.task_type = this.type
                this.ruleForm.creator_user_id = this.UserId
                // this.ruleForm.file_list = JSON.stringify(this.ruleForm.file_list)
                let addParams = this.ruleForm;
                let params = {
                    task_name:this.ruleForm.task_name,
                    start_time:this.ruleForm.chose_time[0],
                    end_time:this.ruleForm.chose_time[1],
                    task_type:this.type,
                    creator_user_id:this.UserId,
                }
                console.log(addParams)
                this.$http.post('/task/addTask',params).then((result) => {
                    if(result.state == 1) {
                        this.getData()
                        this.$message({
                            type: 'success',
                            message: result.message
                        });
                        this.dialogVisible = false
                    }else {
                        this.disabled = false
                    }
                }).catch((err)=>{
                    this.disabled = false
                    console.log(err)
                })
            } else {
              console.log('error submit!!')
              return false;
            }
          });
        },
        // 删除文件
        removeUpload() {
            if(this.$refs.upload1){
                this.$refs.upload1.clearFiles();
            }
            if(this.type == 0) {
                if(this.$refs.upload4){
                    this.$refs.upload4.clearFiles();
                }
                if(this.$refs.upload5){
                    this.$refs.upload5.clearFiles();
                }
            }
        },
        // removeFile() {
        //     this.fileNum --
        //     if(this.fileNum == 0) {
        //         this.ruleForm.file1 = ''
        //         this.ruleForm.file2 = ''
        //         this.ruleForm.file3 = ''
        //     }else if(this.fileNum == 1) {
        //         this.ruleForm.file2 = ''
        //         this.ruleForm.file3 = ''
        //     }else if(this.fileNum == 2) {
        //         this.ruleForm.file3 = ''
        //     }
        // },
        // 清除弹出框上次所填数据
        resetFormOpen(){
            this.ruleForm = {
                task_name: '',
                templete_type:'',
                chose_time: [],
                start_time: '',
                end_time: '',
                task_type: '', 
                file1: '',
                file2: '',
                file3: '',
            }
            this.fileNum = 0
            this.disabled2 = true
            this.disabled = false
            // this.removeUpload()
            if(this.$refs.ruleForm){
                setTimeout(() => {
                    this.$refs.ruleForm.resetFields()
                }, 0);
            }
        },
        closeDialog() {
            this.resetFormOpen()
        },
    },
    activated() {
        this.type = this.$route.name == '/filialeTask' ? 0 : 1
        // this.fileLimit = this.type == 0 ? 3 : 1 
        this.UserId = this.$store.state.userData.UserId
        this.getData()
    },
    watch:{
        currentPage(){
            this.getData()
        }
    }
}
