<script setup>

import {ref} from 'vue'


const selectedYear = ref()
const selectedYear_Options = ref([
    {text: '2025', value: 2025},
    {text: '2024', value: 2024},
    {text: '2023', value: 2023},
])

const revenueAmount = ref('')
const expenseAmount = ref('')
const SDE = ref('')
const ownerSalary = ref('')
const addBackExist = ref(false)
const addBackDescription = ref('')
const addBackPrice = ref('');
const addBackCategory = ref('');
const addBackConfidence = ref('');

const AddBackList = ref([
    {
        description: "",
        price: "",
        category: "",
        confidenceLevel: ""
    }
])

const success = ref(false)

async function add_AddBack() {
    try{
    AddBackList.value.push({
    description: addBackDescription.value,
    price: addBackPrice.value,
    category: addBackCategory.value,
    confidenceLevel: addBackConfidence.value
    })

    addBackDescription.value = ""
    addBackPrice.value = ""
    addBackCategory.value = ""
    addBackConfidence.value = ""
    
    success.value = true;
    }catch (err){
        console.error(err);
        success.value = false;
    }
}

const EBITDAValuesExist = ref(false);

const InterestRate = ref('');
const Taxes = ref('');
const Depreciation = ref('');
const Amortization = ref('');

defineExpose({
    selectedYear,
    revenueAmount,
    SDE,
    if(addBackExist){
        AddBackList
    },
    if(EBITDAValuesExist){
        InterestRate,
        Taxes,
        Depreciation,
        Amortization
    }
})

</script>

<template>

<div class = "parent-container">

    <div class = "selected-year-input">
        <label for="SelectedYearInput">Select Fiscal Year: </label>
        <select name="SelectedYearInput" id="InputYear" v-model = "selectedYear">
            <option disabled value="">Select a Year</option>
            <option v-for = "year in selectedYear_Options" :key = "year.value" :value = "year.value">
                {{ year.text }}
            </option>
        </select>
        <p>Selected Year: {{ selectedYear }}</p>
    </div>

    <div class = "currency-input">
        <label for="revenue">Revenue Amount: </label>
        <input type="text" v-model = "revenueAmount" placeholder="00.00">
        <p>{{ revenueAmount }}</p>

    </div>
    <div class="currency-input">
        <label for="expense">Expense Amount: </label>
        <input type="text" v-model = "expenseAmount" placeholder = "00.00">
        <p>{{ expenseAmount }}</p>
    </div>

    <div class="currency-input">
        <label for="SDE">SDE: </label>
        <input type="text" v-model = "SDE" placeholder = "00.00">
        <p>{{ SDE }}</p>
    </div>

    <div class="currency-input">
        <label for="expense">Owner Salary: </label>
        <input type="text" v-model = "ownerSalary" placeholder = "00.00">
        <p>{{ ownerSalary }}</p>
    </div>

     <div class = "radio-btn-class">
        <label for="hasAddBacks?">Owner Add Backs Exist for year {{ selectedYear }}?: </label>
        <input type="radio" :value ="true" v-model = "addBackExist">
        <label for="yes">Yes </label>
        <input type="radio" :value = "false" v-model = "addBackExist">
        <label for="no">No </label>
    </div>

    <div class = "OwnerAddBacks" v-if = "addBackExist">

        <div class = "addBackEntry">
            <h4>Add claimed add backs (as many as needed): </h4>
            <label for="AddBack-Description">Add Back Description: </label>
            <textarea v-model = "addBackDescription" placeholder="description..."></textarea>
            <label for="AddBack-Value">Add Back Value: </label>
            <input type="text" v-model = "addBackPrice">
            <label for="AddBack-Category">Add Back Category: </label>
            <input type="text" v-model = "addBackCategory">
            <label for = "AddBack-ConfidenceLevel">Add Back Confidence Level:</label>
            <input type="text" v-model = "addBackConfidence">

            <button @click="add_AddBack()">Create Add Back</button>
        </div>
        <div class = "AddBackList" v-if = "success">
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
                <tr v-for = "addBacks in AddBackList">
                    <td>{{ addBacks.description }}</td>
                    <td>{{ addBacks.price }}</td>
                    <td>{{ addBacks.category }}</td>
                    <td>{{ addBacks.confidenceLevel }}</td>
                </tr>
                </tbody>
            </table>
        </div>

    </div>

    <div class="radio-btn-class">
        <label for="hasEBITDA?">Do EBITDA Records Exist for year {{ selectedYear }}?</label>
        <input type="radio" :value = "true" v-model = "EBITDAValuesExist">
        <label for="yes-rd-btn">Yes</label>
        <input type="radio" :value = "false" v-model = "EBITDAValuesExist">
        <label for="no-rd-btn">No</label>
    </div>

    <div class="EBITDA" v-if = "EBITDAValuesExist">
            <h4>Add EBITDA Values: </h4>
            <label for="Interest">Interest Rate: </label>
            <input type = "text" v-model = "InterestRate">
            <label for="AddBack-Value">Taxes: </label>
            <input type="text" v-model = "Taxes">
            <label for="AddBack-Category">Depreciation: </label>
            <input type="text" v-model = "Depreciation">
            <label for = "AddBack-ConfidenceLevel">Amortization:</label>
            <input type="text" v-model = "Amortization">
    </div>

</div>



</template>

<style scoped>

.parent-container {
    display: flex;
    flex-direction: column;
    gap: 20px;
    background-color: gainsboro;
    padding: 20px;
}


.selected-year-input{
    display: flex;
    gap: 20px;
    align-items: center;
}

.currency-input{
    display: flex;
    flex-direction: row;
    gap: 20px;
    align-items: center;
}

.OwnerAddBacks{
    display: flex;
    flex-direction: row;
    width: auto;
    gap: 40px;
    background-color: white;
    padding: 20px;
}

.addBackEntry{
    display: flex;
    flex-direction: column;
}

.AddBackList{
    margin-top: 20px;
}

.OwnerAddBacks input{
    margin: 0px 0px 10px 0px;
}

.OwnerAddBacks button:hover {
    background-color: deepskyblue;
}

.OwnerAddBacks textarea {
    field-sizing: content;
    min-height: 50px;
}

.EBITDA {
    display: flex;
    flex-direction: column;
    background-color: white;
    width: auto;
    padding: 20px;
}

</style>