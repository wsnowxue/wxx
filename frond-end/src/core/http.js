import axios from 'axios'
import message from 'element-ui'
import store from '@/vuex/store.js'
import router from '../router'
let data = store.state.userData
let params = {
    UserId:data?data.UserId:''
}
axios.defaults.params = {...params}
// console.log(axios.defaults);
// axios.defaults.baseURL = 'http://192.168.2.1:3000/'
// axios.defaults.baseURL = 'http://10.10.16.110:3400/api/'
axios.interceptors.response.use((response) => {
    if(response){
        if (response.status == 200) {
            let data = response.data
            if(data.state == 'error'){
                message.Message({
                    showClose: true,
                    message: data.message,
                    type: 'error'
                });
            }
            // if(data.state == 1){
            //     message.Message({
            //         showClose: true,
            //         message: data.message,
            //         type: 'success'
            //     });
            // }
            if(data.state == 4){
                message.Message({
                    showClose: true,
                    message: data.message,
                    type: 'error'
                });
            }
            if(data.state == 3){
                message.Message({
                    showClose: true,
                    message: data.message,
                    type: 'error'
                });
            }
            return data
        }
        if (response.status == 304) {
            console.log('没权限');
            return response
        }
    }
    return response;
}, function (error) {
    message.Message({
        showClose: true,
        message: '网络或服务器原因，请求失败，请检查网络设置',
        type: 'error'
    });
    if (error.response) {
        // console.error(error);
    }else{
        // console.error(error.message);
    }
    return {}
});
export default axios