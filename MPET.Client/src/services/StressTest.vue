<script setup>
import RangeInput from '@/components/basic/RangeInput.vue';
import SingeOutput from '@/components/basic/SingeOutput.vue';
import SingleInput from '@/components/basic/SingleInput.vue';
import { watch, ref } from 'vue';


const props = defineProps({sessionId: String})

watch(
    () => props.sessionId,
    (newId) => {
        if (newId) loadData()
    },
    { immediate: true }
)

const replicatedData = ref({
    nerveLevel: 0,
    revenue: 0,
    expense: 0,
    revenueDrop: 0,
    marginLevelShift: 0,
    interestRateShift: 0
})

const data = ref();

async function loadData(){
    try{

        const response = await fetch("/api/stresstest/default", {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json', 
                'X-Session-Id' : props.sessionId
            }
        });


        if (!response.ok) {
            const errorText = await response.text(); // Try to get server error details
            alert(`Server error: ${response.status}`);
            return;
        }

        replicatedData.value = await response.json();

    }catch(err){
        console.error('Fetch failed:', err.name, err.message);
    }
}

const emit = defineEmits(['update'])

async function onInput(){
    try{

        const stData = {
            stModel: {...replicatedData.value}
        };

        console.log(stData);
        
        const response = await fetch("/api/stresstest/calculate", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json', 
                'X-Session-Id' : props.sessionId
            },
            body: JSON.stringify(stData)
        });

        if (!response.ok) {
            const errorText = await response.text(); // Try to get server error details
            alert(`Server error: ${response.status}`);
            return;
        }

        data.value = await response.json();
        emit('update', data)

    }catch(err){
        console.error('Fetch failed:', err.name, err.message);
    }
}


</script>

<template>
    <div class="parent-container">
        <div class="input-container">
            <h4>Calculate Stress Revenue</h4>
            <RangeInput label = "Nerve Levels" v-model = "replicatedData.nerveLevel" />
            <RangeInput label = "Revenue" v-model = "replicatedData.revenue"/>
            <RangeInput label = "Expense" v-model = "replicatedData.expense" />
            <RangeInput label = "Revenue Drop (%)" type = "percent" v-model = "replicatedData.revenueDrop" />
            <RangeInput label = "Margin Levels Shift" v-model = "replicatedData.marginLevelShift"/>
            <RangeInput label = "Interest Rate Change" type = "percent" v-model = "replicatedData.interestRateShift"/>
            <button @click = "onInput">Save</button>
        </div>

        <div class="output-container">
            <SingeOutput label = "Adjusted Profit"/>
            <SingeOutput label = "Updated DSCR" />
            <SingeOutput label = "Updated cash Flow after Debt"/>
            <SingeOutput label = "Suggestion" />
        </div>

    </div>

</template>

<style scoped>
.parent-container {
    flex: 1;
    display: flex;
    gap: 22px;
    box-sizing: border-box;
}

.output-container {
    display: flex;
    flex: 1;
    flex-direction: column;
    box-sizing: border-box;
}

.input-container {
    display: flex;
    flex: .75;
    flex-direction: column;
    gap: 22px;
    box-sizing: border-box;
}
</style>