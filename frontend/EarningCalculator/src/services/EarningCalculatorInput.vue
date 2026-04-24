<script setup>

import {ref, watch, useId} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue'
import CurrencyOutput from '@/components/ea_comp/CurrencyOutput.vue'
import EBITDAInput from '@/components/ea_comp/EBITDAInput.vue'
import EBITDAOutput from '@/components/ea_comp/EBITDAOutput.vue'

const props = defineProps({
    initialData: Object
})

watch(
    () => props.initialData,
    (data) => {
        if(!data) return

        Object.assign(currencyForm, data.operating)
        Object.assign(EBITDAForm, data.financials)

        AddBackList.value = data.adjustments.addBacks ? data.adjustments.addBacks.map(x => ({...x})) : []
    },
        
    {immediate: true}
)

// ----------------------
// State
// ----------------------

const selectedYear = ref()
const yearOptions = ref([
    {text: '2025', value: 2025},
    {text: '2024', value: 2024},
    {text: '2023', value: 2023},
])

const currencyForm = ref({
    revenue: null,
    expense: null,
    ownerSalary: null,
    ReportedSDE: null
})

const addBackExist = ref(false)

const AddBackList = ref([])

const categoryOptions = ref([
    {text: "non-recurring", value: 1},
    {text: "discretionary", value: .75},
    {text: "questionable", value: .25},

])

const AddBackForm = ref({
        id: useId(),
        description: null,
        price: null,
        category: null,
        confidenceLevel: null
})

const EBITDAValuesExist = ref(false);

const EBITDAForm = ref({
    InterestRate: null,
    Taxes: null,
    Depreciation: null,
    Amortization: null
})


// ----------------------
// Helpers
// ----------------------

function generateId() {
  return crypto.randomUUID()
}


function resetForm() {
 
   Object.assign(currencyForm.value, {
        revenue: null,
        expense: null,
        ownerSalary: null,
        ReportedSDE: null
    })

    Object.assign(EBITDAForm.value, {
        InterestRate: null,
        Taxes: null,
        Depreciation: null,
        Amortization: null
    })

    selectedYear.value = null
    addBackExist.value = false
    EBITDAValuesExist.value = false

}

function resetAddBackForm() {
    Object.assign(AddBackForm.value, {
        id: generateId(),
        description: null,
        price: null,
        category: null,
        confidenceLevel: 50
    })
}

// ----------------------
// Actions
// ----------------------


function add_AddBack() {


    if(Object.values(AddBackForm.value).some(val => val === null)) {
        alert("Please fill in all add back!")
        return
    }

    AddBackList.value.push({
        ...AddBackForm.value
    })

    resetAddBackForm();

}


const emit = defineEmits(['save', 'load-year'])

function submitYear() {
    
    const payload = {
        year: selectedYear.value,
        operating: {...currencyForm.value },
        adjustments: { addBacks: AddBackList.value},

        financials: {...EBITDAForm.value}
    }

    if(!dataIsNotNull(payload)){
        alert("Not all fiels have data entered — Please fill in all entries!");
        return;
    }

    emit('save', payload);
    alert("Loaded");

    AddBackList.value = [];
    resetForm();

}

function loadYear(year){
    emit('load-year', year)
}

function dataIsNotNull(obj) { 
    for(let key in obj){ 
        if(obj[key] === null){ 
            return false; 
        } 
        if(typeof obj[key] === 'object' && !dataIsNotNull(obj[key])){ 
            return false; } 
    } 
    
    return true;
}


</script>

