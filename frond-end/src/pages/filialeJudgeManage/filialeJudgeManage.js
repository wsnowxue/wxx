export default {
    data() {
        return {
            // 分公司/个人
            type:1,
            // 搜索参数
            tableSearch:{
                templete_name:'',
                templete_check_statue:'',
                statue:'',
            },
            // 表格数据
            tableData:[],
            tableThead:[
                {name:'考核方案名称',value:'templete_name'},
                {name:'创建人',value:'creator_user_name'},
                {name:'创建时间',value:'create_time'},
                // {name:'审核状态',value:'templete_check_statue_name'},
                // {name:'启用状态',value:'statue_name'},
            ],
            // 审核状态
            templeteCheckStatueList:[
                {name:'审核中',id:0},
                {name:'通过',id:1},
                {name:'退回',id:2},
            ],
            // 使用状态
            statueList:[
                {name:'启用',id:1},
                {name:'停用',id:0},
            ],
            // 翻页
            page:{
                start_index:1,
                total:10
            },
            currentPage:1,
            total:10,
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
                },
                templete_type:this.type
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/templete/GetDefaultTemplete',{params:params}).then((result) => {
                    // console.log(result);
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
        openChangeStatue(item){
            let statue_name = !item.statue ? '启用' : '停用';
            let params = {
                templete_id: item.templete_id,
                statue: !item.statue
            }
            this.$confirm('请确认是否' + statue_name + '?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.$http.post('/templete/ChangeTempletestatue',params).then((result) => {
                    // console.log(result);
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
        // 去新增页
        routerToAdd(type,row){
            let name = this.type?'/personJudgeManageAdd':'/filialeJudgeManageAdd'
            let params = {}
            type == 'change'?params = {templete_id:row.templete_id}:{}
            this.$router.push({name:name,params:params})
        },
        // 去详情页
        routerToDetail(item){
            console.log(item);
            let name = this.type?'/personJudgeManageDetail':'/filialeJudgeManageDetail'
            this.$router.push({name:name,params:{templete_id:item.templete_id,status:true}})
        },
        // 清除搜索条件
        resetForm(formName){
            this.$refs[formName].resetFields()
        }
    },
    created(){
        this.getData()
    },
    activated(){
        this.type = this.$route.name == '/filialeJudgeManage'?0:1
        if(this.$route.params.templete_check_statue != undefined){
            this.tableSearch.templete_check_statue = Number(this.$route.params.templete_check_statue)
        }
        if(this.$route.query){
            this.getData()
        }
    },
    filters:{
        filterCheckStatue(value){
            if(value == 0){
                return '审核中'
            }
            if(value == 1){
                return '通过'
            }
            if(value == 2){
                return '退回'
            }
        }
    },
    watch:{
        currentPage(){
            this.page.start_index = this.currentPage
            this.getData()
        }
    }
}