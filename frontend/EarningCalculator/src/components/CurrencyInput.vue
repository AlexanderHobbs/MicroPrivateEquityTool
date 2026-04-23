<!-- CurrencyInput.vue -->

<script setup>

const prop = defineProps({
    label: {type: String, required: true},
    modelValue: {type: Number, default: null},
});

const emit = defineEmits(['update:modelValue']);

const onInput = (event) => {
    const value = event.target.value;
    const parsed = value === "" ? null : Number(value);
    emit("update:modelValue", parsed);
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
  <div class = "currency-input">
    <label>{{ label }}</label>
    <input type="number" placeholder="00.00" :value="modelValue" @input="onInput" />
  </div>

    <!-- <Teleport to = "body">
        <div v-show = "nonDigit" class = "error-character">
            <p>Please enter a number</p>
            <button @click="nonDigit = true">Close</button>
        </div>
    </Teleport> -->
</template>


<style scoped>

.currency-input {
    display: flex;
    align-items: center;
    gap: 16px;
    background: #ffffff;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
}

/* Label consistency */
.currency-input label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
    min-width: 140px;
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