<template>
<body>
    <div class = "parent-container">
        <div class = "ea-input-container">
            <h2>Calculate True Earnings</h2>

            <div class = "selected-year-input">
                <label>Select Fiscal Year: </label>
                <select v-model = "selectedYear"  @change="loadYear(selectedYear)">
                    <option disabled value="">Select a Year</option>
                    <option v-for = "year in yearOptions" :key = "year.value" :value = "year.value">
                        {{ year.text }}
                    </option>
                </select>
            </div>

            <div class = "currency-input-form">
                <SingleInput  label = "Revenue Amount: " v-model = "currencyForm.revenue"/>
                <SingleInput  label = "Expense Amount:" v-model = "currencyForm.expense" />
                <SingleInput label = "Reported SDE: " v-model = "currencyForm.ReportedSDE" />           
                <SingleInput label = "Owner Salary: " v-model = "currencyForm.ownerSalary" />                
            </div>

            <div class = "radio-btn-class">
                <label>Owner Add Backs Exist? </label>
                <input type="radio" :value ="true" v-model = "addBackExist">
                <label for="yes-rd-btn">Yes</label>
                <input type="radio" :value = "false" v-model = "addBackExist">
                <label for="no-rd-btn">No</label>
            </div>

            <Transition name = "fade"> 
            <div class = "OwnerAddBacks" v-if = "addBackExist">
                <div class = "addBackEntry">
                    <h4>Add Back Entry: </h4>
                    <label>Add Back Description: </label>
                    <textarea v-model = "AddBackForm.description" placeholder="description"></textarea>
                    <label>Add Back Value: </label>
                    <input type="number" v-model.number = "AddBackForm.price">
                    <label>Add Back Category: </label>
                    <select v-model = "AddBackForm.category">
                        <option v-for="category in categoryOptions" :key = "category.text" :value="category.text">
                            {{ category.text }}
                        </option>
                    </select>
                    <!-- <input type="text" v-model = "AddBackForm.category"> -->
                    <label>Add Back Confidence Level:</label>
                    <input type="range" v-model.number = "AddBackForm.confidenceLevel" min = "0" max = "100">
                    <span class = "confidence-value">{{ AddBackForm.confidenceLevel }}%</span>

                    <button class = "addBack-btn" @click="add_AddBack()">Create Add Back</button>
                </div>
            </div>
            </Transition>  

            <div class="radio-btn-class">
                <label>Do EBITDA Records Exist?</label>
                <input type="radio" :value = "true" v-model = "EBITDAValuesExist">
                <label for="yes-rd-btn">Yes</label>
                <input type="radio" :value = "false" v-model = "EBITDAValuesExist">
                <label for="no-rd-btn">No</label>
            </div>

            <Transition name = "fade"> 
                <div class="EBITDA" v-if = "EBITDAValuesExist">
                        <h4>Add EBITDA Values: </h4>
                        <EBITDAInput label = "Interest Rate" v-model = "EBITDAForm.InterestRate"/>
                        <EBITDAInput label = "Taxes" v-model = "EBITDAForm.Taxes"/>
                        <EBITDAInput label = "Depreciation" v-model = "EBITDAForm.Depreciation"/>
                        <EBITDAInput label = "Amortization" v-model = "EBITDAForm.Amortization"/>
                </div>
            </Transition>
            <button class = "save-btn" @click = "submitYear">Save Data</button>

        </div>

        <div class = "earning-output">
        <h1>Review</h1>

        <div class = "ea-output-container">

            <div class = "output-container"><CurrencyOutput :data = "currencyForm" :label = "selectedYear"/></div>

            <div class="output-container">
                <div class = "AddBackList">
                        <h4>Add Backs</h4>
                        <table class = "Table">
                            <thead>
                                <tr>
                                    <th>Description</th>
                                    <th>Price</th>
                                    <th>Category</th>
                                    <th>Confidence Level</th>
                                </tr>
                            </thead>
                            <tbody>
                            <tr v-for = "item in AddBackList" :key = "item.id">
                                <td>{{ item.description }}</td>
                                <td>{{ item.price }}</td>
                                <td>{{ item.category }}</td>
                                <td>{{ item.confidenceLevel }}</td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class = "output-container">
                    <EBITDAOutput :data = "EBITDAForm"/>
                </div>
        </div>
        </div>
    </div>
</body>

</template>


<style scoped>

.parent-container {
    display: flex;
}

.ea-output-container {
    display: flex;
    flex-direction: column;
    gap: 22px;
    background: rgba(255, 255, 255, .75);
    padding: 20px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
    position: sticky;
    top: 20px;
    margin-bottom: 55px;
}

.earning-output {
    display: flex;
    flex: 1;
    flex-direction: column;
    gap: 22px;
    padding: 20px;
    box-sizing: border-box;
}

.ea-input-container {
    display: flex;
    flex: 1.50;
    flex-direction: column;
    gap: 22px;
    padding: 20px;
    box-sizing: border-box;
}

/* Shared input row style */
.selected-year-input {
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    min-width: none;
}

/* Label consistency */
.selected-year-input label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
    min-width: 140px;
}

.currency-input-form {
    display: flex;
    flex-direction: column;
    gap: 22px;
}

/* Inputs */
input, select, textarea {
    padding: 10px 12px;
    border-radius: 8px;
    border: 1px solid transparent;
    box-sizing: border-box;
    outline: none;
    font-size: 13px;
    background: #f9fafb;
    transition: all 0.2s ease;
    font-family: 'Arial', sans-serif;
}

input:hover, select:hover, textarea:hover {
    background: #f3f4f6;
}

input:focus, select:focus, textarea:focus {
    background: #ffffff;
    border-color: #111827;
    box-shadow: 0 0 0 2px rgba(0,0,0,0.05);
}

/* AddBack section container */
.OwnerAddBacks {
    display: flex;
    gap: 24px;
    background: #ffffff;
    padding: 18px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
    box-sizing: border-box;
}

/* Left form */
.addBackEntry {
    display: flex;
    flex-direction: column;
    gap: 10px;
    flex: 1;
}

/* Table container */
.AddBackList {
    flex: 1;
    margin-top: 0;
}

/* Table styling */
.Table {
    width: 100%;
    border-collapse: collapse;
    font-size: 12px;
}

.Table th {
    text-align: left;
    padding: 8px;
    background: #f3f4f6;
    color: #374151;
    font-weight: 600;
    border-bottom: 1px solid #e5e7eb;
}

.Table td {
    padding: 8px;
    border-bottom: 1px solid #f1f1f1;
    color: #111827;
}

/* EBITDA section styled like a card */
.EBITDA {
    display: flex;
    flex-direction: column;
    gap: 10px;
    background: #ffffff;
    padding: 18px;
    border-radius: 14px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

/* Button styling */
.save-btn, .addBack-btn {
    width: fit-content;
    padding: 10px 14px;
    border-radius: 10px;
    border: none;
    background: #111827;
    color: #ffffff;
    font-size: 13px;
    cursor: pointer;
    transition: all 0.15s ease;
}

.save-btn:hover, .addBack-btn:hover {
    background: #1f2937;
    transform: translateY(-1px);
}

.save-btn:active, .addBack-btn:active {
    transform: translateY(0);
}

.confidence-value {
    font-size: 12px;
    color: #6b7280;
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}


</style>