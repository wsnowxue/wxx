import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const store = new Vuex.Store({
    // 定义状态
    state: {
        routerArray:[],
        // 当前nav
        routerName:{},
        // 已有的tab nav
        navTabs:[{title:'首页',name:'/dashboard',url:'/dashboard'}],

        // filialeJudgeManageAdd数据
        // tree:[],
        // arr:[]
        // 宽度
        width:150,
        width200:200,
        width240:240,
        width290:290,
        // 用户信息
        userData:{},
        // 按钮权限
        buttonAuthority:{},
        // 存储菜单，做首页四个按钮的跳转
        menuList:[],
        name:''
    },
    mutations:{
        setName(state,name){
            state.name = name
        },
        setRouterName(state,obj){
            obj = obj || {}
            for (const key in obj) {
                if(typeof obj[key] == 'string'){
                    obj[key].replace('Detail','')
                }
            }
            state.routerName = obj;
            
            // console.log(obj);
            
            let flag = state.navTabs.find(item=>item.url == obj.url)
            if(!flag){
                state.navTabs.push(obj)
            }
        },
        deleteRouterName(state,str){
            state.navTabs = state.navTabs.filter(item=>item.url!=str)
        },
        resetRouterName(state){
            state.navTabs = [{title:'首页',name:'/dashboard',url:'/dashboard'}]
        },

        // 存储用户信息
        saveUesrData(state,obj){
            state.userData = obj
            // console.log(obj);
            
        },
        changeToArray(state,arr){
            // console.log(arr);
            let objT = {};
            let objArr = {};
            let arrObj = [];
            let arrT = [];
            let count = 1
            arr.forEach((item) => {
                item.span? item.span= 0:''
                if(objT[item.dimension_id]){
                    objT[item.dimension_id]++
                    objArr[item.dimension_id].push(item)
                    
                }else{
                    objT[item.dimension_id] = 1
                    objArr[item.dimension_id] = [item]
                }
                // arrT.push(item)
            });
            for (const key in objArr) {
                arrObj.push(...objArr[key])
            }
            // console.log(JSON.stringify(arrObj));
            for (const key in objT) {
                arrObj.find((item)=>{
                    if(item.dimension_id == key){
                        item.span = objT[key]
                        return true
                    }
                    if(!item.dimension_id){
                        item.span = 1
                        // return true
                    }
                })
            }
            arr.splice(0)
            arr.push(...arrObj)

            // console.log(arrT);
            // console.log(objT);
            console.log(arr);
            
            
            
        },
        saveButtonAuthority(state,obj){
            state.buttonAuthority = obj
        },
        saveMenuList(state,arr){
            state.menuList = JSON.parse(JSON.stringify(arr))
        }
        // // 将filialeJudgeManageAdd数据转为树的形式
        // toTree(state,arr){
        //     let arr1 = [];
        //     let obj = {}
        //     arr.forEach((item)=>{
        //         if(item.span!=0){
        //             obj = {
        //                 name1:'123',
        //                 children:[item]
        //             }
        //             arr1.push(obj)
        //         }else{
        //             arr1[arr1.length-1].children.push(item)
        //         }
        //     })
        //     state.tree = arr1
        //     console.log(arr1);
        // },
        // // 将filialeJudgeManageAdd数据转为数组的形式
        // toArray(state,tree){
        //     var arr = []
        //     for(let i = 0;i<tree.length;i++){
        //         let children = tree[i].children
        //         if(children.length == 0){
        //             arr.push(children[0])
        //         }else{
        //             arr.push(children[0])
        //             for(let j = 1;j<children.length;j++){
        //                 children.span=children.length
        //                 arr.push(children[j])
        //             }
        //         }
        //     };
        //     console.log(arr);
            
        // }
    }
})

export default store