export default {
    name: 'filialeTaskDetail',
    data() {
        return {
            type: '',// 0：分公司 1：个人
            typeFlag:'',
            tableDataYearly:[],
            // tableFinanical:[],
            // tableDataBank:[],
            tableDataNewCar:[],
            tableDataOldCar:[],
            activeName: 'first1',
            // 年度任务指标 - 表头
            yearTask:[
                // {name:'分公司',value:'task_object_name'},
                {name:'全年任务指标',value:'task_yearly'},
                {name:'1月任务分解数',value:'task_Jan'},
                {name:'2月任务分解数',value:'task_Feb'},
                {name:'3月任务分解数',value:'task_Mar'},
                {name:'4月任务分解数',value:'task_Apr'},
                {name:'5月任务分解数',value:'task_May'},
                {name:'6月任务分解数',value:'task_Jun'},
                {name:'7月任务分解数',value:'task_Jul'},
                {name:'8月任务分解数',value:'task_Aug'},
                {name:'9月任务分解数',value:'task_Sep'},
                {name:'10月任务分解数',value:'task_Oct'},
                {name:'11月任务分解数',value:'task_Nov'},
                {name:'12月任务分解数',value:'task_Dec'},
            ],
            // 金融产品 - 表头
            // financeProduct:[
            //     {name:'分公司',value:'task_object_name'},
            //     {name:'业务量（万）',value:'traffic'},
            //     {name:'传统新车（万）',value:'traditional_new_car'},
            //     {name:'易融贷（万）',value:'YiRong_loan'},
            //     {name:'公牌私贷（万）',value:'public_credit'},
            //     {name:'车主贷（万）',value:'car_loaner'},
            //     {name:'融资租赁（万）',value:'finanical_leasing'},
            //     {name:'二手车（万）',value:'second_hand_car'},
            // ],
            // 银行分类 - 表头
            // bankType:[
            //     {name:'分公司',value:'task_object_name'},
            //     {name:'业务量（万）',value:'traffic'},
            //     {name:'宁波工行（万）',value:'ICBC_NB'},
            //     {name:'安徽工行（万）',value:'ICBC_AH'},
            //     {name:'建设银行（万）',value:'CBC'},
            //     {name:'广东工行（万）',value:'ICBC_GD'},
            //     {name:'温州银行（万）',value:'Bank_WZ'},
            //     {name:'其他银行（万）',value:'other_bank'},
            //     {name:'融资租赁（万）',value:'Financial_Lean'},
            // ]
        }
    },
    methods: {
        backPage(){
            if(this.type == '0') {
                this.$router.push({path:'/filialeTask'})
            }else if(this.type == '1') {
                this.$router.push({path:'/personTask'})
            }
        },
        // 请求表格
        getDataYearly(params){
            this.$http.get('/task/GetYearlyTask',{params: params}).then((result) => {
                result.data.forEach((item,i) => {
                    if(item.templete_type == 1) {
                        this.tableDataYearly = item.detail
                    }else if(item.templete_type == 4) {
                        this.tableDataNewCar = item.detail
                    }else if(item.templete_type == 5) {
                        this.tableDataOldCar = item.detail
                    }
                });
                if(!result.data.length){
                    this.tableDataYearly = []
                    this.tableDataNewCar = []
                    this.tableDataOldCar = []
                }
            }).catch((err)=>{
                console.log(err);
            })
        },
        // getDataFinanical(params){
        //     this.$http.get('/task/GetFinanicalProductsTask',{params: params}).then((result) => {
        //         this.tableFinanical = result.data
        //     }).catch((err)=>{
        //         console.log(err);
        //     })
        // },
        // getDataBank(params){
        //     this.$http.get('/task/GetCoopertionBankTask',{params: params}).then((result) => {
        //         this.tableDataBank = result.data
        //     }).catch((err)=>{
        //         console.log(err);
        //     })
        // },
    },
    activated(){
        if(!this.$route.params.id){
            this.$router.push({name:this.$route.name.replace('Detail','')})
        }else{
            this.type = this.$route.params.type
            this.type?this.typeFlag = '客户经理':this.typeFlag = '分公司'
            let params = {
                pagination:{
                    rows: 0,
                    page: 0,
                    sidx: "id",
                    sord: "asc",
                    records: 0,
                    total: 0,
                },
                task_type: this.$route.params.type,
                task_id: this.$route.params.id,
            }
            this.getDataYearly(params);
            // this.type == 0 && this.getDataFinanical(params);
            // this.type == 0 && this.getDataBank(params);
        }
    }
}
