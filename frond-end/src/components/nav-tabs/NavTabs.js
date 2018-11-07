export default {
    name: 'navTabs',
    data() {
        return {
            activeTab: '',
            editableTabs2: [{title:'首页',name:'/dashboard',url:'/dashboard'}],
            tabIndex: 0,
        }
    },
    methods: {
        // 点自己，跳转到对应的路由的页面
        jumpToTab() {
           this.$router.push({name:this.activeTab})
           let arr = this.$store.state.navTabs;
           let activePage = arr.find(item => item.url == this.activeTab)
           this.$store.commit('setRouterName',activePage)
        },
        // 移除tab
        removeTab(targetName) {
            // console.log(this.activeTab);
            let obj;
            let arr = this.$store.state.navTabs;
            // 判断是否为当前页
            console.log(1);
            
            if (targetName == this.activeTab) {
                let index = arr.findIndex(item=>item.url == targetName)
                obj = arr[index+1] || arr[index-1];
                setTimeout(() => {
                    // this.activeTab = obj.url
                    this.$router.push({name:obj.url})
                }, 0);
            }else{
                // this.$router.push({path:targetName})
            }
            this.$store.commit('deleteRouterName',targetName)
            
        }
        
    },
    watch:{
        '$route'(){
            this.activeTab = this.$route.path.replace('Detail','').replace('Add','').replace('Check','')
            let arr = this.$store.state.navTabs;
            let activePage = arr.find(item => item.url == this.activeTab) || '/dashboard'
            document.title = activePage.title || '首页'
        }
    },
    computed:{
        editableTabs(){
            this.activeTab = this.$store.state.routerName.url || '/dashboard'
            // 若什么都不改，那么在删除标签的时候没有问题，而刷新的时候不会选中到当前标签
            // 若使用一次 this.activeTab ，那么删除标签的时候，不会选中，刷新的时候可以选中
            // 解决办法：由于登录页和导航页是同级路由，而导航页未做 keep-alive。即采用created解决
            // console.log(this.activeTab);
            document.title = this.$store.state.routerName.title || '绩效考核管理系统'
            return this.$store.state.navTabs
        }
    },
    created() {
        this.activeTab = this.$store.state.routerName.url || '/dashboard'
        console.log(this.activeTab);
    },
}