export default {
    data() {
        return {
            // 搜索参数
            tableSearch:{
                dimension_name:'',
            },
            // 表格数据
            tableData:[],
            tableThead:[
                // {name:'维度名称',value:'dimension_name'},
                // {name:'启用状态',value:'statue'},
            ],
            // 弹窗
            tableObject:{
                dimension_name:'',
                department:'',
            },
            dialogVisible:false,
            // 弹窗 - 必填项校验
            rules:{
                dimension_name:[
                    {required:true,message:'请输入维度名称',trigger:'blur'},
                    // { min: 1, max: 10, message: '长度在 1 到 10 个字符', trigger: 'blur' }
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
            pageNumber:[10,20,50,100]
        };
    },
    methods: {
        // 请求表格
        getData(type){
            let defaultParams = {
                pagination:{
                    page: this.page.start_index,
                    rows:this.page.total,
                }
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/dimension/Getdimension',{params:params}).then((result) => {
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
        openChangeStatue(item) {
            let statue_name = !item.statue? '启用' : '停用';
            let params = {
                id: item.id,
                statue: !item.statue
            }
            this.$confirm('请确认是否' + statue_name + '?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.$http.post('/dimension/Changedimensionstatue',params).then((result) => {
                    console.log(result);
                    if(result.state == 1) {
                        this.$message({
                            type: 'success',
                            message: statue_name + '成功!'
                        });
                        this.getData()
                    }
                }).catch((err)=>{
                   
                })
            }).catch(() => {
                this.$message({
                type: 'info',
                message: '已取消' + statue_name
                });          
            });
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
            let params = {...this.tableObject}
            delete params.selectedDepartments
            this.$http.post('/dimension/Adddimension',params).then((result) => {
                console.log(result);
                if(result.state == 1) {
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
                this.tableObject.selectedDepartments = []
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