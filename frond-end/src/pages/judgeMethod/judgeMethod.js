export default {
    data() {
        return {
            // 搜索参数
            tableSearch:{
                check_method_name:'',
                check_method_define:'',
            },
            // 表格数据
            tableData:[
            ],
            tableThead:[
                {name:'考核办法名称',value:'check_method_name'},
                {name:'考核办法',value:'check_method_define'},
                // {name:'启用状态',value:'statue'},
            ],
            // 弹窗
            tableObject:{
                check_method_name:'',
                check_method_define:'',
                check_method_proc:''
            },
            dialogVisible:false,
            // 弹窗 - 必填项校验
            rules:{
                check_method_name:[
                    {required:true,message:'请输入考核办法名称',trigger:'blur'},
                    { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' }
                ],
                check_method_define:[
                    {required:true,message:'请输入考核办法',trigger:'blur'},
                    { min: 1, max: 100, message: '长度在 1 到 100 个字符', trigger: 'blur' }
                ],
            },
            // 翻页
            page:{
                start_index:1,
                total:10
            },
            currentPage:1,
            total:0,
            timeOut:null,
            pageNumber:[10,25,50,100]
        };
    },
    methods: {
        // 请求表格
        getData(type){
            console.log(this.currentPage);
            
            let defaultParams = {
                pagination:{
                    page: this.page.start_index,
                    rows:this.page.total,
                }
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/checkmethod/getcheckmethod',{params:params}).then((result) => {
                    console.log(result);
                    this.tableData = result.data
                    this.total = result.total
                }).catch((err)=>{
                
                })
            }, 0); 
        },
        // 翻页
        sizeChange(total){
            this.page.total = total
            this.getData()
        },
        currentChange(currentPage){
            this.page.start_index = currentPage
            this.getData()
        },
        // 启用/停用
        changeStatue(item){
            let statue_name = !item.check_method_statue ? '启用':'停用'
            let params = {
                check_method_id:item.id,
                check_method_statue:!item.check_method_statue
            }
            this.$confirm('请确认是否' + statue_name + '?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.$http.post('/checkmethod/changecheckmethodstatue',params).then((result) => {
                    console.log(result);
                    if(result.state == 1){
                        this.$message({
                            type: 'success',
                            message: statue_name + '成功!'
                        });
                        this.getData()
                    }
                }).catch((err)=>{
                
                })
            })
        },
        // 新增校验
        submitForm(formName){
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    console.log('已全部填完');
                    this.addForm(formName)
                } else {
                  console.log('error submit!!');
                  return false;
                }
            });
        },
        // 确认新增
        addForm(formName){
            let params = this.tableObject
            this.$http.post('/checkmethod/addcheckmethod',params).then((result) => {
                console.log(result);
                if(result.state == 1){
                    this.$message({
                        type: 'success',
                        message: '新增成功!'
                    });
                    this.dialogVisible = false
                    this.getData()
                    this.resetForm(formName)
                }
            }).catch((err)=>{
                
            })
        },
        // 清除搜索条件
        resetForm(formName){
            if(this.$refs[formName]){
                this.$refs[formName].resetFields()
            }
        }
    },
    created(){
        this.getData()
    },
    watch:{
        currentPage(){
            this.page.start_index = this.currentPage
            this.getData()
        }
    }
}