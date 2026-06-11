import './assets/main.css'
import { createApp } from 'vue'
import App from '@/App.vue'
import vuetify from '@/plugins/veutify.js'


const app = createApp(App)
app.use(vuetify)
app.mount('#app')