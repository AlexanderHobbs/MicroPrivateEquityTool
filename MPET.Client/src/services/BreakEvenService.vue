<script setup>
import SingleInput from '@/components/basic/SingleInput.vue';
import {ref, computed} from 'vue'

const fixedCostNum = ref(1);

const fixedCostTotal = ref(0);

let nextId = 0;
const fixedCostItems = ref([])

const totalFixedCost = computed(() =>
    fixedCostItems.value.reduce((sum, item) => sum + Number(item.amount), 0)
)

function addFixedCost(){
    fixedCostItems.value.push({ id: nextId++, label: '', amount: 0 })
}

function removeFixedCost(data){
    fixedCostItems.value = fixedCostItems.value.filter(item => item.id !== id)
}

</script>

<template>
    <div class = "parent-container">
        <div class = "input-container">
            <SingleInput label = "Gross Revenue for most Current Year" />
            <SingleInput label = "Monthly Debt Service: "/>

            <div class = "fixed-cost-container">
            <h4>Fixed Cost</h4>
            <button @click="addFixedCost">+</button>
            <div
                v-for="item in fixedCostItems"
                :key="item.id"
                class="fixed-cost"
            >
                <div>
                    <input v-model="item.label"  placeholder="Description" />
                    <input v-model="item.amount" placeholder="Amount" type="number" />
                </div>
                <button @click="removeFixedCost(item.id)">−</button>
            </div>
            <button @click="addFixedCost">+ Add Fixed Cost</button>
            <p>Total Fixed Costs: {{ totalFixedCost }}</p>
            </div>

            <div>
            <h4>Variable Cost %</h4>
            <SingleInput label = "COGS"/>
              
            </div>
        </div>
    </div>
</template>

<style scoped>


.fixed-cost {
    display: flex;
    gap: 20%;
}

.fixed-cost :nth-child(1) {
    flex: 1;
    display: flex;
}

</style>