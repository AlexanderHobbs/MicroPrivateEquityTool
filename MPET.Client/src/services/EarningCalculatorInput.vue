<script setup>

import {ref, watch, useId} from 'vue'
import SingleInput from '@/components/basic/SingleInput.vue'
import CurrencyOutput from '@/components/ea_comp/CurrencyOutput.vue'
import EBITDAInput from '@/components/ea_comp/EBITDAInput.vue'
import EBITDAOutput from '@/components/ea_comp/EBITDAOutput.vue'

const props = defineProps({
    initialData: Object,
    sessionId: String
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

const inputDictionary = ref({})

const selectedYear = ref()
const yearOptions = ref([
    {text: '2025', value: 2025},
    {text: '2024', value: 2024},
    {text: '2023', value: 2023},
])

const currencyForm = ref({ 
    Revenue: null,
    Expense: null, 
    OwnerSalary: null, 
    ReportedSDE: null })

const addBackExist = ref(false)

const AddBackList = ref([])

const categoryOptions = ref([
    {text: "non-recurring", value: 1},
    {text: "discretionary", value: .75},
    {text: "questionable", value: .25},

])

const AddBackForm = ref({
        Id: useId(),
        Description: null,
        Amount: null,
        category: null,
        CategoryWeight: null,
        ConfidenceLevel: null
})

const EBITDAValuesExist = ref(false);

const EBITDAForm = ref({
    IsAvailable: addBackExist.value,
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
        Revenue: null,
        Expense: null,
        OwnerSalary: null,
        ReportedSDE: null
    })

    Object.assign(EBITDAForm.value, {
        InterestRate: null,
        Taxes: null,
        Depreciation: null,
        Amortization: null
    })

    selectedYear.value = null

}

function resetAddBackForm() {
    Object.assign(AddBackForm.value, {
        Id: generateId(),
        Description: null,
        Amount: null,
        category: null,
        ConfidenceLevel: 50
    })
}

// ----------------------
// Actions
// ----------------------


function add_AddBack() {


    if(Object.values(AddBackForm.value).some(val => val === null)) {
        alert("Please fill in all add backs!")
        return
    }

    AddBackList.value.push({
        ...AddBackForm.value
    })

    resetAddBackForm();

}


const emit = defineEmits(['save', 'load-year', 'calculate'])

function saveYear() {

    if (addBackExist.value && AddBackForm.value.description) {
        add_AddBack()
    }
    
    const payload = {
        Operating: {...currencyForm.value },
        Adjustments: { addBacks: AddBackList.value},
        Financials: {...EBITDAForm.value}
    }

    if (!dataIsNotNull(payload)) { 
        alert("Not all fiels have data entered — Please fill in all entries!");
        return 
    }   
    
    inputDictionary.value[selectedYear.value] = payload

    AddBackList.value = [];
    resetForm();

}

async function calculateEarnings(){

    const dictionary = {
        InputDictionary: {...inputDictionary.value}
    }

    const response = await fetch('/api/earning/calculate', {
      method: 'POST',
      headers: {'Content-Type': 'application/json', 
      'X-Session-Id' : props.sessionId
      },
      body: JSON.stringify(dictionary)
    })

    if (!response.ok) {
        const errorText = await response.text(); // Try to get server error details
        alert(`Server error: ${response.status} - ${errorText}`);
        return;
    }

    const data = await response.json();
    console.log(data)

    emit('calculate', data)
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
    <div class = "parent-container">
        <div class = "input-container">
            <h2>Calculate True Earnings</h2>
            
            <div class = "ea-input-container">
                <div class = "selected-year-input">
                    <label>Select Fiscal Year: </label>
                    <select class = "year_select" v-model = "selectedYear"  @change="loadYear(selectedYear)">
                        <option disabled value="">Select a Year</option>
                        <option v-for = "year in yearOptions" :key = "year.value" :value = "year.value">
                            {{ year.text }}
                        </option>
                    </select>
                </div>

                <div class = "currency-input-form">
                    <SingleInput label = "Revenue" v-model="currencyForm.Revenue"/>
                    <SingleInput label = "Expense" v-model="currencyForm.Expense"/>
                    <SingleInput label = "Reported SDE: " v-model = "currencyForm.ReportedSDE" />           
                    <SingleInput label = "Owner Salary" v-model="currencyForm.OwnerSalary"/>
                </div>

                <div class = "vertical-line"></div>

                <div class = "radio-btn-class">
                    <!-- <img  class = "label-icon" src = "../assets/navbar-icons/comparison.png"/> -->
                    <div>
                        <label>Owner Add Backs Exist? </label>
                        <input type="radio" :value ="true" v-model = "addBackExist">
                        <label for="yes-rd-btn">Yes</label>
                        <input type="radio" :value = "false" v-model = "addBackExist">
                        <label for="no-rd-btn">No</label>
                    </div>
                </div>

                <Transition name = "fade"> 
                <div class = "OwnerAddBacks" v-if = "addBackExist">
                    <div class = "addBackEntry">
                        <h4>Add Back Entry: </h4>
                        <label>Add Back Description: </label>
                        <textarea v-model = "AddBackForm.Description" placeholder="description"></textarea>
                        <label>Add Back Value: </label>
                        <input type="number" v-model.number = "AddBackForm.Amount">
                        <label>Add Back Category: </label>
                        <select v-model = "AddBackForm.category">
                            <option v-for="category in categoryOptions" :key = "category.text" :value="category.text">
                                {{ category.text }}
                            </option>
                        </select>
                        <!-- <input type="text" v-model = "AddBackForm.category"> -->
                        <label>Add Back Confidence Level:</label>
                        <input type="range" v-model.number = "AddBackForm.ConfidenceLevel" min = "0" max = "100">
                        <span class = "confidence-value">{{ AddBackForm.ConfidenceLevel }}%</span>

                        <button class = "addBack-btn" @click="add_AddBack()">Create Add Back</button>
                    </div>
                </div>
                </Transition>  

                <div class = "vertical-line"></div>

                <div class="radio-btn-class">
                    <!-- <img  class = "label-icon" src = "../assets/navbar-icons/comparison.png"/> -->
                    <div>
                        <label>Do EBITDA Records Exist?</label>
                        <input type="radio" :value = "true" v-model = "EBITDAValuesExist">
                        <label for="yes-rd-btn">Yes</label>
                        <input type="radio" :value = "false" v-model = "EBITDAValuesExist">
                        <label for="no-rd-btn">No</label>
                    </div>
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
                <button class = "save-btn" @click = "saveYear">Save Year</button>
                <button class = "save-btn" @click = "calculateEarnings">Calculate Earnings</button>
            </div>
        </div>

        <div class = "earning-output">
            <h2>Review</h2>
            <!-- Add option to scroll through yearly input using arrows (<>) -->

            <div class = "ea-output-container">

                <div class = "output-container"><CurrencyOutput :data = "currencyForm" :label = "selectedYear"/></div>

                <div class="output-container">
                    <div><hr></div>
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
                                    <tr v-if = "AddBackList.length" v-for = "item in AddBackList" :key = "item.id">
                                        <td>{{ item.Description }}</td>
                                        <td>{{ item.Amount }}</td>
                                        <td>{{ item.category }}</td>
                                        <td>{{ item.ConfidenceLevel }}</td>
                                    </tr>
                                    <tr v-else>
                                        <td colspan="4">
                                            <div class = "blank-table-data">
                                                <img src="../assets/top-navbar-icons/setting.png" alt="no add backs">
                                                <p>No add backs added yet</p>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div><hr></div>
                    <div class = "output-container">
                        <EBITDAOutput :data = "EBITDAForm"/>
                    </div>
            </div>
        </div>
    </div>
</template>


<style scoped>

.parent-container {
    display: flex;
    flex-direction: row;
    gap: 20px;
}

.vertical-line {
  border-top: 1px solid rgb(204, 204, 204);
  background-color: black;
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
    flex: .75;
    flex-direction: column;
    gap: 22px;
    box-sizing: border-box;
}

.input-container {
    display: flex;
    flex: 1;
    flex-direction: column;
    gap: 22px;
    box-sizing: border-box;
}

.ea-input-container {
    display: flex;
    flex: 1.50;
    flex-direction: column;
    gap: 22px;
    padding: 25px 20px;
    box-sizing: border-box;
    background-color: white;
    border-radius: 20px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0,0,0,0.05);
}

/* Shared input row style */
.selected-year-input {
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    padding: 12px 0px;
    min-width: none;
}

/* Label consistency */
.selected-year-input label {
    font-size: 16px;
    font-weight: 500;
    color: #374151;
    min-width: 140px;
    flex: 1;
}

.year_select {
    flex: 3;
    padding: 15px 10px;
}

.label-icon {
    height: 20px;
    width: auto;
    border-radius: 7px;
    background-color: #f1f2f2;
    padding: 8px;
}

.currency-input-form {
    display: flex;
    flex-direction: column;
    gap: 15px;
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

.radio-btn-class {
    display: flex;
    align-items: center;
    gap: 20px;
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
    padding: 20px;
    border: 1px solid rgba(0, 0, 0, 0.105);
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


.blank-table-data {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
}

.blank-table-data img {
    width: 60px;
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