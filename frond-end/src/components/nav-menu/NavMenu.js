export default {
    name: 'navMenu',
    data:function() {
        return {
            routerNavslist:[],
            routerName: {},
            isCollapse:false,
            defaultActiveNow:''
        }
    },
    methods: {
        handleOpen(key, keyPath) {
            // console.log(key, keyPath);
        },
        handleClose(key, keyPath) {
            // console.log(key, keyPath);
        },
        add(routerName){
            this.routerName = routerName
            console.log(routerName)
            // this.$router.push({path:routerName.url})
            // 路由隐形传参
            this.$router.push({name:routerName.url,params:{id:routerName.url}})
            this.$store.commit('setRouterName',routerName)            
        },
        getData(){
            // console.log(url);
            
            setTimeout(()=>{
                this.routerNavslist = [
                    {
                        name:'一级菜单1',
                        children:[
                            {name:'/page1',url:'/page1',title:'页面1'},
                            {name:'/page3',url:'/page3',title:'页面3'},
                        ]
                    },
                    {
                        name:'考核任务管理',
                        children:[
                            {name:'/dashboard',url:'/dashboard',title:'首页'},                            
                            {name:'/filialeTask',url:'/filialeTask',title:'分公司任务分发管理'},
                            {name:'/personTask',url:'/personTask',title:'个人任务分发管理'},
                        ]
                    },
                    {
                        name:'考核管理',
                        children:[
                            {name:'/judgeManageFiliale',url:'/judgeManageFiliale',title:'考核管理-分公司'},
                            {name:'/judgeManagePerson',url:'/judgeManagePerson',title:'考核管理-个人'}
                        ]
                    },
                    {
                        name:'考核方案配置',
                        children:[
                            {name:'/page4',url:'/page4',title:'页面4'},
                            {name:'/indexDefinition',url:'/indexDefinition',title:'指标定义维护'},
                            {name:'/judgeMethod',url:'/judgeMethod',title:'考核办法维护'},
                            {name:'/judgeDimension',url:'/judgeDimension',title:'考核维度维护'},
                            {name:'/judgeTerm',url:'/judgeTerm',title:'考核细项及公式维护'},
                            {name:'/filialeJudgeManage',url:'/filialeJudgeManage',title:'分公司考核方案管理'},
                            {name:'/personJudgeManage',url:'/personJudgeManage',title:'个人考核方案管理'},
                        ]
                    },
                    {
                        name:'考核方案审核',
                        children:[
                            {name:'/filialeVerifyManage',url:'/filialeVerifyManage',title:'分公司考核方案审核管理'},
                            {name:'/personVerifyManage',url:'/personVerifyManage',title:'个人考核方案审核管理'},
                            // {name:'/filialeVerifyManageDetail',url:'/filialeVerifyManageDetail',title:'分公司考核方案-审核'},
                        ]
                    },
                ]
                
                // this.$store.commit('sendRouters',this.routerNavslist)
            },100)
        },
        getNavs(){
            let params = {
                roleId:this.userData.RoleId || ''
            }
            this.$http.get('/RoleAuthorize/GetMenuList',{params:params}).then((result) => {
                // console.log(result);
                this.changeData(result)
                this.$store.commit('saveMenuList',result)
                
            })
        },
        changeData(data){
            let tree = [];  // 存放转换后的数组
            let arr = [];   // 存放所有二级菜单
            let obj = {};   // 记录 tree 的 id
            let count = 0;  // 为 tree 的 id 记数
            if(!(data instanceof Array)){
                data = []
            }
            data.forEach((item) => {
                if(item.F_ParentId == '0'){
                    obj[item.F_Id] = count
                    count++;
                    tree.push({
                        name:item.F_FullName,
                        id:item.F_Id,
                        children:[]
                    })
                }else{
                    arr.push(item)
                }
            });
            arr.forEach((item)=>{
                tree[ obj[item.F_ParentId] ].children.push({
                    name:item.F_UrlAddress,
                    url:item.F_UrlAddress,
                    title:item.F_FullName,
                })
            })
            // console.log(tree);
            this.routerNavslist = tree;

            let url = this.$route.path.replace('Detail','').replace('Add','').replace('Check','')
            let activePage = {}
            this.routerNavslist.find((item)=>{
                activePage = item.children.find(list=>list.url == url)
                if(activePage){
                    return true
                }
            })
            setTimeout(() => {
                this.defaultActiveNow = activePage?activePage.url :'/dashboard'
            }, 0);
            // console.log(activePage);
            if(activePage){
                this.$store.commit('setRouterName',activePage)
            }
            
        },
        // 统一登录
        unifiedLogin(){
            this.$http.get('/Login/GetCurrent').then((result) => {
                console.log(result);
                // 保存用户信息到sessionStorage
                this.disabled = false
                if(result.state == 1){
                    this.$store.commit('saveUesrData',result.data)
                    this.$store.commit('resetRouterName')
                    sessionStorage.setItem('userData',Base64.encode(JSON.stringify(result.data)))
                    document.title = '首页'
                    let data = sessionStorage.getItem('userData')
                    let userData = JSON.parse(Base64.decode(data)) 
                    this.$store.commit('saveUesrData',userData)
                    this.$store.commit('setName',userData.UserName)
                    this.handleUserData()
                    this.$router.push({name:'/dashboard'})
                }
            }).catch((err)=>{
                console.log(err);
            })
        },
        handleUserData(){
            let data = sessionStorage.getItem('userData')
            data = data?Base64.decode(data):'{}'
            let userData = JSON.parse(data) || {}
            console.log(userData);
            this.userData = userData 
            this.getNavs()
            this.getButtons()
        },
        getButtons(){
            let obj = {}
            let params = {
                roleId:this.userData.RoleId || ''
            }
            this.$http.get('/RoleAuthorize/GetButtonList',{params:params}).then((result) => {
                // console.log(result);
                if(!(result instanceof Array)){
                    result = []
                }
                
                result.forEach((item)=>{
                    obj[item.F_UrlAddress] = true
                })
                this.$store.commit('saveButtonAuthority',obj)
                // console.log(obj);
                
            })
        }
    },
    created(){
        this.unifiedLogin()
        return
        // let data = sessionStorage.getItem('userData')
        // if(!data){
        //     this.unifiedLogin()
        //     return
        // }
        // let userData = JSON.parse(Base64.decode(data)) || {}
        // this.$store.commit('saveUesrData',userData)
        // this.name = userData.UserName
        // this.$store.commit('resetRouterName')
        // document.title = '首页'
        // this.$store.commit('setName',userData.UserName)
        // this.handleUserData()
    },
    watch:{
        '$route'(){
            // console.log(this.$route);
            this.defaultActiveNow = this.$route.path.replace('Detail','').replace('Add','').replace('Check','')
        }
    }
}