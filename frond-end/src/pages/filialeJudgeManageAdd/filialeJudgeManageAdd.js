export default {
    data() {
        return {
            // 分公司0/个人1
            type:'',
            tableDetail:{
                templete_name:'',
                allow_beyond_limit:0
            }, // 顶部
            tableData3: [],
            // 部门
            departments: [],
            //   弹窗
            dialogVisible:false,
            dialogCheckPerson:false,
            innerVisible:false,
            tableObject: {
                dimension_id:'',
                detail_id:'',
                base_score:'',
                formule:'',
                // detail_obj:{}
            },
            tableObject2:{

            },
            rules:{
                dimension_id:[{required:true,message:'请选择考核维度',trigger:'change'}],
                detail_id:[{required:true,message:'请选择细项指标',trigger:'change'}],
            },
            // 审核人
            checker_list:[],
            // 审核人列表
            gridData:[],
            // 考核维度
            dimensions: [],
            // 细则
            details:[]
        }
        
    },
    methods: {
        getData(templete_id){
            console.log(this.type);
            
            let params = {
                templete_type: this.type
            }
            let url = '/templete/GetTempleteList'
            if(templete_id){
                params.templete_id = templete_id
                url = '/templete/GetTempleteDetail'
            }
            
            this.$http.get(url,{params:params}).then((result) => {
                console.log(result);
                this.tableData3 = result.data || []
                // this.tableDetail.templete_name = result.data[0].templete_name
                this.tableDetail.templete_name = this.$route.params.templete_id?result.data[0].templete_name:''
                this.tableDetail.allow_beyond_limit = result.data[0].allow_beyond_limit?true:false
                // this.checker_list = {'checker_id':result.data[0].creator_user_id,'checker_name':result.data[0].creator_user_name || '','checker_order':0,'checker_weight':''}
                console.log(result.data[0].allow_beyond_limit);
                
                
                // 查询考核维度
                this.getDimension()
                // 查询细项指标及其指标公式
                this.getDetail()
            }).catch((err)=>{
                console.log(err);
                // 查询考核维度
                this.getDimension()
                // 查询细项指标及其指标公式
                this.getDetail()
            })
        },
        // 请求部门
        getDepartment(){
            let url = '/Organize/GetTreeSelectJsonNew'
            let data = ''
            this.$http.get(url).then((result) => {
                this.departments = result.data || []
            }).catch((err)=>{
            
            })
        },
        // 获取考核维度
        getDimension(){
            let params = {
                pagination:{
                    rows:0,
                    page:0,
                    // sidx:"dimension_name",
                    // sord:"asc"
                },
                statue:1
            }
            this.$http.get('/dimension/Getdimension',{params:params}).then((result) => {
                console.log(result);
                this.dimensions = result.data

                this.$store.commit('changeToArray',this.tableData3)
            }).catch((err)=>{
                console.log(err);
            })
        },
        // 获取细项指标及其指标公式
        getDetail(){
            let params = {
                pagination:{
                    rows:0,
                    page:0,
                    sidx:"id",
                    sord:"asc"
                },
                statue:1
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
        // 获取考核办法
        getJudgeMethods(){
            let params = {
                pagination:{
                    rows:0,
                    page:0,
                    sidx:"indicator_name",
                    sord:"asc"
                }
            }
            this.$http.get('/indicators/getindicators',{params:params}).then((result) => {
                console.log(result);
                if(result.state == 1){
                    this.judgeMethods = result.data

                }else{
                    this.judgeMethods = []
                }
            })
        },
        changeRow(row){
            this.details.forEach((item)=>{
                if(item.id == row.detail_id){
                    row.detail_obj = item
                }
            })
            console.log(row.detail_obj);
            
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
        addProject(formName){
            // this.dialogVisible = true
            console.log(this.tableObject);
            // 故采用下面这种简单粗暴的方式解决
            // 选择框校验问题：因为elementUI版本问题，导致选过之后，第一次重置，会再次校验选择框，但第二次重置不会，而且两次重置必须分开触发
            // 所以只能将下一次触发放到 setTimeout
            this.resetForm(formName)
            setTimeout(() => {
                this.resetForm(formName)
            }, 0);
        },
        changeDimension(obj,index){
            console.log(obj);
            console.log(index);
            
            let length = this.tableData3.length;
            for (let i = index+1; i < length; i++) {
                if(this.tableData3[i] && this.tableData3[i].span){
                    break
                }else{
                    this.tableData3[i].dimension_id = obj.dimension_id
                }
            }

            this.$store.commit('changeToArray',this.tableData3)
        },
        deleteRow(index,row){
            if(this.tableData3[index].span!=0 && this.tableData3[index+1] && !this.tableData3[index+1].span){
                this.tableData3[index+1].span = this.tableData3[index].span-1
            }else{
                let spanNumber = -1;
                spanNumber = this.tableData3.find((value)=>{
                    if(value.dimension_id == row.dimension_id && value.span!=0){
                        return true
                    }else{
                        return false
                    }
                })
                spanNumber.span && spanNumber.span--
            }
            this.tableData3.splice(index,1)
            this.$store.commit('changeToArray',this.tableData3)
        },
        checkDialog(formName){
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    console.log('已全部填完');
                    this.submitForm(formName)
                } else {
                  console.log('error submit!!');
                  return false;
                }
            });
        },
        // 弹窗表单
        submitForm(formName) {
            
            let dimension_id = this.tableObject.dimension_id
            let detail_id = this.tableObject.detail_id
            let base_score = this.tableObject.base_score
            let formule = this.tableObject.formule
            let check_method_name = this.tableObject.check_method_name
            let item = this.tableData3.find(item=>item.dimension_id == dimension_id)
            if(item){
                item.span++;
                let index = this.tableData3.findIndex(item=>item.dimension_id == dimension_id)
                this.tableData3.splice(index+1,0,{
                    dimension_id: dimension_id,
                    detail_id: detail_id,
                    base_score: base_score,
                    detail_obj:{
                        formule: formule,
                        check_method_name: check_method_name,
                    },
                    span:0
                })
            }else{
                this.tableData3.push({
                    dimension_id: dimension_id,
                    detail_id: detail_id,
                    base_score: base_score,
                    detail_obj:{
                        formule: formule,
                        check_method_name: check_method_name,
                    },
                    span:1
                })
            }
            this.dialogVisible = false
            console.log(this.tableData3);
        },
        resetForm(formName) {
            if(this.$refs[formName]){
                this.$refs[formName].resetFields();
            }
        },
        detailChange(value){
            this.details.forEach(item=>{
                if(item.id == value){
                    // this.tableObject.indicator_formula_name = item.
                    this.tableObject.detail_name = item.detail_name
                    this.tableObject.formule = item.formule
                    this.tableObject.check_method_name = item.check_method_name
                }
            })
        },
        // 删除审核人
        deleteExaminer(index) {
            this.checker_list.splice(index,1);
        },
        // 获取审核人列表
        getCompanyCheckerList() {
            let url = '';
            if(this.type == 1){
                url = '/User/GetBranchCheckerList'
            }else{
                url = '/User/GetCompanyCheckerList'
            }
            this.$http.get(url).then((result) => {
                this.gridData = result.data
            }).catch((err)=>{
            
            })
        },
        // 审核人多选
        handleCurrentClick(val) {
            this.innerVisible = false;
            let flag = true
            this.checker_list.forEach(item=>{
                if(item.checker_id == val.id) {
                    flag = false
                    return
                }
            })
            flag && this.checker_list.push({'checker_id':val.id,'checker_name':val.name,'checker_order':0})
        },
        // 选择审核人
        selectPerson(){
            console.log(this.tableData3);
            this.dialogCheckPerson = true;
            // this.checker_list.push({'checker_id':this.tableData3[0].checker_id || '','checker_name':this.tableData3[0].checker_name || '+','checker_order':0})

            this.getCompanyCheckerList()
        },
        // 提交总表单
        submitTable(){
            if(!this.tableDetail.templete_name){
                this.$message({
                    type: 'error',
                    message: '请填写考核方案名称'
                });
                return
            }
            if(this.checker_list.length==0){
                this.$message({
                    type: 'error',
                    message: '请先选择审核人'
                });
                return
            }
            
            // 判断是否有重复
            if(this.checkJudgeFlag()){
                this.$message({
                    type: 'error',
                    message: '考核维度重复!请重新选择！'
                });
                return
            }
            // 判断是否选择部门
            if(this.checkDepartment()){
                this.$message({
                    type: 'error',
                    message: '请选择部门！'
                });
                return
            }
            if(this.checkDetailsFlag()){
                this.$message({
                    type: 'error',
                    message: '细项指标重复!请重新选择！'
                });
                return
            }
            // 判断是否有哪项的基础分值为0
            if (this.checkScore0()) {
                this.$message({
                    type: 'error',
                    message: '基础分值不能为 0'
                });
                return
            }
            // 判断是否为100分
            if(this.checkScore()){
                this.$message({
                    type: 'error',
                    message: '总分必须为100！'
                });
                return
            }
            
            this.checker_list.forEach((item,index)=>{
                item.checker_order = index+1
            })
            let allow_beyond_limit = this.tableDetail.allow_beyond_limit?1:0;
            let creator_id = this.$store.state.userData.UserId || 0;
            // todo
            // let checker_id = this.checker_list
            let templete_name = this.tableDetail.templete_name || ''
            let arr = [];
            this.tableData3.forEach(item=>{
                arr.push({
                    templete_name: templete_name,
                    templete_type: this.type,
                    dimension_id: item.dimension_id ||'',
                    detail_id: item.detail_id ||'',
                    base_score: item.base_score,
                    allow_beyond_limit: allow_beyond_limit,
                    org: item.org_id,
                    // creator_id: this.$store.state.userData.UserId
                    creator_id: creator_id,
                    // checker_id: checker_id,
                })
            })
            console.log(arr);
            let params = {}
            let url = ''
            let templete_id = this.$route.params.templete_id
            if(templete_id){
                url = '/templete/UpdateTemplete'
                params = {
                    updateTempleteList:JSON.stringify(arr),
                    checkerList: JSON.stringify(this.checker_list),
                    templete_id: templete_id
                }
            }else{
                url = '/templete/addtemplete'
                params = {
                    addTempleteList:JSON.stringify(arr),
                    checkerList: JSON.stringify(this.checker_list)
                }
            }
            this.$http.post(url,params).then((result) => {
                console.log(result);
                if(result.state == 1){
                    this.$message({
                        type: 'success',
                        message: templete_id?'考核方案修改成功。':'考核方案添加成功。'
                    });
                    this.innerVisible = false
                    this.dialogCheckPerson = false
                    this.$router.push({name:this.$route.path.replace('Add','')})
                }
            }).catch((err)=>{
                console.log(err);
            })

            // console.log(this.tableDetail);
            
            // console.log(this.tableData3);
        },
        closeDialog() {
            this.checker_list = [];
        },
        // 判断是否有相同的考核维度
        checkJudgeFlag(){
            let obj = {}
            this.tableData3.forEach(item=>{
                if(obj[item.dimension_id] && item.span){
                    obj[item.dimension_id]++
                }
                if(!obj[item.dimension_id] && item.span){
                    obj[item.dimension_id] = 1
                }
            })
            for (const key in obj) {
                if (obj[key]>1) {
                    return true
                }
            }
            console.log(obj);
            
            return false
        },
        // 判断是否选择部门
        checkDepartment(){
            this.tableData3;
            let array = this.tableData3
            for (let i = 0; i < array.length; i++) {
                if(array[i].span && !array[i].org_id){
                    return true
                }
                if(!array[i].span && !array[i].org_id) {
                    array[i].org_id = array[i-1].org_id
                }
            }
            console.log(array.map(item=>item.org_id));
            return false
        },     
        
        
        // 判断是否有相同的细项指标
        checkDetailsFlag(){
            let arr1 = this.tableData3
            let arr2 = this.tableData3
            let flag = false
            arr1.forEach((item,index)=>{
                arr2.forEach((list,i)=>{
                    if(item.dimension_id == list.dimension_id 
                        && item.detail_id == list.detail_id 
                        && index != i){
                        flag = true
                    }
                })
            })
            return flag
        },
        // 总分是否为100
        checkScore(){
            let sum = 0
            this.tableData3.forEach(item=>{
                sum+=item.base_score || 0
            })
            return sum != 100
        },
        // 基础分值是否为0
        checkScore0(){
            return this.tableData3.find(item=>item.base_score == 0)
        },
        
        // 返回
        backPage(){
            let name = this.type?'/personJudgeManage':'/filialeJudgeManage'
            this.$router.push({name:name})
        }
        
    },
    computed:{
        tableData4(){
            return this.tableData3
        }
    },
    activated:function(){
        // this.tableData3 = []
        // this.tableDetail = {}
        // this.dialogVisible = false;
        // this.tableObject = {}
        // this.$store.commit('toTree',this.tableData3)
        this.getDepartment()
        this.type = this.$route.name == '/filialeJudgeManageAdd'?0:1
        if(this.$route.params.templete_id){
            this.getData(this.$route.params.templete_id)
        }else{
            this.tableDetail={
                templete_name:'',
                allow_beyond_limit:0
            }
            this.getData()
        }
        
        console.log(`切换到所在路由的时候，触发${this.count++}次`);
    }
}