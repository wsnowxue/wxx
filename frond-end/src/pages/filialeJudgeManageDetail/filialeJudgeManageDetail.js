export default {
    data() {
        return {
            // 分公司0/个人1
            type:'',
            flag:0,
            tableSearch:{
                templete_id:''
            },
            templete_name:'',
            checkDetails:{
                templete_name:'',
                create_time:'',
                creator_user_name:'',
                templete_check_statue:'',
                back_reason:''
            },
            tableData3: [],
            page:{
                start_index:1,
                total:100
            },
            // 细则
            details:[],
            currentPage:1,
            total:10,
            timeOut:null,
            pageNumber:[10,25,50,100]
            
        }
        
    },
    methods: {
        getData(templete_id){
            let params = {
                templete_id:templete_id
            }
            clearTimeout(this.timeOut)
            this.timeOut = setTimeout(() => {
                this.$http.get('/templete/gettempletedetail',{params:params}).then((result) => {
                    console.log(result);
                    this.tableData3 = result.data
                    this.templete_name = result.data[0].templete_name
                    this.checkDetails = result.data[0]
                    this.$store.commit('changeToArray',this.tableData3)
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
                console.log(result);
                this.details = result.data;
                this.tableData3.forEach((item,index)=>{
                    this.details.forEach((list)=>{
                        if(list.id == item.detail_id){
                            item.detail_obj = list
                        }
                    })
                })
                this.tableData3 = this.tableData3.splice(0)
                console.log(this.tableData3);
                
                
                // this.tableData3 = result.data
            })
        },
        objectSpanMethod({ row, column, rowIndex, columnIndex }) {
            if (columnIndex === 0 || columnIndex === 1) {
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
        // 返回
        backPage(){
            let name = this.type?'/personJudgeManage':'/filialeJudgeManage'
            this.$router.push({name:name})
        }
    },
    activated(){
        this.tableSearch.templete_id = this.$route.params?this.$route.params.templete_id:''
        this.tableSearch.templete_id?'':this.backPage()
        this.getData(this.tableSearch.templete_id)
        this.type = this.$route.name == '/filialeJudgeManageDetail'?0:1
        this.flag = this.$route.params.status
        console.log(this.$route.params);
    },
    filters:{
        filterCheck(value){
            return value == 1?'通过':'退回'
        }
    }
}