export default {
    data() {
        var checkerObj = (rule, value, callback) => {
          if (this.checker_object_list.length == 0) {
            return callback(new Error('考核对象不能为空'));
          }else {
            return callback();
          }
        };
        var checker_list_rule = (rule, value, callback) => {
          if (this.checker_list.length == 0) {
            return callback(new Error('考评人不能为空'));
          }else {
            return callback();
          }
        };
        var score_weight = (rule, value, callback) => {
            if(this.checker_list.length == 0) {
              return callback(new Error('权重不能为空'));
            }else {
                this.checker_list.forEach(item=>{
                  if(!item.checker_weight || item.checker_weight < 0) {
                    return callback(new Error('权重不能为空且必须大于0'));
                  }
                })
            }
        };
        var filling_people_rule = (rule, value, callback) => {
          if (!this.tableObject.filling_people) {
            return callback(new Error('考评结果审核人不能为空'));
          }else {
            return callback();
          }
        };
        return {
            data: [],
            defaultProps: {
              children: 'ChildNodes',
              label: 'text'
            },
            type: '',// 0：发起考核-分公司-显示部分项, 1：发起考核-个人-显示全部项
            addUrl:'',// 发起考核接口
            sourceChecker: 1,// 1，考评人 2，考核结果审核人
            innerVisible: false,// 二级添加考评人弹框
            chosePerson: false,// 二级选择考评对象树状图弹框
            choseButton: false, // 选择考核对象显示
            gridData: [], // 考评人列表
            filling_people_name:'',// 审核人姓名
            templeteList:[],// 下载考核方案
            chosenChecker: [], // 选中的考核对象（页面展示姓名）
            checker_list: [],// 选中的考评人集合
            checker_object_list: [],// 需要传给后台的选中的考核对象id
            disabled:false, // 防止重复提交
            disabled2:true, // 防止空点下载
            // 表单对象
            tableObject:{
                assessment_name:'',
                start_time: '',
                end_time: '',
                start_end_time: [],
                templete_id: '',// 绩效考核模板
                // filling_people: '',// 选中的考核结果审核人
                filling_people: this.$store.state.userData.UserId,// 选中的考核结果审核人
            }, 
            rules: {
                assessment_name: [
                    { required: true, message: '请输入绩效考核名称', trigger: 'blur' },
                    { min: 1, max: 50, message: '长度在 1 到 50 个字符', trigger: 'blur' }
                ],
                start_end_time: [
                    { type: 'array', required: true, message: '请选择日期', trigger: 'change' }
                ],
                checkerObj: [
                    { validator:checkerObj, required: true, trigger: 'blur' }
                ],
                templete_id: [
                    { required: true, message: '请选择绩效考核方案', trigger: 'change' }
                ],
                checker_list_rule: [
                    { validator: checker_list_rule, required: true, trigger: 'blur' }
                ],
                filling_people_rule: [
                    { validator: filling_people_rule, required: true, trigger: 'blur' }
                ],
            }
        };
    },
    props:['obj'], // 组件传值
    methods: {
        // 获取绩效考核下拉
        getTemplete() {
            let params = {
                pagination:{
                    page: 0,
                    rows: 0,
                    sidx: "id",
                    sord: "asc",
                    records: 0,
                    total: 0,
                },
                templete_type:this.type,
                statue: 1, // 只有启用状态的模板才能下载
            }
            this.$http.get('/templete/getDefaultTemplete',{params:params}).then((result) => {
                this.templeteList = result.data
            }).catch((err)=>{
            
            })
        },
        // 获取是否需要自评 方案下载 change
        templateChange(templete_id) {
            if(this.type == '1') {
                this.disabled2 = this.tableObject.templete_id && this.checker_object_list.length > 0 ? false : true
            }else {
                this.disabled2 = this.tableObject.templete_id ? false : true
            }
            let params = {
                templete_id:templete_id
            }
            this.GetBranchCheckerList()
            this.type == 1 && this.$http.get('/PersonalAssessment/GetPersonalAssessmentTempleteIsNeedSelfCheck',{params:params}).then((result) => {
                this.tableObject.need_self_check = result.data ? 1 : 0
            }).catch((err)=>{
            })
        },
        // 点击获取选中的考核对象
        getCheckedKeys() {
            this.chosenChecker = []
            this.checker_object_list = []
            let getCheckedKeys = this.$refs.tree.getCheckedNodes()
            console.log(getCheckedKeys);
            
            getCheckedKeys.forEach(item=>{
                if(item.nodeDepth == '4') {
                    this.chosenChecker.push({id:item.id,name:item.text})
                    this.checker_object_list.push(item.id)
                }
            })
            this.choseButton = true
            this.chosePerson = false
            if(this.type == '1') {
                this.disabled2 = this.tableObject.templete_id && this.checker_object_list.length > 0 ? false : true
            }
        },
        // 删除考评人
        deleteExaminer(index) {
            this.checker_list.splice(index,1);
        },
        // 获取分公司默认考评人
        getDefaultChecker() {
            this.$http.get('/BranchAssessment/GetBranchDefaultChecker').then((result) => {
                result.data.forEach(item=>{
                    this.checker_list.push({'checker_id':item.F_Id,'checker_name':item.F_RealName ,'checker_order':0,'checker_weight':'0'})
                })
            }).catch((err)=>{
            
            })
        },
        // 获取个人 考评人列表
        GetBranchCheckerList() {
            let params = {
                templete_id:this.tableObject.templete_id,
                type:this.type
            }
            this.$http.get('/DealAssessmentManage/GetCheckerList',{params}).then((result) => {
                this.gridData = result.data
            }).catch((err)=>{
            
            })
        },
        // 考评人单选
        handleCurrentClick(val) {
            this.innerVisible = false;
            // 添加考评人 一 个人不能重复/结果审核人 
            if(this.sourceChecker == 1) {
                let flag = true
                this.checker_list.forEach((item,index)=>{
                    if(item.checker_id == val.id) {
                        flag = false
                        return
                    }
                    if(item.department_name == val.department_name) {
                        flag = false
                        this.$confirm(`已有相同部门,是否用 ${val.name} 替换 ${item.checker_name}`).then(() => {
                            this.checker_list.splice(index,1,{'checker_id':val.id,'checker_name':val.name,'checker_order':0,'checker_weight':'0','department_name':val.department_name})
                        }).catch(() => {});
                    }
                })
                flag && this.checker_list.push({'checker_id':val.id,'checker_name':val.name,'checker_order':0,'checker_weight':'0','department_name':val.department_name})

            }else {
                this.tableObject.filling_people = val.id
                this.filling_people_name = val.name
            }
        },
        // 获取个人考核的组织架构树形图 
        getTreeJson() {
            let params = {
                organize_id: this.$store.state.userData.CompanyId
            }
            this.$http.get('/User/GetTreeJson',{params:params}).then((result) => {
                this.data = result
            }).catch((err)=>{
            
            })
        },
        // 清除上次所填数据
        resetForm(){
            this.filling_people_name = '' // 审核人姓名
            this.chosenChecker = []  // 选中的考核对象
            this.data = []
            this.checker_list = [] // 选中的考评人集合
            this.checker_object_list = [] // 考核对象
            this.disabled = false //再次打开弹框可以提交
            this.disabled2 = true // 禁止绩效考核模板空点下载
            this.tableObject.assessment_name = '' 
            this.tableObject.start_time = '' 
            this.tableObject.end_time = '' 
            this.tableObject.start_end_time = [] 
            this.tableObject.templete_id = '' // 绩效考核模板
            // this.tableObject.filling_people = '' // 选中的考核结果审核人
            if(this.$refs.tableObject){
                setTimeout(() => {
                    this.$refs.tableObject.resetFields()
                }, 0);
            }
        },
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
            if (valid) {
                console.log(this.checker_list)
                this.disabled = true
                this.tableObject.start_time = this.tableObject.start_end_time[0]
                this.tableObject.end_time = this.tableObject.start_end_time[1]
                
                this.tableObject.checker_object_list = JSON.stringify(this.checker_object_list)
                this.tableObject.checker_list = JSON.stringify(this.checker_list)
                let params = JSON.parse(JSON.stringify(this.tableObject))
                // delete params.checker_object_list
                delete params.start_end_time
                
                this.$http.post(this.addUrl,params).then((result) => {
                    if(result.state == 1) {
                        this.$message({
                            type: 'success',
                            message: result.message
                        });
                        this.$emit('refreshList')
                        this.obj.flag = false
                    }else{
                        this.disabled = false
                    }
                }).catch((err)=>{
                    this.disabled = false
                })
            } else {
              console.log('error submit!!')
              return false;
            }
          });
        },
        openDialog() {
            // this.type == '0' && this.getDefaultChecker()
            this.getTemplete()
            this.type && this.getTreeJson()
        },
        closeDialog() {
            this.resetForm()
        },
    },
    activated() {
        this.type = this.$route.name == '/judgeManageFiliale' ? '0' : '1';
        this.addUrl = this.type == '1' ? '/PersonalAssessment/LaunchPersonalAssessment' : '/BranchAssessment/LaunchBranchAssessment'
    },
    
}