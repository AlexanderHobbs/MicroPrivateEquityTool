<script setup>

import { ref, onMounted } from 'vue';
import LoadingScreen from '@/components/basic/LoadingScreen.vue';
import SummaryIcon from '../assets/loading-icons/summary.png'


const props = defineProps({sessionId: String})

onMounted(loadData())

const earningData = ref(null)
const debtData = ref(null)
const dscrData = ref(null)
const stData = ref(null)
const beData = ref(null)


async function loadData(){
    try{

        earningData.value = await fetchJson("api/central/earning", props.sessionId)
        debtData.value = await fetchJson("api/central/debt", props.sessionId)
        dscrData.value = await fetchJson("api/central/dscr", props.sessionId)
        stData.value = await fetchJson("api/central/stest", props.sessionId)
        beData.value = await fetchJson("api/central/breakeven", props.sessionId)

        success.value = true;

    }catch(err){

    }
}

async function fetchJson(url, sessionId) {
    const response = await fetch(url, {
        method: 'GET',
        headers: {                          // headers were missing the 'headers' key
            'Content-Type': 'application/json',
            'X-Session-Id': sessionId
        }
    })
    if (!response.ok) {
        const text = await response.text()
        throw new Error(`${response.status}: ${text}`)
    }
    return response.json()
}


const success = ref(false)
</script>

<template>
    <div v-if = "success">
    <h4>Summary Dashboard</h4>
    </div>
    <div v-else>
        <LoadingScreen :iconSrc = "SummaryIcon"/>
    </div>
</template>

<style scoped>

</style>