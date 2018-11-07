export default {
    data() {
        return {
            // 搜索参数
            tableSearch:{
                detail_name:'',
            },
            // 表格数据
            tableData:[],
            tableThead:[
                {name:'细项名称',value:'detail_name'},
                {name:'指标公式',value:'formule'},
                {name:'考核办法',value:'check_method_define'},
                // {name:'使用状态',value:'statue_name'},
            ],
            // 弹窗
            tableObject:{
                detail_name:'',
                formule:'',
                method_id:''
            },
            check_methods:[{id:'1',count_method_name:'客家话2',check_method_id:'2'}],
            definitions:[],
            dialogVisible:false,
            // 弹窗 - 必填项校验
            rules:{
                detail_name:[
                    {required:true,message:'请输入细项名称',trigger:'blur'},
                    { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' }
                ],
                formule:[
                    {required:true,message:'请输入指标公式',trigger:'blur'},
                    { min: 1, max: 100, message: '长度在 1 到 100 个字符', trigger: 'blur' }
                ],
                method_id:[{required:true,message:'请选择考核办法',trigger:'change'}],
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
            let defaultParams = {
                pagination:{
                    page: this.page.start_index,
                    rows:this.page.total,
                }
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/dimension/GetDimensionDetail',{params:params}).then((result) => {
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
        // 请求考核办法
        getCheckMethod(){
            let params = {
                pagination:{
                    rows:0,
                    page:0
                },
                check_method_statue:1
            }
            this.$http.get('/checkmethod/getcheckmethod',{params:params}).then((result) => {
                console.log(result);
                this.check_methods = result.data || []
            }).catch((err)=>{
            
            })
        },
        // 请求指标定义
        getDefinition(){
            let params = {
                pagination:{
                    page: 1,
                    rows: 0,
                },
                statue:1
            }
            this.$http.get('/indicators/getindicators',{params:params}).then((result) => {
                console.log(result)
                this.definitions = result.data
            }).catch((err)=>{
                
            })
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
            let statue = item.statue == 0 ? 1 : 0;
            let statue_name = statue == 1 ? '启用' : '停用';
            let params = {
                id: item.id,
                statue: statue
            }
            this.$confirm('请确认是否' + statue_name + '?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.$http.post('/dimension/ChangeDimensionDetailStatue',params).then((result) => {
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
            // 校验指标公式
            // 校验是否有特殊符号
            // 校验括号是否成对
            let reg1 = /[\(]/g
            let reg2 = /[\)]/g
            let reg3 = /[\+|\-|\\*|\/|∮|÷|㏒|㏑|∫|∑|π|ˆ]+/g
            let s = this.tableObject.formule
            // let s = '1+2+--×*÷/123㏒㏑123∫234∮÷234∫∑πˆ'
            let match1 = s.match(reg1) || [];
            let match2 = s.match(reg2) || [];
            let match3 = s.match(reg3) || [];
            let flag = true;
            if(match1.length != match2.length){
                this.$message({
                    type: 'error',
                    message: '括号数量不匹配'
                });
                return
            }
            for(let i = 0;i<match3.length;i++){
                if(match3[i].length>1){
                    flag = false
                    break 
                }
            }
            if(!flag){
                this.$message({
                    type: 'error',
                    message: '符号使用不正确'
                });
                return
            }
            console.log(match3);
    
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    console.log('已全部填完');
                    this.addForm()
                } else {
                  console.log('error submit!!');
                  return false;
                }
            });
        },
        // 确认新增
        addForm(){
            let params = this.tableObject
            this.$http.post('/dimension/AddDimensionDetail',params).then((result) => {
                console.log(result);
                if(result.state == 1){
                    this.$message({
                        type: 'success',
                        message: '新增成功!'
                    });
                    this.dialogVisible = false
                    this.getData()
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
    activated(){
        this.getDefinition()
        this.getCheckMethod()
    },
    watch:{
        currentPage(){
            this.page.start_index = this.currentPage
            this.getData()
        }
    }
}