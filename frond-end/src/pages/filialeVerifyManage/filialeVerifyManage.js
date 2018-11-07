export default {
    data() {
        return {
            // 分公司0/个人1
            type:'',
            // 搜索参数
            tableSearch:{
                templete_name:'',
                creator_user_name:'',
                templete_check_statue:''
            },
            // 表格数据
            tableData:[],
            tableThead:[
                {name:'考核方案名称',value:'templete_name'},
                {name:'创建人',value:'creator_user_name'},
                {name:'职位名称',value:'creator_user_duty'},
                {name:'组织名称',value:'creator_user_org'},
                {name:'创建时间',value:'create_time'},
            ],
            // 审核状态
            templeteCheckStatueList:[
                {name:'审核中',id:0},
                {name:'通过',id:1},
                {name:'退回',id:2},
            ],
            // 翻页
            currentPage: 1,
            pageTotal: 10,
            total: 10,
            pageNumber: [10,25,50,100],
            timeOut: null,
        };
    },
    methods: {
        // 请求表格
        getData(){
            let defaultParams = {
                pagination:{
                    page: this.currentPage,
                    rows:this.pageTotal,
                    sidx:"id",
                    sord:"asc",
                    records:0,
                    total:0,
                },
                templete_type: this.type,
                checker: this.$store.state.userData.UserId
            }
            let params = Object.assign(defaultParams,this.tableSearch)
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/templete/GetDefaultTempleteCheck',{params:params}).then((result) => {
                    console.log(result);
                    this.tableData = result.data
                    this.total = result.total
                    this.tableData.forEach(item=>{
                        item.create_time = item.create_time.substr(0,10)
                    })
                }).catch((err)=>{
                
                })
            }, 0); 
        },
        // 翻页
        sizeChange(total){
            this.pageTotal = total
            this.getData()
        },
        currentChange(currentPage){
            this.currentPage = currentPage
            this.getData()
        },
        // 去详情页
        routerToDetail(item){
            console.log(item);
            let name = this.type?'/personVerifyManageDetail':'/filialeVerifyManageDetail'
            this.$router.push({name:name,params:{status:!item.flag,templete_id:item.templete_id}})
        },
        // 清除搜索条件
        resetForm(formName){
            this.$refs[formName].resetFields()
        }
    },
    activated(){
        this.type = this.$route.name == '/filialeVerifyManage'?0:1
        if(this.$route.params.templete_check_statue != undefined){
            this.tableSearch.templete_check_statue = Number(this.$route.params.templete_check_statue)
        }
        this.getData()
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
    }
}
