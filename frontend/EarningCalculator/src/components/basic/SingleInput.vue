<!-- CurrencyInput.vue -->

<script setup>

const prop = defineProps({
    label: {type: String, required: true},
    modelValue: {type: [Number, String, null], default: null},
    placeholder: {type: String, default: "00.00"},
    inputType: {type: Number, default: 1},
    style: {type: String, default: "single-input"}
});

const emit = defineEmits(['update:modelValue']);

const onInput = (event) => {
    const value = event.target.value;

    if(prop.inputType === 1){
        const parsed = value === "" ? null : Number(value);
        emit("update:modelValue", parsed);
    }else{
        emit("update:modelValue", value);
    }

}

// const nonDigit = ref()
// const checkDigit = (event) => {
//     if (isNaN(Number(event.key))) {
//         nonDigit = false
//         event.preventDefault(); // Stop the character from being entered
//     }else{
//         nonDigit = true
//     }
// }
</script>

<template>
  <div :class = "style" v-if = "inputType === 1" >
    <label>{{ label }}</label>
    <input type="number" :placeholder= "placeholder" :value="modelValue" @input="onInput" />
  </div>

  <div :class = "style" v-if = "inputType === 2" >
    <label>{{ label }}</label>
    <input type="text" :value="modelValue" @input="onInput" />
  </div>

  <div :class = "style" v-if = "inputType === 3" >
    <label>{{ label }}</label>
    <textarea :value = "modelValue" @input="onInput"></textarea>
  </div>
  

    <!-- <Teleport to = "body">
        <div v-show = "nonDigit" class = "error-character">
            <p>Please enter a number</p>
            <button @click="nonDigit = true">Close</button>
        </div>
    </Teleport> -->
</template>


<style scoped>

.single-input {
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    gap: 20px;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
}

.no-border{
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    gap: 20px;
    padding: 12px 14px;
}

/* Label consistency */
.single-input label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
}

input {
    padding: 10px 12px;
    border-radius: 8px;
    border: 1px solid transparent;
    outline: none;
    font-size: 13px;
    background: #f9fafb;
    transition: all 0.2s ease;
    font-family: 'Arial', sans-serif;
}

input:hover {
    background: #f3f4f6;
}

input:focus {
    background: #ffffff;
    border-color: #111827;
    box-shadow: 0 0 0 2px rgba(0,0,0,0.05);
}
</style>