import {defineStore} from 'pinia';
import {ref} from 'vue';

export const useCalculationStore = defineStore('calculation', () => {
    const sessionId = ref(localStorage.getItem('sessionId'))

    const earningResult = ref(null)
    const debtResult = ref(null)
    const dscrResult = ref(null)

    function createSession(){
        const id = crypto.randomUUID()
        localStorage.setItem('sessionId', id)
        return id
    }

    function clearAll() {
        earningResult.value = null;
        debtResult.value = null;
        dscrResult.value = null;
        localStorage.removeItem('sessionId')
    }

    return {sessionId, earningResult, debtResult, dscrResult, clearAll}

})