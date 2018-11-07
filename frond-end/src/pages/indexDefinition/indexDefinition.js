export default {
    data() {
        return {
            // 搜索参数
            tableSearch:{
                indicator_name:'',
                indicator_define:'',
                indicator_count_method:''
            },
            // 统计方式
            countMethod:[],
            // 表格数据
            tableData:[],
            tableThead:[
                {name:'指标名称',value:'indicator_name'},
                {name:'指标定义',value:'indicator_define'},
                // {name:'统计方式',value:'indicator_count_method'},
                // {name:'启用状态',value:'statue_name'},
            ],
            // 弹窗
            tableObject:{
                indicator_name:'',
                indicator_define:'',
                indicator_count_method:''
            },
            dialogVisible:false,
            // 弹窗 - 必填项校验
            rules:{
                indicator_name:[
                    {required:true,message:'请输入指标名称',trigger:'blur'},
                    { min: 1, max: 100, message: '长度在 1 到 100 个字符', trigger: 'blur' }
                ],
                indicator_define:[
                    {required:true,message:'请输入定义',trigger:'blur'},
                    { min: 1, max: 50, message: '长度在 1 到 50 个字符', trigger: 'blur' }
                ],
                indicator_count_method:[{required:true,message:'请选择统计方式',trigger:'blur'}],
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
                this.$http.get('/indicators/getindicators',{params:params}).then((result) => {
                    console.log(result);
                    this.tableData = result.data
                    this.total = result.total
                    this.tableData.forEach(item=>{
                        if(item.statue == '0') {
                            item.statue_name =  '停用'
                        }else {
                            item.statue_name = '启用'
                        }
                    })
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
        // 请求指标统计方式
        getCountMethod(){
            this.$http.get('/indicators/getallcountmethod').then((result) => {
                console.log(result);
                this.countMethod = result.data
            }).catch((err)=>{
               
            })
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
                this.$http.post('/indicators/changeindicatorsstatue',params).then((result) => {
                    console.log(result);
                    if(result.state == 1) {
                        this.$message({
                            type: 'success',
                            message: statue_name +'成功!'
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
            let params = this.tableObject
            this.$http.post('/indicators/addindicators',params).then((result) => {
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
            }
        }
    },
    created(){
        this.getData()
        this.getCountMethod()
    },
    filters:{
        filterMethod(value,countMethod){
            let name = ''
            countMethod.forEach(item=>{
                item.id == value?name = item.count_method_name:''
            })
            return name
        }
    },
    watch:{
        currentPage(){
            this.page.start_index = this.currentPage
            this.getData()
        }
    }
}